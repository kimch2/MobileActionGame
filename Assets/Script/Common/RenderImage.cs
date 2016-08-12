using UnityEngine;
using System.Collections;

// 自作ポストエフェクトを適用させる際に使用
public class RenderImage : MonoBehaviour {

	[SerializeField]
	private Shader shader;
	[SerializeField]
	private Material mat;
	
	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		// レンダリング結果(src)にシェーダー処理を施してdstへ出力
		Graphics.Blit(src, dst, mat);
	}

	void Awake()
	{
		mat = new Material(shader);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
