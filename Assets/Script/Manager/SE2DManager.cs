using UnityEngine;
using System.Collections;

public class SE2DManager : AbstractSoundManager<SE2DManager>
{
	protected override void Awake()
	{
		base.Awake();
		//DontDestroyOnLoad(this.gameObject);
	}

	public void PlayShot(AudioClip clip)
	{
		m_AudioSource.PlayOneShot(clip);
	}
}
