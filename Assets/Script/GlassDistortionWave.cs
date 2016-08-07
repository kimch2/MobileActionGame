using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GlassDistortionWave : MonoBehaviour {

	[System.Serializable]
	private class ShaderProperties
	{
		public Texture	bumpTex;
		[Range(0.0f, 1.0f)] public float	alpha				= 1.0f;
		[Range(0.0f, 0.5f)] public float	innerCircleRadius	= 0.2f;
		[Range(0, 256f)]	public int		bumpAmt				= 128;
	}

	[SerializeField]
	private Shader				m_Shader;
	[SerializeField]
	private ShaderProperties	m_ShaderProperties;
	[SerializeField]
	private float				m_ScaleUpPower = 0.01f;
	[SerializeField]
	private float				m_DecreaseAlpha = 0.01f;

	[SerializeField]
	private bool m_IsWaveing = false;
	public bool IsWaveing
	{
		get { return m_IsWaveing; }
		private set { m_IsWaveing = value; }
	}

	private MeshRenderer m_MeshRender;
	private Material m_Material;

	void Awake()
	{
		// マテリアルの設定
		m_Material = new Material(m_Shader);
		SetMaterialProperties();

		// 設定したマテリアルをレンダラーに登録
		m_MeshRender = GetComponent<MeshRenderer>();
		m_MeshRender.material = m_Material;
	}

	void Update()
	{
		// 拡大しつつ透過
		if (IsWaveing)
		{
			m_ShaderProperties.alpha -= Time.deltaTime * m_DecreaseAlpha;
			transform.localScale += Time.deltaTime * new Vector3(m_ScaleUpPower, m_ScaleUpPower, 0.0f);

			if(m_ShaderProperties.alpha <= 0.0f)
			{
				m_ShaderProperties.alpha = 0.0f;
				IsWaveing = false;
			}
		}

		// 毎回値を更新する
		SetMaterialProperties();
	}

	// マテリアルの値を設定
	void SetMaterialProperties()
	{
		m_Material.SetTexture("_BumpMap", m_ShaderProperties.bumpTex);
		m_Material.SetInt("_BumpAmt", m_ShaderProperties.bumpAmt);
		m_Material.SetFloat("_Alpha", m_ShaderProperties.alpha);
		m_Material.SetFloat("_InnerCircleRadius", m_ShaderProperties.innerCircleRadius);
	}

	public void StartWave(Vector3 position)
	{
		IsWaveing = true;
		m_ShaderProperties.alpha	= 1.0f;
		transform.localScale		= Vector3.one;
		transform.position			= position;
	}

}
