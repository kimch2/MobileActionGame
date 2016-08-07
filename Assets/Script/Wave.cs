using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {

	[SerializeField]
	private Shader shader;
	[SerializeField]
	private Color color = new Color(0.0f, 0.5f, 0.5f);
	[SerializeField]
	private Vector3 pos;
	[SerializeField]
	private Vector3 vec = new Vector3(0.0f, 0.0f, 1.0f);
	[SerializeField, Tooltip("色の幅")]
	private float amplitude = 1.0f;
	[SerializeField, Tooltip("波の幅")]
	private float exponent = 20.0f;
	[SerializeField]
	private float interval = 20.0f;
	[SerializeField]
	private float speed = 10.0f;
	

	// シェーダーに値を渡すために必要なID群
	struct ShaderID
	{
		public int color;
		public int pos;
		public int vec;
		public int param;

	};
	private ShaderID sID;

	private Material mat;

	void Awake()
	{
		sID.color	= Shader.PropertyToID("_Color");
		sID.pos		= Shader.PropertyToID("_Position");
		sID.vec		= Shader.PropertyToID("_Vector");
		sID.param	= Shader.PropertyToID("_Params");
	}

	void OnEnable()
	{
		mat = new Material(shader);

		// オブジェクトが
		//「ヒエラルキービューに表示されない」
		//「シーンに保存されない」
		//「Resources.UnloadUnusedAssets でアンロードされない」の組み合わせ
		mat.hideFlags = HideFlags.HideAndDontSave;

		// カメラに映る全体を単一シェーダで描画したい場合に呼び出す
		GetComponent<Camera>().SetReplacementShader(shader, null);
		Update();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Shader.SetGlobalColor(sID.color, color);
		Shader.SetGlobalVector(sID.pos, pos);
		Shader.SetGlobalVector(sID.vec, vec);
		
		Vector4 param = new Vector4(amplitude, exponent, interval, speed);
		Shader.SetGlobalColor(sID.param, param);
	}

	void OnDisable()
	{
		mat = null;
		GetComponent<Camera>().ResetReplacementShader();
	}

}
