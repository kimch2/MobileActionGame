using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AfterImageMaterialsData : MonoBehaviour
{
	[SerializeField] private SkinnedMeshRenderer m_SkineedMeshRenderer;

	// 影の設定をオフにする
	public void ShadowOff()
	{
		m_SkineedMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		m_SkineedMeshRenderer.receiveShadows = false;
    }

	public void SetMaterial(Material mat)
	{
		m_SkineedMeshRenderer.material = mat;
		for (int i = 0; i < m_SkineedMeshRenderer.materials.Length; i++)
		{
			m_SkineedMeshRenderer.materials[i] = mat;
		}
	}

	public void SetColor(Color c)
	{
		m_SkineedMeshRenderer.material.color = c;
		for (int i = 0; i < m_SkineedMeshRenderer.materials.Length; i++)
		{
			m_SkineedMeshRenderer.materials[i].color = c;
		}
	}

}
