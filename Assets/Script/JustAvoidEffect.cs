using UnityEngine;
using System.Collections;

// 完全回避成功時に発生するエフェクト
public class JustAvoidEffect : MonoBehaviour {

	[SerializeField] private GlassDistortionWave m_GlassDistortionWave;


	[SerializeField] AudioClip		m_PlaySE;
	[SerializeField] private float	m_AddPositionY = 0.8f;

	// エフェクトの固定位置
	private Vector3 m_WaitPosition;

	// 自分の持っているエフェクトをすべて開始させる
	public void StartEffect(Vector3 position)
	{
		position.y += m_AddPositionY;
		transform.position = position;

		m_GlassDistortionWave.StartWave(position);

		SEManager.Instance.PlayShot(m_PlaySE);
	}

	void Update()
	{
		if (!gameObject.activeSelf) return;

		if (!m_GlassDistortionWave.IsWaveing)
		{
			gameObject.SetActive(false);
		}
	}

}
