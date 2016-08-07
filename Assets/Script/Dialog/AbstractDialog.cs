using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

// ダイアログの基底クラス
// ダイアログは専用のCanvasに表示させる
public abstract class AbstractDialog : MonoBehaviour {

	protected Action	m_Callback;
	protected Image		m_Image;

	public bool IsShow() { return gameObject.activeSelf; }

	public virtual void Show(Action callback)
	{
		// ここら辺にダイアログ表示時のアニメーションを開始する
		// コルーチンを入れる


		callback = m_Callback;
	}

	public virtual void Close()
	{
		gameObject.SetActive(false);
	}
}
