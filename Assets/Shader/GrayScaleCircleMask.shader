Shader "Hidden/GrayScaleCircleMask"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Radius("Radius", float) = 0.5
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
				o.uv -= fixed2(0.5, 0.5);
				return o;
			}
			
			sampler2D _MainTex;
			float _Radius;

			fixed4 frag (v2f i) : SV_Target
			{
				float distance = sqrt(pow(i.uv.x, 2) + pow(i.uv.y, 2));
				
				i.uv += fixed2(0.5, 0.5);
				fixed4 col = tex2D(_MainTex, i.uv);


				if (distance < _Radius) {
					// just invert the colors
					col = 1 - col;
				}

				return col;
			}
			ENDCG
		}
	}
}
