using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class UguiTextRotate : UIBehaviour, IMeshModifier
{
	[SerializeField, Range(0.0f, -45.0f)]
	private float m_Angle;

	private const int ONE_TEXT_VERTEX_NUM = 6;

	public new void OnValidate()
	{
		base.OnValidate();

		Graphic graphic = GetComponent<Graphic>();
		if (graphic != null) graphic.SetVerticesDirty();
	}

	public void ModifyMesh(Mesh mesh)
	{
		throw new NotImplementedException();
	}

	public void ModifyMesh(VertexHelper verts)
	{
		List<UIVertex> stream = ListPool<UIVertex>.Get();
		verts.GetUIVertexStream(stream);

		modify(ref stream);

		verts.Clear();
		verts.AddUIVertexTriangleStream(stream);
		ListPool<UIVertex>.Release(stream);
	}

	void modify( ref List<UIVertex> stream)
	{
		// 一文字6頂点なので6で割った数がテキスト数(空白も含める)
		//Debug.LogFormat("文字数 : {0}", stream.Count / ONE_TEXT_VERTEX_NUM);

		for(int i = 0; i < stream.Count; i += ONE_TEXT_VERTEX_NUM)
		{
			// 文字の中央を取得(i, i+3の間)
			Vector2 center = Vector2.Lerp(stream[i].position, stream[i + 3].position, 0.5f);
			// 頂点を回す
			for(int j = 0; j < ONE_TEXT_VERTEX_NUM; ++j)
			{
				UIVertex	vert	= stream[i + j];
				Vector3		pos		= vert.position - (Vector3)center;
				Vector2		newPos = new Vector2(
					pos.x * Mathf.Cos(m_Angle * Mathf.Deg2Rad) - pos.y * Mathf.Sin(m_Angle * Mathf.Deg2Rad),
					pos.x * Mathf.Sin(m_Angle * Mathf.Deg2Rad) + pos.y * Mathf.Cos(m_Angle * Mathf.Deg2Rad)
					);

				vert.position = (Vector3)(newPos + center);
				stream[i + j] = vert;
			}
		}

	}

}
