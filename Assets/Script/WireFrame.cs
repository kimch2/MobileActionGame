using UnityEngine;
using System.Collections;

public class WireFrame : MonoBehaviour {

	[SerializeField, Tooltip("ワイヤーフレームの色を指定したものにするか?(falseの場合は元のマテリアルの色が適用される)")]
	private bool isUseMyColor = false;
	[SerializeField, Tooltip("ワイヤーフレームの色")]
	private Color myColor;

	// Use this for initialization
	void Start () {
		// メッシュ情報を引っ張ってきてラインで描画しなおす
		MeshFilter mf = GetComponent<MeshFilter>();
		mf.mesh.SetIndices(mf.mesh.GetIndices(0), MeshTopology.LineStrip, 0);

		// マテリアルカラーを変更してよりそれっぽく
		MeshRenderer mr = GetComponent<MeshRenderer>();
		mr.material.color = new Color(myColor.r, myColor.g, myColor.b, myColor.a);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
