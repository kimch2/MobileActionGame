Shader "Hidden/ImageRotate"
{
	Properties
	{
		_MainTex		("Texture", 2D)			= "white"{}
		_RotatePower	("RotatePower", float)	= 1.0
		_Radius			("Radius", float)		= 0.5
		_CenterX		("CenterX", float)		= 0.5	// UV座標を弄る時中心を0として考えたいため用意	
		_CenterY		("CenterY", float)		= 0.5	// UV座標を弄る時中心を0として考えたいため用意	
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

			sampler2D _MainTex;
			float _CenterX;
			float _CenterY;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				float2 uv = v.uv.xy - float2(_CenterX, _CenterY).xy;	// 中心位置補正
				o.uv = uv;
				return o;
			}
			
			float _RotatePower;
			float _Radius;

			fixed4 frag (v2f i) : SV_Target
			{
				//fixed4 col = tex2D(_MainTex, i.uv)
				//fixed4 col = fixed4(tex2D(_MainTex, i.uv).xyz, 1.0);
				
				float2	offset	= i.uv;												// 現在のuv座標を得る
				float	angle	= 1.0 - length(offset / float2(_Radius, _Radius));	// 中心位置から離れていたら角度が弱く
				angle = max(0, angle);												// 0以下にならないように修正
				angle = pow(angle, 2) * _RotatePower * _Time.y;						// 時間経過で捻る
				float cosLength, sinLength;
				sincos(angle, sinLength, cosLength);
				float2x2 rotateMat = float2x2(cosLength, -sinLength, sinLength, cosLength);

				float2 uv = mul(offset, rotateMat);
				uv += float2(_CenterX, _CenterY).xy;
				return tex2D(_MainTex, uv);
			}
			ENDCG
		}
	}
}
