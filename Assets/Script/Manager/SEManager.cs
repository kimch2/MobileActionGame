using UnityEngine;
using System.Collections;

public class SEManager : AbstractSoundManager<SEManager>
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
