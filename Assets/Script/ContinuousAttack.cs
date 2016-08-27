using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 連続攻撃を行うオブジェクトにアタッチすること
public class ContinuousAttack : MonoBehaviour {

	[SerializeField] private ContinuosHitObjectList m_HitObjectDatas;
	[SerializeField] private AbstractObject			m_AbstractObject;
	private List<GameObject>	hitObjects		= new List<GameObject>();
	private List<int>			hitObjectIds	= new List<int>();

	// Use this for initialization
	void Start () {
		// 判定オブジェクトの準備
		for(int i = 0; i < m_HitObjectDatas.hitChildObjects.Count; ++i)
		{
			ContinuosHitObject obj = Instantiate<ContinuosHitObject>(m_HitObjectDatas.hitChildObjects[i]);
			obj.transform.SetParent(gameObject.transform);

			// 予め設定した値を設定
			Debug.Log(m_AbstractObject.GetParameter().attack);
			obj.SetDamageParameter(m_AbstractObject.GetParameter().attack);
			obj.SetLocalTransform();

			// 自分を判定オブジェクトの変数に格納したら非アクティブ化の後に配列に格納
			obj.m_ContinuosAttack = this;
			obj.gameObject.SetActive(false);
			hitObjects.Add(obj.gameObject);
		}

		Debug.Log(hitObjects.Count);
	}
	
	// 既に判定を行ったオブジェクトかどうかを判断する
	public bool CheckHadHit(int id)
	{
		for(int i = 0; i < hitObjectIds.Count; ++i)
		{
			if (hitObjectIds[i] == id) return true;
		}
		// リストに格納しておく
		hitObjectIds.Add(id);
		return false;
	}


	// 判定オブジェクトを配置する
	public void StartAttack(int attackNum)
	{
		hitObjects[attackNum-1].SetActive(true);
	}

	// 攻撃終了
	public void ExitAttack(int attackNum)
	{
		Debug.Log(attackNum);
		hitObjectIds.Clear();
		hitObjects[attackNum-1].SetActive(false);
	}

	// 引数のオブジェクトがactiveかどうか
	public bool IsActiveAttackObject(int attackNum) { return hitObjects[attackNum - 1].activeSelf; }

}
