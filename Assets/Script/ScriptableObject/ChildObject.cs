using UnityEngine;
using System.Collections;

public class ChildObject : ScriptableObject {
	public GameObject	model;
	public Vector3		localPosition	= Vector3.zero;
	public Vector3		localRotation	= Vector3.zero;
	public Vector3		scale			= Vector3.one;
}
