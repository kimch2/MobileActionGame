using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

// SceneManagerのラッピングクラス
public class MySceneManager : SingletonBehaviourScript<MySceneManager>
{
	private Action m_Callback;
	private string m_NextSceneName;

	protected override void Awake()
	{
		base.Awake();
		//DontDestroyOnLoad(this.gameObject);
		Application.targetFrameRate = 60;
	}

	// 通常のScene遷移
	public void LoadScene(string name)
	{
		SceneManager.LoadScene(name);
	}

	// 非同期のScene遷移
	// コールバックは非同期読み込み終了時に行う時に呼び出される
	public void LoadSceneAsync(string name, Action callback)
	{
		m_NextSceneName = name;
		m_Callback		= callback;
		StartCoroutine(CalcLoadSceneAsync());
	}

	// 現在アクティブなScene名を取得
	public string GetActiveSceneName()
	{
		return SceneManager.GetActiveScene().name;
	}

	// 非同期読み込み処理
	private IEnumerator CalcLoadSceneAsync()
	{
		yield return SceneManager.LoadSceneAsync(m_NextSceneName);

		// ここまで来たら読み込み完了
		Debug.Log(GetActiveSceneName());
		m_Callback();
	}

}
