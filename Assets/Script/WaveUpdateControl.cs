using UnityEngine;
using System.Collections;

public class WaveUpdateControl : MonoBehaviour {

	[SerializeField]
	private Material myMat;
	[SerializeField]
	private Renderer targetRenderer;
	[SerializeField]
	private Texture inputTex;

	private RenderTexture[] waveBuf = new RenderTexture[3];
	private int currentTargetID = 0;

	// Use this for initialization
	void Start () {
		myMat = new Material(myMat);
		for (int i = 0; i < 3; ++i)
		{
			waveBuf[i] = new RenderTexture(1024, 1024, 24);
			waveBuf[i].wrapMode = TextureWrapMode.Clamp;	// ループしない
			waveBuf[i].Create();
		}
		myMat.SetTexture("_draw", inputTex);
	}
	
	// Update is called once per frame
	void Update () {
		int prevID1 = (currentTargetID - 1 + 3) % 3;
		int prevID2 = (currentTargetID - 2 + 3) % 3;
		myMat.SetTexture("_prev1", waveBuf[prevID1]);
		myMat.SetTexture("_prev2", waveBuf[prevID2]);

		Graphics.Blit(waveBuf[prevID1], waveBuf[prevID2], myMat);
		targetRenderer.material.mainTexture = waveBuf[currentTargetID];
		currentTargetID = (currentTargetID + 1) % 3;
	}
}
