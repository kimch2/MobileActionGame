using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : AbstractSoundManager<BGMManager>
{
	// クロスフェード用
	private AudioSource m_SubAudioSource;

	[SerializeField, Range(0.0f, 1.0f), Tooltip("BGM切り替え時のクロスフェードを行う時の割合\n0だとフェードアウト後に再生, 1だとフェードアウト開始と同時に再生")]
	private float m_CrossFadeRatio = 1.0f;

	public float fadeTime = 1.0f;

	[SerializeField]
	private BGMListObject m_BGMListObject;

	protected override void Awake()
	{
		base.Awake();
		m_SubAudioSource = gameObject.AddComponent<AudioSource>();
		m_SubAudioSource.playOnAwake = false;

		DontDestroyOnLoad(this.gameObject);
	}

	// BGMリストの設定
	public void SetBGMListObject(BGMListObject bgmListObject)
	{
		m_BGMListObject = bgmListObject;
	}

	// BGM再生
	public void Play(int bgmNo, bool isFade)
	{
		if (isFade)
		{
			// コルーチン
		}

		// 再生中なら一旦ストップ掛けてから再開
		if (m_AudioSource.isPlaying || m_SubAudioSource.isPlaying)
		{
			Stop(false);
		}

		// フェードしない場合はボリュームを基準値に合わせ即再生
		m_AudioSource.volume	= m_BaseVolume;
		m_AudioSource.clip		= m_BGMListObject.clips[bgmNo];
		m_AudioSource.Play();
	}

	// BGMクロスフェード再生
	public void PlayToCrossFade(int bgmNo)
	{
		// メインAudioの情報をサブに渡す(現在の再生時間も)
		// メインの方に引数のbgmclipを入れる
	}

	// BGM停止
	public void Stop(bool isFade)
	{
		if (isFade)
		{
			// コルーチン
		}

		// フェードしない場合は即停止(サブも)
		m_AudioSource.Stop();
		m_SubAudioSource.Stop();
	}
}
