// 作ったはいいけどキューブ以外に適用してもかなりしょぼかった
Shader "Custom/WireFrame" {
	Properties {
		_LineColor ("LineColor", Color) = (1,1,1,1)
		_GridColor ("GridColor", Color) = (1,1,1,1)
		_LineWidth ("LineWidth", float) = 0.01
	}
	SubShader {
		Pass{
			Cull off								// カリングせずに両面描画
			Tags{ "RenderType" = "Transparent" }	// 透過使用
			Blend SrcAlpha OneMinusSrcAlpha			// αブレンディング
			AlphaTest Greater 0.5					// 出力値が指定値より大きかったら描画

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			uniform float4 _LineColor;
			uniform float4 _GridColor;
			uniform float _LineWidth;

			// 頂点入力(位置, テクスチャ情報, 色)
			struct vertData{
				float4 vertex : POSITION;
				float4 texcoord0 : TEXCOORD0;
				float4 color : COLOR;
			};

			// 出力データ
			struct fragData{
				float4 pos : POSITION;
				float4 texcoord0 : TEXCOORD0;
				float4 color : COLOR;
			};

			// 頂点処理
			fragData vert(vertData v){
				fragData o;	// 返り値
				o.pos		= mul(UNITY_MATRIX_MVP, v.vertex);	// モデルビュープロジェクション変換
				o.texcoord0 = v.texcoord0;
				o.color		= v.color;
				return o;
			}

			// フラグメント処理
			fixed4 frag(fragData i) : COLOR
			{
				fixed4 answer;

				// step･･･第二引数が第一引数以上なら1, 小さいなら0を返す
				float lx = step(_LineWidth, i.texcoord0.x);
				float ly = step(_LineWidth, i.texcoord0.y);
				float hx = step(i.texcoord0.x, 1 - _LineWidth);
				float hy = step(i.texcoord0.y, 1 - _LineWidth);

				/*
				float3 lerp(float3 a, float3 b, float w)
				{
					return a + w*(b-a);
				}
				つまり第3引数が0なら第一引数の色, 1なら元の色に第二引数の色が加算される
				*/
				answer = lerp(_LineColor, _GridColor, lx*ly*hx*hy);

				return answer;
			}
			ENDCG
		}
	}
	Fallback off
}