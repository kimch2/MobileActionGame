using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

// フェードイン・アウトを管理するクラス
public class FadeManager : SingletonBehaviourScript<FadeManager> {

	[SerializeField] private Image m_Image;
	public bool IsFading
	{
		get { return m_Image.raycastTarget; } private set { m_Image.raycastTarget = value; }
	}

	private float	m_NowTime = 0.0f;
	private Action	m_Callback;

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(this.gameObject);

		Color color		= m_Image.color;
		color.a			= 1.0f;
		m_Image.color	= color;
	}

	public void FadeIn(float time = 0.5f, Action callback = null)
	{
		IsFading	= true;
		m_Callback	= callback;
		StartCoroutine(FadeInUpdate(time));
	}

	public void FadeOut(float time = 0.5f, Action callback = null)
	{
		IsFading = true;
		m_Callback = callback;
		StartCoroutine(FadeOutUpdate(time));
	}

	private void ExitFade()
	{
		m_NowTime	= 0.0f;
		IsFading	= false;
		if (m_Callback != null) m_Callback();
	}

	private IEnumerator FadeInUpdate(float time)
	{
		Color color = m_Image.color;

		// timeが0の時は一瞬で終了させる
		if(time <= 0.0f)
		{
			color.a = 1.0f;
			m_Image.color = color;

			ExitFade();
			yield break;
		}

		while (m_NowTime <= time)
		{
			color.a			= Mathf.Lerp(0.0f, 1.0f, m_NowTime / time);
			m_Image.color	= color;
			m_NowTime		+= Time.deltaTime;
			yield return 0;
		}

		ExitFade();
	}

	private IEnumerator FadeOutUpdate(float time)
	{
		Color color = m_Image.color;

		// timeが0の時は一瞬で終了させる
		if (time <= 0.0f)
		{
			color.a = 0.0f;
			m_Image.color = color;

			ExitFade();
			yield break;
		}

		while (m_NowTime <= time)
		{
			color.a = Mathf.Lerp(1.0f, 0.0f, m_NowTime / time);
			m_Image.color = color;
			m_NowTime += Time.deltaTime;
			yield return 0;
		}

		ExitFade();
	}

}
