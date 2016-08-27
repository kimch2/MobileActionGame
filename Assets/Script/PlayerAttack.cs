using UnityEngine;
using System.Collections;

// 通常攻撃
public class PlayerAttack : MonoBehaviour {

	[SerializeField] private Animator			m_Animator;
	[SerializeField] private GameObject			m_Hand;
	[SerializeField] private ChildObject		m_SwordObject;
	[SerializeField] private ContinuousAttack	m_ContinuousAttack;
	[SerializeField] private AudioClip			m_SlashSE;

	private GameObject		m_Sword;
	private const int		MAX_ATTACK_NUM = 3;
	private const string	ATTACK_STATE_NAME = "NormalAttack";

	private bool IsPreTagToAttack = false;

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

		if (m_Animator.GetInteger(ATTACK_STATE_NAME) == 0) return;

		bool IsTagToAttack = m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

		// 攻撃以外なら値を0に戻す
		if (IsPreTagToAttack && !IsTagToAttack)
		{
			m_ContinuousAttack.ExitAttack(m_Animator.GetInteger(ATTACK_STATE_NAME));
			m_Animator.SetInteger(ATTACK_STATE_NAME, 0);
        }

		// 次の更新時に現在のタグ情報が必要なので確保しておく
		IsPreTagToAttack = IsTagToAttack;
    }

	public void OnClickAttackButton()
	{
		int	nowAttackNum = m_Animator.GetInteger(ATTACK_STATE_NAME);

		// 最大攻撃回数までいっていたら何もしない
		if (nowAttackNum == MAX_ATTACK_NUM) return;

		// 仮
		if(nowAttackNum >= 1) m_ContinuousAttack.ExitAttack(nowAttackNum);

		// 連撃回数加算
		++nowAttackNum;
		m_Animator.SetInteger(ATTACK_STATE_NAME, nowAttackNum);
		m_ContinuousAttack.StartAttack(nowAttackNum);
		SEManager.Instance.PlayShot(m_SlashSE);
		
		// 初回の攻撃ならPlayを実行
		if (nowAttackNum != 1) return;
		m_Animator.Play("slash1", 0, 0.0f);
	}
}