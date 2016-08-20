using UnityEngine;
using System.Collections;

public class DialogManager : SingletonBehaviourScript<DialogManager> {

	[SerializeField]
	private NormalDialog m_NormalDialog;
	[SerializeField]
	private NormalDialog m_CheckDialog;

	protected override void Awake()
	{
		base.Awake();

	}

	public void OnClickMenuButton()
	{
		m_NormalDialog.gameObject.SetActive(true);
		m_NormalDialog.Show(null);
	}

	public void OnClickMenuBackTitleButton()
	{
		m_CheckDialog.gameObject.SetActive(true);
		m_CheckDialog.Show(null);
	}
}
