using UnityEngine;
using System.Collections;

public class ContinuosHitObject : MonoBehaviour {

	[System.Serializable]
	private class LocalTransform
	{
		public Vector3 position;
		public Vector3 rotation;
		public Vector3 scale;
	}

	[SerializeField]
	private LocalTransform localTransform;
	public ContinuousAttack m_ContinuosAttack;

	// 親の位置からのローカルステータス設定
	public void SetLocalTransform()
	{
		transform.localPosition = localTransform.position;
		transform.localRotation = Quaternion.Euler(localTransform.rotation.x, localTransform.rotation.y, localTransform.rotation.z);
		transform.localScale	= localTransform.scale;
	}

	// 触れた瞬間に判定
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag != "hitObject") return;

		// 攻撃が効くオブジェクトに付与しているIDを渡す
		if (m_ContinuosAttack.CheckHadHit(0))
		{
			// ダメージ判定とか

		}
	}
}
