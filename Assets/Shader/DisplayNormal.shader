﻿// 適用したモデルの法線をカラーレンダリングするシェーダー
Shader "Custom/DisplayNormal" {
	SubShader {
		Pass{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f{
				float4 pos : SV_POSITION;
				fixed3 color : COLOR0;
			};

			v2f vert(appdata_base v){
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.normal * 0.5 + 0.5;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target{
				return fixed4(i.color, 1);
			}

			ENDCG
		}
	} 
	FallBack Off
}
