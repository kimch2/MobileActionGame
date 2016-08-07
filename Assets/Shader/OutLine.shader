Shader "Unlit/OutLine"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LineColor ("LineColor", Color) = (0.0, 0.0, 0.0, 1.0)
		_LineWidth ("LineWidth", float) = 0.3
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD1;	// 視点方向
			};

			sampler2D _MainTex;
			float4	_LineColor;
			float	_LineWidth;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex	= mul(UNITY_MATRIX_MVP, v.vertex);
				o.normal	= v.normal;
				o.uv		= v.uv;
				o.viewDir	= ObjSpaceViewDir(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				i.viewDir	= normalize(i.viewDir);
				float edge	= dot(i.normal, i.viewDir);
				fixed4 col	= edge < _LineWidth ? _LineColor : tex2D(_MainTex, i.uv);				
						
				return col;
			}
			ENDCG
		}
	}
}
