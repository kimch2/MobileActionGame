using UnityEngine;
using System;
using System.Collections;

// シングルトンオブジェクトの基底クラス
// シングルトンとして扱いたいオブジェクトにスクリプトを用意する時はこいつを継承したスクリプトを使用する
public abstract class SingletonBehaviourScript<T> : MonoBehaviour where	T : SingletonBehaviourScript<T>
{
	// シングルトンとして扱うオブジェクトのタグ
	protected static readonly string[] findTags =
	{
		"GameManager", "GameController"
	};

	protected static T instance;
	public static T Instance
	{
		get
		{
			if (instance != null) return instance;

			Type type = typeof(T);
			foreach (var tag in findTags)
			{
				GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
				for (int i = 0; i < objs.Length; i++)
				{
					instance = (T)objs[i].GetComponent(type);
					if (instance != null) return instance;
				}
			}
			Debug.LogWarning(string.Format("{0} is not found", type.Name));
			return null;
		}
	}

	virtual protected void Awake()
	{
		Debug.Log(gameObject.name);
		CheckInstance();
	}

	protected bool CheckInstance()
	{
		if (instance == null)
		{
			instance = (T)this;
			return true;
		}
		else if(Instance == this)
		{
			return true;
		}

		Destroy(this);
		return false;
	}
}
