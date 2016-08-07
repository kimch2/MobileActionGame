using UnityEngine;
using System.Collections;

// 目標とするターゲットと指定した距離を保って追従するスクリプト
// 主に座標だけ親子関係を結びたい時に使用
public class FollowObject : MonoBehaviour {

	[SerializeField]
	private GameObject target;
	[SerializeField, Tooltip("ターゲットとの間隔")]
	private float m_Length = 5.0f;

	// Use this for initialization
	void Start () {
		updatePosition();
	}
	
	// オブジェクトの更新が終わった後に位置を更新するためLateUpdateを使用
	void LateUpdate () {
		updatePosition();
	}

	void updatePosition()
	{
		transform.position = target.transform.position - transform.forward * m_Length;
	}
}
