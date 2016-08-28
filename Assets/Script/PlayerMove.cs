using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System;

public class PlayerMove : MonoBehaviour {
	[SerializeField] private ObjectAnimator		m_ObjAnimator;
	[SerializeField] private Transform			m_ModelTransform;
	[SerializeField] private Rigidbody			m_Rigidbody;
	[SerializeField] private CreateAfterImage	m_CreateAfterImage;
	[SerializeField] private JustAvoidEffect	m_JustAvoidEffect;

	[SerializeField, Range(0.0f, 0.5f), Tooltip("移動速度")]
	private float speedPower = 0.01f;
	[SerializeField, Tooltip("現在の移動ベクトル")]
	private Vector3 spdVec;

	public bool IsDebugUseKeyMove = true;

	// ダンジョン時の移動はカメラから見て上下左右に移動させるため
	// あらかじめクォータービューの角度を設定しておいて移動方向も固定にしておく
	private readonly Vector3 ADVANCE_UP_VEC		= new Vector3(-1.0f, 0.0f, 1.0f);		// 上方向ベクトル
	private readonly Vector3 ADVANCE_SIDE_VEC	= new Vector3(1.0f, 0.0f, 1.0f);        // 横方向ベクトル

	// 特殊アクション中
	private const float AVAIDANCE_MOVE_SPEED = 0.1f;

	public bool		m_IsPlaySpecialAction	= false;
	private Action	m_SpecialActionFunc		= null;
	public int		m_NowPlayAnimationHash	= 0;
	public bool		m_IsChangeHash			= false;
	public bool		m_IsJudgmentInJustAvoid = false;	// 完全回避の判定時間中か

	// Use this for initialization
	void Start () {
		m_CreateAfterImage.IsCreating = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		// 特殊アクション中なら関数を実行してすぐ抜ける
		if (m_IsPlaySpecialAction)
		{
			// ハッシュの移行確認
			if(!m_IsChangeHash && m_NowPlayAnimationHash != m_ObjAnimator.GetCurrentAnimationHash())
			{
				m_IsChangeHash = true;
				m_NowPlayAnimationHash = m_ObjAnimator.GetCurrentAnimationHash();
            }

			m_SpecialActionFunc();
			return;
		}

		// 移動ベクトル算出
		spdVec = new Vector3(
			CrossPlatformInputManager.GetAxisRaw("Vertical") * ADVANCE_UP_VEC.x + CrossPlatformInputManager.GetAxisRaw("Horizontal") * ADVANCE_SIDE_VEC.x, 
			0.0f,
			CrossPlatformInputManager.GetAxisRaw("Vertical") * ADVANCE_UP_VEC.z + CrossPlatformInputManager.GetAxisRaw("Horizontal") * ADVANCE_SIDE_VEC.z
		) * speedPower;

#if UNITY_EDITOR
		if (IsDebugUseKeyMove)
		{
			spdVec = new Vector3(
				Input.GetAxisRaw("Vertical") * ADVANCE_UP_VEC.x + Input.GetAxisRaw("Horizontal") * ADVANCE_SIDE_VEC.x,
				0.0f,
				Input.GetAxisRaw("Vertical") * ADVANCE_UP_VEC.z + Input.GetAxisRaw("Horizontal") * ADVANCE_SIDE_VEC.z
			) * speedPower;
		}
#endif
		// 移動実行
		m_Rigidbody.MovePosition(spdVec + transform.position);

		// 移動ベクトルが発生していたら
		if (spdVec != Vector3.zero)
		{
			// 進行方向に体の向きを向かせる
            spdVec.Normalize();
			transform.forward = spdVec;
			m_ModelTransform.forward = spdVec;
			m_ModelTransform.position = transform.position;

			// てすと
			// 移動中に残像発生
			m_CreateAfterImage.IsCreating = true;
		}
		else
		{
			m_CreateAfterImage.IsCreating = false;
		}
		m_ObjAnimator.SetBool("IsMove", spdVec != Vector3.zero);
	}

	private void ActionAvoidance()
	{
		// 一定秒数内は完全回避判定をONにする
		CheckJustAvoidTime(m_ObjAnimator.GetCurrentAnimationNormalizedTime());

		float movePower = 5.0f * Time.deltaTime;

		m_Rigidbody.MovePosition(transform.forward * movePower + transform.position);
		m_ModelTransform.position = transform.position;

		// 終了判断
		CheckExitAction();
	}

	private void ActionRearRolling()
	{
		// 一定秒数内は完全回避判定をONにする
		CheckJustAvoidTime(m_ObjAnimator.GetCurrentAnimationNormalizedTime());

		float movePower = 5.0f * Time.deltaTime;

		m_Rigidbody.MovePosition(-transform.forward * movePower + transform.position);
		m_ModelTransform.position = transform.position;

		// 終了判断
		CheckExitAction();
	}

	// 完全回避判定のONOFFチェック
	private void CheckJustAvoidTime(float normalizedTime)
	{
		if (normalizedTime > 0.0 && normalizedTime < 0.3)
		{
			Time.timeScale = 0.2f;
			m_IsJudgmentInJustAvoid = true;
		}
		else
		{
			Time.timeScale = 1.0f;
			m_IsJudgmentInJustAvoid = false;
		}
	}

	// 終了処理のチェック
	private void CheckExitAction()
	{
		if (m_ObjAnimator.GetCurrentAnimationHash() == m_NowPlayAnimationHash) return;
		if (m_ObjAnimator.GetCurrentAnimationNormalizedTime() < 0.95f) return;
		//if (stateInfo.IsTag("Action")) return;

		m_IsPlaySpecialAction = false;
		m_IsChangeHash = false;
		m_NowPlayAnimationHash = 0;
		Debug.Log("Exit Action");
	}

	public void OnClickActionButton(string motionName)
	{
		if (m_IsPlaySpecialAction || m_ObjAnimator.IsEqualToCurrentAnimationTag("Action")) return;

		// モーション開始 ハッシュを格納しておく
		// また、この時点では前回のモーションのハッシュが残っているので再度ハッシュ確認を行う必要あり
		m_ObjAnimator.Play(motionName);
		m_NowPlayAnimationHash = m_ObjAnimator.GetCurrentAnimationHash();
		m_IsPlaySpecialAction	= true;

		// テスト
		m_JustAvoidEffect.gameObject.SetActive(true);
		m_JustAvoidEffect.StartEffect(transform.position);

		// 文字列によって行う処理を変える
		switch (motionName)
		{
			case "Avoidance":
				m_CreateAfterImage.IsCreating = true;
				m_SpecialActionFunc = ActionAvoidance;
				return;
			case "RearRolling":
				m_SpecialActionFunc = ActionRearRolling;
				return;
			default:
				Debug.LogError("＊＊＊モーション名が異常です＊＊＊");
				return;
		}
	}
}
