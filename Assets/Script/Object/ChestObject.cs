using UnityEngine;
using System.Collections;

public class ChestObject : AbstractObject {


	protected override void Dead()
	{
		base.Dead();
		Debug.Log("Chest Dead");
	}
}
