using UnityEngine;
using System.Collections;
using System;

namespace UnityStandardAssets.ImageEffects
{
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/TimeStop")]
	public class TimeStopImageEffect : ImageEffectBase
	{
		private enum State{ Wait, ScaleUp, ScaleDown }

		[SerializeField, Range(0.0f, 1.0f)] private float m_Radius = 0.0f;

		private const float ADD_RADIUS_SCALE = 0.05f;
		private State	m_State		= State.Wait;
		private Action	m_Callback	= null;
		
		// 時止め開始
		public void StartTimeStop(Action callback = null)
		{
			if (IsTimeStopping()) return;

			m_Radius	= 0.0f;
			m_State		= State.ScaleUp;
			m_Callback	= callback;
		}
		// 時止め終了
		public void EndTimeStop(Action callback = null)
		{
			if (!IsTimeStopping()) return;
			
			m_State		= State.ScaleDown;
			m_Callback	= callback;
		}

		// 時止め状態か
		public bool IsTimeStopping() { return m_State == State.Wait && m_Radius == 1.0f; }

		// 更新処理
		void Update()
		{
			if (m_State == State.Wait) return;

			// スケーリング
			if(m_State == State.ScaleUp)
			{
				m_Radius += ADD_RADIUS_SCALE;
				if (m_Radius < 1.0f) return;

				m_Radius = 1.0f;
            }
			else
			{
				m_Radius -= ADD_RADIUS_SCALE;
				if (m_Radius > 0.0f) return;

				m_Radius = 0.0f;
			}

			// 待機状態に変更
			m_State = State.Wait;

			// コールバックが登録されていたら実行
			if (m_Callback == null) return;
			m_Callback();
		}

		// カメラが映している映像をテクスチャとしてシェーダーに渡す
		void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			material.SetFloat("_Radius", m_Radius);
			Graphics.Blit(source, destination, material);
		}
	}
}
