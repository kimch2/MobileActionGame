using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	[SerializeField] private Animator m_Animator;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickAttackButton()
	{
		m_Animator.Play("slash0", 0, 0.0f);
	}
}
