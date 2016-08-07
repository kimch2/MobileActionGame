using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class PlayerSkill : MonoBehaviour {

	[SerializeField] private TimeStopImageEffect m_TimeStop;

	public float m_StopTimeLimit = 3.0f;
	public float m_NowTime = 0.0f;

	void Update()
	{
		if (!m_TimeStop.IsTimeStopping()) return;

		m_NowTime -= Time.deltaTime;
		if (m_NowTime > 0.0f) return;

		m_NowTime = 0.0f;
		m_TimeStop.EndTimeStop();
	}

	public void OnButtonClickSkill()
	{
		Debug.Log("clickSkill");
		if (m_TimeStop.IsTimeStopping()) return;

		m_TimeStop.StartTimeStop();
		m_NowTime = m_StopTimeLimit;
	}
}
