﻿using UnityEngine;
using System.Collections;

public class GameManager : SingletonBehaviourScript<GameManager> {

	protected override void Awake()
	{
		base.Awake();

		BGMManager.Instance.SetBGMListObject(Resources.Load("BGMData/Stage0") as BGMListObject);
		//BGMManager.Instance.Play(0, false);
		FadeManager.Instance.FadeOut(0.5f, delegate {
			BGMManager.Instance.Play(0, false);
			Debug.Log("ok");
		});
	}
}
