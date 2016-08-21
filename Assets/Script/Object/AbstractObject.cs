using UnityEngine;
using System.Collections;

// ダメージ判定があるオブジェクトの基底クラス
public class AbstractObject : MonoBehaviour {

	[SerializeField]
	private ObjectCommonParameter m_Parameter;
	
	public ObjectCommonParameter GetParameter() { return m_Parameter; }
	
	protected virtual void Damage()
	{
		Debug.LogFormat("{0}Damage, NowHitPoint：{1}", 0, m_Parameter.hitPoint);
	}

	protected virtual void Dead()
	{

	}

	void OnTriggerStay( Collider collider )
	{
		if (collider.gameObject.tag != "PlayerAttackObject") return;

		Debug.Log(collider.tag);

		ContinuosHitObject obj = collider.GetComponent<ContinuosHitObject>();

		// 攻撃が効くオブジェクトに付与しているIDを渡す
		if (obj.m_ContinuosAttack.CheckHadHit(m_Parameter.id))
		{
			// ダメージ判定とか
			Debug.Log("damage");
		}

		Damage();
	}
}
