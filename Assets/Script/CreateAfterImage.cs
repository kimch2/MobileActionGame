using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// 残像生成処理
// キャラにモーションが割り当てられている場合と
// 純粋にモデルだけの場合で生成処理が変わる
public class CreateAfterImage : MonoBehaviour {

	[SerializeField] private float CREATE_INTERVAL_TIME = 0.3f;

	private const float INIT_AFTERIMAGE_ALPHA	= 0.7f;
	private const float DECREASE_ALPHA			= 1.0f;

	private enum ImageType { Motion, Model };

	
	[SerializeField, Tooltip("残像の元となる対象の種類")]
	private ImageType m_ImageType = ImageType.Model;

	[SerializeField, Tooltip("残像に使用するモデル")]
	private GameObject m_AfterImageObject;

	[SerializeField, Tooltip("Motion時のみ設定")]
	private Animator m_Animator;

	[SerializeField]
	private List<Material> m_AfterImageMaterials;

	[SerializeField, Tooltip("残像の元モデルに揺れ物処理が入っている場合は使用すること")]
	private GameObject m_SpringManagerAttachedObject;

	public bool IsCreating = false;
	
	private bool[] m_IsUseMaterial;
	private AfterImageObject[] m_AfterImageObjects = new AfterImageObject[3];

	private float nowTime = 0.0f;

	void Awake()
	{
		m_IsUseMaterial = new bool[m_AfterImageMaterials.Count];
		// 最初にあらかじめ残像を生成して非アクティブにしておく
		// 生成の際に座標を簡単にするため子にしておく
        for (int i = 0; i < 3; ++i)
		{
			m_AfterImageObjects[i] = m_ImageType == ImageType.Motion ?
				CreateAfterImageToAnimatorModel(i) : CreateAfterImageToNormalModel(i);
		}
	}

	// Update is called once per frame
	void Update () {
		if (!IsCreating) return;


		nowTime += Time.deltaTime;
		if (nowTime >= CREATE_INTERVAL_TIME)
		{
			nowTime = 0.0f;
			// リストの中使われていないものを使用する
			for (int i = 0; i < m_IsUseMaterial.Length; ++i)
			{
				if (m_IsUseMaterial[i]) continue;

				// 使用フラグをオンに
				m_IsUseMaterial[i] = true;

				if (m_ImageType == ImageType.Model)
				{
					m_AfterImageObjects[i].StartTransmissive(INIT_AFTERIMAGE_ALPHA, transform);
				}

				AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
				m_AfterImageObjects[i].StartTransmissive(INIT_AFTERIMAGE_ALPHA, transform, stateInfo.shortNameHash, stateInfo.normalizedTime);
				return;
			}
		}


	}

	// Animator有のオブジェクト
	AfterImageObject CreateAfterImageToAnimatorModel(int matNum)
	{
		AfterImageCreateParam param = new AfterImageCreateParam();
		param.afterImageObj = m_AfterImageObject;
		param.transform		= transform;
		param.mat			= m_AfterImageMaterials[matNum];
		param.decreaseAlpha = DECREASE_ALPHA;

		return AfterImageObject.InstantiateToAttachedAnimator(param, delegate { m_IsUseMaterial[matNum] = false; });
	}

	// Animator無しのオブジェクト
	AfterImageObject CreateAfterImageToNormalModel(int matNum)
	{
		AfterImageCreateParam param = new AfterImageCreateParam();
		param.afterImageObj = m_AfterImageObject;
		param.transform		= transform;
		param.mat			= m_AfterImageMaterials[matNum];
		param.decreaseAlpha = DECREASE_ALPHA;

		return AfterImageObject.InstantiateToNormal(param, delegate { m_IsUseMaterial[matNum] = false; });
	}
}
