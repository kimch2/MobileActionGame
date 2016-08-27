using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

// 攻撃時に判定を行うオブジェクト
// 予めダメージ算出用のパラメーターを渡して対象が触れた瞬間にダメージを算出して対象に渡す
public class ContinuosHitObject : MonoBehaviour {

	[System.Serializable]
	private class LocalTransform
	{
		public Vector3 position;
		public Vector3 rotation;
		public Vector3 scale;
	}

	// ダメージ算出用パラメーター
	private class DamageParameter
	{
		public int attack;
	}

	[SerializeField]
	private LocalTransform		m_LocalTransform;
	private DamageParameter		m_DamageParameter = new DamageParameter();
	public	ContinuousAttack	m_ContinuosAttack;

	// ダメージの元となるパラメーターを設定
	// ※今は攻撃力のみもらう
	public void SetDamageParameter(int attack)
	{
		m_DamageParameter.attack = attack;
	}

	// ダメージを渡す
	// もし今後拡張する時はダメージ用クラス渡すことになるかも
	public int GetDamage() { return m_DamageParameter.attack; }

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
