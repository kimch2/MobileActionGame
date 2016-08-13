using UnityEngine;
using System.Collections;

public class EquipmentObject : ScriptableObject {
	public GameObject	model;
	public Vector3		localPosition	= Vector3.zero;
	public Vector3		localRotation	= Vector3.zero;
	public Vector3		scale			= Vector3.one;
}
