using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

// ダイアログの基底クラス
// ダイアログは専用のCanvasに表示させる
public abstract class AbstractDialog : MonoBehaviour {

	[SerializeField]
	protected GameObject m_Dialog;
	protected Action	m_Callback;
	
	public bool IsShow() { return gameObject.activeSelf; }

	public virtual void Show(Action callback)
	{
		// ここら辺にダイアログ表示時のアニメーションを開始する
		// コルーチンを入れる
		m_Dialog.gameObject.transform.localScale = Vector3.zero;
		iTween.ScaleTo(m_Dialog.gameObject, iTween.Hash(
			"scale", Vector3.one, 
			"time", 0.5f, 
			"easetype", iTween.EaseType.easeOutElastic
			));

		callback = m_Callback;
	}

	public virtual void Close()
	{
		gameObject.SetActive(false);
	}
}
