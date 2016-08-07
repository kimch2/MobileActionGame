using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

// 電光掲示板のような真横にスクロールするタイプ
public class UguiTextScroll : MonoBehaviour {
	[SerializeField, Range(-1, 1), Tooltip("進行方向")]
	private int scrollDirect = -1;
	[SerializeField]
	private float scrollPower = 1.0f;
	[SerializeField]
	private RectTransform maskRect;
	[SerializeField]
	private RectTransform myRect;
	
	private Vector3 basePos;
	private Vector3 pos;

	void Start()
	{
		// ループ座標設定
		basePos = new Vector3(maskRect.rect.width, 0.0f, 0.0f);
		pos = myRect.localPosition;
	}

	void Update()
	{
		pos.x += scrollPower * scrollDirect;
		if (scrollDirect == -1 && pos.x <= -basePos.x)
		{
			pos.x = basePos.x;
		}
		else if (scrollDirect == 1 && pos.x >= basePos.x)
		{
			pos.x = -basePos.x;
		}

		myRect.localPosition = pos;
	}

}
