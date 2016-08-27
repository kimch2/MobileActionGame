using UnityEngine;
using System.Collections;

public class ChestObject : AbstractObject {

	[SerializeField]
	private AudioClip m_DamageSE;
	[SerializeField]
	private AudioClip m_OpenSE;

	AudioSource m_AudioSource;

	void Awake()
	{
		m_AudioSource = GetComponent<AudioSource>();
	}

	protected override void Damage(int damage)
	{
		base.Damage(damage);
		
		// ここらへんで死亡かそうではないかで音の判定行う
		m_AudioSource.PlayOneShot(m_DamageSE);
	}

	protected override void Dead()
	{
		base.Dead();
		Debug.Log("Chest Dead");
	}
}
