using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ContinuosHitObject : MonoBehaviour {

	[System.Serializable]
	private class LocalTransform
	{
		public Vector3 position;
		public Vector3 rotation;
		public Vector3 scale;
	}

	[SerializeField]
	private LocalTransform	m_LocalTransform;
	public ContinuousAttack m_ContinuosAttack;

	// 親の位置からのローカルステータス設定
	public void SetLocalTransform()
	{
		transform.localPosition = m_LocalTransform.position;
		transform.localRotation = Quaternion.Euler(m_LocalTransform.rotation.x, m_LocalTransform.rotation.y, m_LocalTransform.rotation.z);
		transform.localScale	= m_LocalTransform.scale;
	}

#if UNITY_EDITOR
	// Editorで設定したtransformをスクリプトにすぐ反映できるようにするため作成
	[CustomEditor(typeof(ContinuosHitObject))]
	public class ContinuosHitObjectEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			ContinuosHitObject hitObject = target as ContinuosHitObject;
			if ( GUILayout.Button("LocalTransform更新") )
			{
				hitObject.m_LocalTransform.position = hitObject.transform.position;
				hitObject.m_LocalTransform.rotation = hitObject.transform.rotation.eulerAngles;
				hitObject.m_LocalTransform.scale	= hitObject.transform.localScale;
			}
		}
	}
#endif
}
