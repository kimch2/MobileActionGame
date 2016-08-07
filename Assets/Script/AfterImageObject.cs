using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// 残像生成時に渡すパラメーター
public class AfterImageCreateParam
{
	public GameObject	afterImageObj;
	public Transform	transform;
	public Material		mat;
	public float		decreaseAlpha;
}

// 残像用スクリプト
// アルファが0になったらDestroy
public class AfterImageObject : MonoBehaviour {

	private Material	m_Material;
	private float		m_DecreaseAlpha;

	private AfterImageMaterialsData m_Materialdatas;
	private Animator	m_Animator;
	private Action		m_ExitCallback;

	// Instantiate時に共通の設定を行う処理はここに書く
	private static void SetCommonParamToInstantiate(AfterImageObject afterObj, AfterImageCreateParam param, Action callback)
	{
		// transform設定
		afterObj.transform.position = param.transform.position;
		afterObj.transform.rotation = param.transform.rotation;
		afterObj.transform.localScale = param.transform.localScale;

		// Material
		Color c = param.mat.color;
		c.a = 0.0f;
		param.mat.color = c;

		// 透過減少値設定
		afterObj.m_DecreaseAlpha = param.decreaseAlpha;

		// コールバック設定
		afterObj.m_ExitCallback = callback;

		// もしSpring処理が適用されているならオフにしておく
		AfterImageSpringManager springManager = afterObj.GetComponent<AfterImageSpringManager>();
		if (springManager.springManagerAttachedObject == null) return;

		Destroy(springManager.springManagerAttachedObject.GetComponent<UnityChan.SpringManager>());
	}

	public static AfterImageObject InstantiateToAttachedAnimator(AfterImageCreateParam param, Action callback)
	{
		
		GameObject			obj = Instantiate<GameObject>(param.afterImageObj);
		AfterImageObject	afterObj = obj.AddComponent<AfterImageObject>();

		// 共通設定の処理
		SetCommonParamToInstantiate(afterObj, param, callback);

		// マテリアルの適用
		afterObj.m_Material = param.mat;
		afterObj.m_Materialdatas = afterObj.GetComponent<AfterImageMaterialsData>();
		afterObj.m_Materialdatas.SetMaterial(param.mat);
		afterObj.m_Materialdatas.ShadowOff();

		// Animator設定
		afterObj.m_Animator = afterObj.GetComponent<Animator>();
		afterObj.m_Animator.applyRootMotion = false;
		afterObj.m_Animator.speed = 0;

		return afterObj;
	}

	public static AfterImageObject InstantiateToNormal(AfterImageCreateParam param, Action callback)
	{
		GameObject obj = Instantiate<GameObject>(param.afterImageObj);
		AfterImageObject afterObj = obj.AddComponent<AfterImageObject>();

		// 共通設定の処理
		SetCommonParamToInstantiate(afterObj, param, callback);

		// マテリアルの適用
		afterObj.GetComponent<Renderer>().material = param.mat;
		afterObj.m_Material = param.mat;

		return afterObj;
	}

	// 透過の準備
	public void StartTransmissive (float initAlpha, Transform parentTransform)
	{
		// transformを親モデルの位置に合わせる
		transform.position = parentTransform.position;
		transform.rotation = parentTransform.rotation;
		transform.localScale = parentTransform.localScale;

		// 透明度初期化
		Color c = m_Material.color;
		c.a = initAlpha;
		m_Material.color = c;

		StartCoroutine(transmissiveModel());
	}

	public void StartTransmissive(float initAlpha, Transform parentTransform, int animationShortHash, float normalizedTime)
	{
		// transformを親モデルの位置に合わせる
		transform.position = parentTransform.position;
		transform.rotation = parentTransform.rotation;
		transform.localScale = parentTransform.localScale;

		// 透明度初期化
		Color c = m_Material.color;
		c.a = initAlpha;
		m_Material.color = c;

		// 直前のポーズを取る
		m_Animator.Play(animationShortHash, 0, normalizedTime);

		StartCoroutine(transmissiveModel());
	}

	// 完全に透過しきったか
	bool isAlphaZero() { return m_Material.color.a <= 0.0f ? true : false; }

	// 透過処理
	IEnumerator transmissiveModel()
	{
		while (!isAlphaZero())
		{
			// 念のため0.0きっかりに揃える
			float a = m_Material.color.a - m_DecreaseAlpha * Time.deltaTime <= 0.0f ? 0.0f : m_Material.color.a - m_DecreaseAlpha * Time.deltaTime;
			Color c = m_Material.color;
			c.a = a;
			m_Material.color = c;
			m_Materialdatas.SetColor(c);
			yield return null;
		}
		m_ExitCallback();
	}
	
}
