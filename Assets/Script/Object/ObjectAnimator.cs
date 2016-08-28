using UnityEngine;
using System.Collections;

// Animatorを使用するオブジェクトに適用させるラッピングスクリプト
public class ObjectAnimator : MonoBehaviour {

	[SerializeField] private Animator m_Animator;
	private AnimatorStateInfo	m_StateInfo;
	private AnimatorStateInfo	m_PreStateInfo;

	// Use this for initialization
	void Awake () {
		m_StateInfo		= m_Animator.GetCurrentAnimatorStateInfo(0);
		m_PreStateInfo	= m_StateInfo;
	}
	
	// Update is called once per frame
	void Update () {
		m_PreStateInfo	= m_StateInfo;
		m_StateInfo		= m_Animator.GetCurrentAnimatorStateInfo(0);
	}

	// Animatorの取得
	public Animator GetAnimator() { return m_Animator; }

	// アニメーション開始
	public void Play(string name, float startTime = 0.0f) { m_Animator.Play(name, 0, startTime); }
	
	// クロスフェード
	public void CrossFade(string name, float fadeTime) { m_Animator.CrossFade(name, fadeTime); }

	// パラメーター設定
	public void SetInteger(string name, int value) { m_Animator.SetInteger(name, value); }
	public void SetFloat(string name, float value) { m_Animator.SetFloat(name, value); }
	public void SetBool(string name, bool value) { m_Animator.SetBool(name, value); }
	public void SetTrigger(string name) { m_Animator.SetTrigger(name); }

	// パラメーター取得
	public int		GetInteger(string name) { return m_Animator.GetInteger(name); }
	public float	GetFloat(string name) { return m_Animator.GetFloat(name); }
	public bool		GetBool(string name) { return m_Animator.GetBool(name); }

	// 現在のアニメーションタグが引数のものか
	public bool IsEqualToCurrentAnimationTag(string name) { return m_StateInfo.IsTag(name); }

	// 一つ前のアニメーションタグが引数のものか
	public bool IsEqualToPreAnimationTag(string name) { return m_PreStateInfo.IsTag(name); }

	// 現在のアニメーションの正規化時間を返す
	public float GetCurrentAnimationNormalizedTime() { return m_StateInfo.normalizedTime - Mathf.Floor(m_StateInfo.normalizedTime); }

	// 一つ前のアニメーションの正規化時間を返す
	public float GetPreAnimationNormalizedTime() { return m_PreStateInfo.normalizedTime - Mathf.Floor(m_PreStateInfo.normalizedTime); }

	// 現在のハッシュを返す
	public int GetCurrentAnimationHash() { return m_StateInfo.fullPathHash; }

	// 一つ前のハッシュを返す
	public int GetPreAnimationHash() { return m_PreStateInfo.fullPathHash; }
}
