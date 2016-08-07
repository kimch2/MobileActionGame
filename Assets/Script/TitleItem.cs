using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Titleシーンで選ぶ選択肢のスクリプト
// 基本的にuGUIのテキストを使用
[RequireComponent(typeof(Text))]
public class TitleItem : MonoBehaviour {

	[SerializeField, Tooltip("項目名")]
	private string title;
	public string Title { get { return title; } private set { title = value; } }
	[SerializeField, Tooltip("説明文")]
	private string memo;
	public string Memo { get { return memo; } private set { memo = value; } }
	[SerializeField, Tooltip("選択中に表示させる画像")]
	private Sprite spriteImage;
	public Sprite SpriteImage { get { return spriteImage; } private set { spriteImage = value; } }

	void Awake()
	{
		Text text = GetComponent<Text>();
		text.text = Title;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
