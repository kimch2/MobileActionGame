using UnityEngine;
using System.Collections;

// プレイヤー・敵・オブジェクトが共通でもつパラメーター
[System.Serializable]
public class ObjectCommonParameter{

	public int id = 0;  // シーンに配置されるときに設定される固有のID
	public int hitPoint;
	public int attack;
	public int defence;
}
