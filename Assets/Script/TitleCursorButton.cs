using UnityEngine;
using System.Collections;

public class TitleCursorButton : MonoBehaviour {
	enum Type { Up = -1, Down = 1 }

	[SerializeField]
	private Type type;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PushButton()
	{
		Debug.Log("OK");
	}

}
