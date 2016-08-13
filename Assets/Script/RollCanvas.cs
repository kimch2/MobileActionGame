using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// 項目を配置するキャンバスのスクリプト
public class RollCanvas : MonoBehaviour {

	enum CanvasMode { Wait, UpRolling = 1, DownRolling = -1 };

	[SerializeField]
	private Image		m_ItemImage;
	[SerializeField]
	private Text[]		m_GuideTexts;
	[SerializeField]
	private AudioClip	m_UpDownSE;
	[SerializeField]
	private int			m_NowItemNum = 1;

	[SerializeField]
	private float m_Radius = 270.0f;
	[SerializeField]
	private Image m_Elements;
	[SerializeField]
	private float m_RollPower = 20.0f;	
	[SerializeField]
	private CanvasMode mode = CanvasMode.Wait;
	[SerializeField, Tooltip("項目ごとに飛ぶシーン名, 項目の数だけ設定する")]
	private string[] m_ChangeSceneNames;

	private RectTransform[] items;

	private float oneRotate		= 0.0f; 
	private float nowRotate		= 0.0f;	// 回転時の回転量を格納する
	private float stopAngle		= 0.0f;	// ボタンが押された際回転が止まる時の値
	private float angle			= 0.0f;
	
	// 回転を始める前の設定
	void setPreRoll(CanvasMode arg_mode)
	{
		mode = arg_mode;
		SE2DManager.Instance.PlayShot(m_UpDownSE);
		stopAngle = 360.0f * ((float)(items.Length - m_NowItemNum) / (float)(items.Length - 1));
		setImageAndGuideText();
	}

	void setImageAndGuideText()
	{
		// 画像と説明文の設定
		m_ItemImage.sprite = items[m_NowItemNum].GetComponent<TitleItem>().SpriteImage;
		for (int i = 0; i < m_GuideTexts.Length; i++)
		{
			m_GuideTexts[i].text = items[m_NowItemNum].GetComponent<TitleItem>().Memo;
		}
	}

	// ボタンが押された時の回転処理
	void calcRoll()
	{
		angle += (float)mode * m_RollPower;
		nowRotate += m_RollPower;
        if(nowRotate >= oneRotate)
		{
			angle = stopAngle;
			nowRotate = 0.0f;
			mode = CanvasMode.Wait;
		}

		m_Elements.GetComponent<RectTransform>().localRotation = Quaternion.AngleAxis(angle, new Vector3(0.0f, 0.0f, 1.0f));
	}

	void Awake()
	{
		// elementsの子の数に応じて配置を決める(0番目に自分も入っているので注意)
		items = m_Elements.GetComponentsInChildren<RectTransform>();
		// 1回の回転量をここで格納
		oneRotate = 360.0f / (float)(items.Length - 1);

		for (int i = 1; i < items.Length; ++i)
		{
			// 座標位置
			float theta = (Mathf.PI*2 / (items.Length - 1)) * (i - 1) + Mathf.PI;	// 180度の地点が現在選ばれている項目となるので最後にPIを足す
			Vector3 pos = new Vector3(Mathf.Cos(theta) * m_Radius, Mathf.Sin(theta) * m_Radius, 0.0f);
			items[i].localPosition = pos;
			// 回転量
			float rotateZ = 360.0f * ((float)(i-1) / (float)(items.Length - 1));
			items[i].localRotation = Quaternion.Euler(0.0f, 0.0f, rotateZ);
		}
		// 現在の項目にあるテキストの設定
		setImageAndGuideText();
	}
	
	// Update is called once per frame
	void Update () {
		if (mode == CanvasMode.Wait) return;
		calcRoll();
	}

	// 回転ボタンが押された時の挙動
	public void PushUpButton()
	{
		if (mode != CanvasMode.Wait) return;
		m_NowItemNum = m_NowItemNum == 1 ? items.Length - 1 : m_NowItemNum - 1;
		setPreRoll(CanvasMode.UpRolling);
	}
	public void PushDownButton()
	{
		if (mode != CanvasMode.Wait) return;
		m_NowItemNum = m_NowItemNum == items.Length - 1 ? 1 : m_NowItemNum + 1;
		setPreRoll(CanvasMode.DownRolling);
	}
	// 決定ボタン
	public void PushDecideButton()
	{
		// Endの場合アプリケーション終了
		if(m_ChangeSceneNames[m_NowItemNum-1] == "End")
		{
			Application.Quit();
			return;
		}
		
		// それ以外はInspectorで設定したシーンに飛ばす
		FadeManager.Instance.FadeIn(0.5f, delegate {
#if UNITY_EDITOR
			MySceneManager.Instance.LoadSceneAsync(m_ChangeSceneNames[m_NowItemNum - 1], delegate {
				// Editorでテスト中にTitleからGameへ遷移した際に
				// デバッグ用のManager群をオフにしておく
				GameObject obj = GameObject.Find("DebugManagerObjects");
				if (obj != null ||  obj.activeSelf) obj.SetActive(false);
			});
#else
			MySceneManager.Instance.LoadScene(m_ChangeSceneNames[m_NowItemNum - 1]);
#endif
		});
	}
}
