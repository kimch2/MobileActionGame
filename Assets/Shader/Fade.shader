Shader "Hidden/Fade"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			uniform float _Timer;
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);				// レンダリング画像の色取得(デフォルトのテクスチャは白なのでそのまま画素が返ってくる)
				
				// Time.y…純粋な経過時間
				// その3成分の半分の値を適用して、透明度は1とする
				// それを1から引き徐々に暗くする
				fixed4 black = 1 - fixed4(_Time.yyy / 2, 1);
				
				// 計算結果をテクスチャの色に掛ける
				col *= black;

				// 色反転
				// col = 1 - col;
				return col;
			}
			ENDCG
		}
	}
}
