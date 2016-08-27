using UnityEngine;
using System.Collections;

public class Yeti : AbstractObject {

	[SerializeField]
	private Animator m_Animator;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected override void Dead()
	{
		base.Dead();
		m_Animator.CrossFade("Dead", 0.3f);
	}

}
