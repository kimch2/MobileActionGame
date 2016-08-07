Shader "Custom/WaveUpdate" 
{
	Properties 
	{
		_prev1	("Prev1", 2D) = "white" {}
		_prev2	("Prev2", 2D) = "white" {}
		_draw	("draw", 2D) = "black" {}
		_width	("width", float) = 1024
		_height	("height", float) = 1024
		_speed	("speed", float) = 10
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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.uv.xy);
				return o;
			}

			sampler2D _prev1;
			sampler2D _prev2;
			sampler2D _draw;
			float _width;
			float _height;

			float _speed;
			fixed4 frag(v2f i) : COLOR
			{
				// 波紋の移動距離の算出
				float strideX = _speed / _width;
				float strideY = _speed / _height;

				float4 retval = tex2D(_prev1, i.uv)*2.0 - tex2D(_prev2, i.uv) + 
				(
				tex2D(_prev1, float2(i.uv.x + strideX, i.uv.y))
				+ tex2D(_prev1, float2(i.uv.x - strideX, i.uv.y))
				+ tex2D(_prev1, float2(i.uv.x, i.uv.y + strideY))
				+ tex2D(_prev1, float2(i.uv.x, i.uv.y - strideY))
				- tex2D(_prev1, i.uv)*4
				) * 0.5;

				float4 halfVec = float4(0.5, 0.5, 0.5, 0.0);
				retval = halfVec + (retval - halfVec) * 0.985;
				retval += tex2D(_draw, i.uv);

				fixed4 col = retval;
				return col;
			}

			ENDCG
		}		
	} 
	FallBack "Diffuse"
}
