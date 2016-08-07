using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

// 宴のスライドを見ながら既存のアウトラインを改造
// アウトラインが破綻しないようユーザーが調整可能なように
public class UguiRichOutLine : Outline {

	[SerializeField, Tooltip("文字の影を生成する個数 太めやくっきりしたアウトラインを描く時はこの値を大きくする")]
	private int copyCount = 4;

	public override void ModifyMesh(VertexHelper vh)
	{
		if (!IsActive()) return;

		var verts = ListPool<UIVertex>.Get();
		vh.GetUIVertexStream(verts);

		int start = 0;
		int end = verts.Count;

		// 円状に影を並べる
		for (int i = 0; i < copyCount; i++)
		{
			float x = Mathf.Sin(Mathf.PI * 2 * i / copyCount) * effectDistance.x;
			float y = Mathf.Cos(Mathf.PI * 2 * i / copyCount) * effectDistance.y;

			ApplyShadow(verts, effectColor, start, verts.Count, x, y);
			start = end;
			end = verts.Count;
		}

		vh.Clear();
		vh.AddUIVertexTriangleStream(verts);
		ListPool<UIVertex>.Release(verts);
	}
}
