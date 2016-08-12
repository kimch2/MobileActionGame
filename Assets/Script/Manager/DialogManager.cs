using UnityEngine;
using System.Collections;

public class DialogManager : SingletonBehaviourScript<DialogManager> {

	[SerializeField]
	private NormalDialog m_NormalDialog;

	protected override void Awake()
	{
		base.Awake();

	}

	public void OnClickMenuButton()
	{
		m_NormalDialog.gameObject.SetActive(true);
		m_NormalDialog.Show(null);
	}

}
