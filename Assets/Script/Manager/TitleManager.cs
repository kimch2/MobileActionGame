using UnityEngine;
using System.Collections;

public class TitleManager : SingletonBehaviourScript<TitleManager> {

	void Start()
	{
		FadeManager.Instance.FadeOut(1.0f, delegate
		{
			BGMManager.Instance.Play(0, false);
		});
	}
}
