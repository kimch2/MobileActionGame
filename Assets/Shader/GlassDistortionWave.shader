Shader "Unlit/GlassDistortionWave"
{
	Properties{
		_Alpha("Alpha", range(0.0, 1.0)) = 0.5
		_InnerCircleRadius("_InnerCircleRadius", range(0.0, 0.5)) = 0.2
		_BumpAmt("Distortion", range(0,256)) = 10
		_MainTex("Tint Color (RGB)", 2D) = "white" {}
		_BumpMap("Normalmap", 2D) = "bump" {}
	}

		SubShader{

		// 描画のプライオリティを指定, SubShaderがどういう性質の描画をするかを特定のシェーダーに伝える不透明
		// 今回は透過処理をするので第一引数はTransparent, 第2引数はOpaque(不透明(ソリッド))
		// ※第2引数が何故Opaqueなのかよくわからん(消しても特に問題ないので消した)
		Tags{ "Queue" = "Transparent" }

		// 特別なパスタイプ
		// スクリーンサイズのスナップショットをテクスチャに展開する
		// またこの記述を行うときは「sampler2D _GrabTexture;」を必ずPass内で書くこと
		GrabPass{
			Name "BASE"
			Tags{ "LightMode" = "Always" }
		}

		//ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha

		// 上記のGrabPassで取得した画像をbumpMap, amtを使い撹乱させる
		Pass{

			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
			// フラグメント処理で出来るだけ精度を下げ実行時間を最小に抑えるためのオプション
	#pragma fragmentoption ARB_precision_hint_fastest

	#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord: TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				float4 uvgrab : TEXCOORD0;
				float2 uvbump : TEXCOORD1;
				float2 uvmain : TEXCOORD2;
			};

			float _BumpAmt;
			float4 _BumpMap_ST;
			float4 _MainTex_ST;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				// ライブラリによってはテクスチャのY軸方向が上下逆になるので
				// UNITY_UV_STARTS_AT_TOPの変数を監視してフリップ(数値を合わせる)する
		#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
		#else
				float scale = 1.0;
		#endif

				// SSのテクスチャ座標合わせ
				// 基本的にこの形がデフォルト
				o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
				o.uvgrab.zw = o.vertex.zw;

				// テクスチャUVにスケールとバイアスを付与する
				o.uvbump = TRANSFORM_TEX(v.texcoord, _BumpMap);
				o.uvmain = TRANSFORM_TEX(v.texcoord, _MainTex);

				// 円の刳り貫きのために座標を中心に合わせる
				o.uvmain -= fixed2(0.5, 0.5);

				return o;
			}

			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;
			sampler2D _BumpMap;
			sampler2D _MainTex;

			float _Alpha;
			float _InnerCircleRadius;

			half4 frag(v2f i) : COLOR
			{
				// 現在の座標の距離算出
				float distance = sqrt(pow(i.uvmain.x, 2) + pow(i.uvmain.y, 2));
			// 座標を中心から元に戻しておく
			i.uvmain += fixed2(0.5, 0.5);

			// まず円形に整えるために半径0.5以上の部分は完全透過する
			if (distance > 0.5) {
				return half4(1.0, 1.0, 1.0, 0.0);
			}

			// 中心部分を刳り貫くために0~_InnerCircleRadiusの範囲は完全透過
			if (distance >= 0.0 && distance < _InnerCircleRadius) {
				return half4(1.0, 1.0, 1.0, 0.0);
			}

			// 歪みの算出

			// バンプマップから法線情報を取得
			// zの値を外してxyのみ取得することで高速化を図る
			half2 bump = UnpackNormal(tex2D(_BumpMap, i.uvbump)).rg;

			// バンプマップ, テクセルサイズ, 歪みの大きさからオフセットを決める
			float2 offset = bump * _GrabTexture_TexelSize.xy * _BumpAmt;
			i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;

			// UNITY_PROJ_COORD…ワールド頂点座標を与えることによってカメラのプロジェクション方向から見たスクリーン座標のUV値に変換される
			half4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
			half4 tint = tex2D(_MainTex, i.uvmain);
			return half4(col * tint.rgb, _Alpha);
		}
		ENDCG
	}
	}
}