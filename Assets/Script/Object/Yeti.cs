using UnityEngine;
using System.Collections;

public class Yeti : AbstractObject {

	[SerializeField] ObjectAnimator m_ObjAnimator;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected override void Dead()
	{
		base.Dead();
		m_ObjAnimator.CrossFade("Dead", 0.3f);
	}

}
