using UnityEngine;
using System.Collections;

// 通常攻撃
public class PlayerAttack : MonoBehaviour {

	// 攻撃～攻撃までの遷移に必要となるパラメーター一覧
	// 通常攻撃の回数によって増減する
	[System.Serializable]
	private class AttackMargin
	{
		public float	activeObjectStartTime;	// 判定開始時間
		public float	activeObjectEndTime;    // 判定終了時間
		public float	moveNextAttackTime;
	}

	[SerializeField] private Animator			m_Animator;
	[SerializeField] private GameObject			m_Hand;
	[SerializeField] private ChildObject		m_SwordObject;
	[SerializeField] private ContinuousAttack	m_ContinuousAttack;
	[SerializeField] private AudioClip			m_SlashSE;
	[SerializeField] private AttackMargin[]		m_AttackMargin = new AttackMargin[MAX_ATTACK_NUM];
	[SerializeField] private bool				m_IsNextMoveToAttack;       // 次攻撃に移行できるか

	private GameObject		m_Sword;
	private const int		MAX_ATTACK_NUM = 3;
	private const string	ATTACK_STATE_NAME = "NormalAttack";

	private bool m_ExitNowAttack	= false;	// 現在の攻撃が終了したか
	private bool IsPreTagToAttack	= false;

	// Use this for initialization
	void Start () {
		// 剣を手に持たせる
		m_Sword = Instantiate<GameObject>(m_SwordObject.model);
		m_Sword.transform.SetParent(m_Hand.transform);

		// 予め設定した値を代入
		m_Sword.transform.localPosition		= m_SwordObject.localPosition;
		m_Sword.transform.localRotation		= Quaternion.Euler(m_SwordObject.localRotation.x, m_SwordObject.localRotation.y, m_SwordObject.localRotation.z);
		m_Sword.transform.localScale		= m_SwordObject.scale;
	}
	
	// Update is called once per frame
	void Update () {

		int nowAttackNum = m_Animator.GetInteger(ATTACK_STATE_NAME);
        if (nowAttackNum == 0) return;

		bool isTagToAttack = m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

		// 攻撃以外なら値を0に戻す
		if (IsPreTagToAttack && !isTagToAttack)
		{
			m_ExitNowAttack			= false;
			m_IsNextMoveToAttack	= false;
			m_ContinuousAttack.ExitAttack(nowAttackNum);
			m_Animator.SetInteger(ATTACK_STATE_NAME, 0);
        }
		else
		{
			// 攻撃オブジェクトの更新
			UpdateAttackObject(nowAttackNum);

			// 次の攻撃へ移行できるかチェック
			if (IsMoveToNextAttack(nowAttackNum))
			{
				PlayAttack(nowAttackNum);
			}
		}

		// 次の更新時に現在のタグ情報が必要なので確保しておく
		IsPreTagToAttack = isTagToAttack;
    }

	// 判定オブジェクトの更新
	void UpdateAttackObject(int nowAttackNum)
	{
		AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
		float time = stateInfo.normalizedTime - Mathf.Floor(stateInfo.normalizedTime);

		// 開始
		if (!m_ContinuousAttack.IsActiveAttackObject(nowAttackNum) && !m_ExitNowAttack)
		{
			// normalizeTimeが0～指定時間の間なら開始する
			// 指定時間が0なら即発動
			if(m_AttackMargin[nowAttackNum-1].activeObjectStartTime == 0.0f || (time >= 0.0f && 
				time <= m_AttackMargin[nowAttackNum - 1].activeObjectStartTime))
			{
				Debug.LogFormat("Start obj, normalizetime : {0}", time);
				m_ContinuousAttack.StartAttack(nowAttackNum);
				SEManager.Instance.PlayShot(m_SlashSE);
			}
		}
		// 終了
		else if (m_ContinuousAttack.IsActiveAttackObject(nowAttackNum) &&
			 time >= m_AttackMargin[nowAttackNum - 1].activeObjectEndTime)
		{
			Debug.LogFormat("Delete obj, normalizetime : {0}", time);
			m_ContinuousAttack.ExitAttack(nowAttackNum);
			m_ExitNowAttack = true;
		}
	}

	// 次の攻撃に移行することができるか
	bool IsMoveToNextAttack(int nowAttackNum)
	{
		if (!m_IsNextMoveToAttack) return false;

		AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
		float time = stateInfo.normalizedTime - Mathf.Floor(stateInfo.normalizedTime);

		return m_ExitNowAttack && time >= m_AttackMargin[nowAttackNum-1].moveNextAttackTime;
	}

	// 攻撃開始
	void PlayAttack(int nowAttackNum)
	{
		// 前回の攻撃オブジェクトを消しておく
		if (nowAttackNum >= 1)
		{
			m_IsNextMoveToAttack	= false;
			m_ExitNowAttack			= false;
			m_ContinuousAttack.ExitAttack(nowAttackNum);
		}

		// 連撃回数加算
		++nowAttackNum;
		m_Animator.SetInteger(ATTACK_STATE_NAME, nowAttackNum);
	}

	public void OnClickAttackButton()
	{
		int	nowAttackNum = m_Animator.GetInteger(ATTACK_STATE_NAME);
		
		// 最大攻撃回数までいっていたら何もしない
		if (nowAttackNum == MAX_ATTACK_NUM) return;

		// 初回の攻撃ならPlayを実行
		if (nowAttackNum == 0)
		{
			PlayAttack(nowAttackNum);
			m_Animator.Play("slash1", 0, 0.0f);
			return;
		}

		// 次の攻撃へのフラグを立てる
		if (m_IsNextMoveToAttack) return;

		// 現在のモーションとモーションタイムをログに流す
		AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
		Debug.LogFormat("motionName : {0}, time : {1}", stateInfo.shortNameHash, stateInfo.normalizedTime);

		// オブジェクトの判定時間内か
		if (stateInfo.normalizedTime <= m_AttackMargin[nowAttackNum - 1].activeObjectEndTime)
		{
			m_IsNextMoveToAttack = true;
		}
	}
}