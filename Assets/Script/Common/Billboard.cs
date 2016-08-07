using UnityEngine;
using System.Collections;

// Editor上でも実行するように
[ExecuteInEditMode]
public class Billboard : MonoBehaviour {

	// カメラに描画される範囲に居る場合のみ処理
	void OnWillRenderObject()
	{
		transform.rotation = Camera.current.transform.rotation;
	}
}
