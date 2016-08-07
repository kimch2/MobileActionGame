using UnityEngine;
using System.Collections;

// サウンド関連の基底クラス
// サウンドマネージャー共通の処理はここに記述
// サウンドクラスを定義する際にはこのクラスを継承させる
[RequireComponent(typeof(AudioSource))]
public abstract class AbstractSoundManager<T> :  SingletonBehaviourScript<T> where T : AbstractSoundManager<T>
{
	protected AudioSource m_AudioSource;

	[SerializeField, Range(0.0f, 1.0f), Tooltip("音量基準値")]
	protected float m_BaseVolume = 1.0f;
	public float BaseVolume
	{
		get { return m_BaseVolume; }
		set
		{
			m_BaseVolume			= Mathf.Clamp(value, 0.0f, 1.0f);
			m_AudioSource.volume	= m_BaseVolume;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		m_AudioSource			= GetComponent<AudioSource>();
		m_AudioSource.volume	= m_BaseVolume;
		if (m_AudioSource.playOnAwake) m_AudioSource.playOnAwake = false;
	}
}
