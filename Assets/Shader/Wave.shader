Shader "Custom/Wave" {
	Properties
    {
        _Color		("Color",  Color)	= (1.0, 0.0, 0.0, 1.0)
		_Position	("Position", Vector)= (0, 0, 0)
		_Params		("Params", Vector)	= (1, 20, 20, 10)
		_Vector		("Vector", Vector)	= (0, 0, 1, 0)
    }
	SubShader {

		Tags { "RenderType" = "Opaque" }

		Pass{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _Color;
			float4 _Params;
			float3 _Vector;
			float3 _Position;

			float w;

			struct Input{
				float4 pos : POSITION;

			};

			struct Output{
				float4 pos : SV_POSITION;

			};


			Output vert(Input input){
				Output o;
				w = length(_Position - _Vector);

				// Moving wave.
				w -= _Time.y * _Params.w;

				// Get modulo (w % params.z / params.z)
				w /= _Params.z;
				w = w - floor(w);

				// Make the gradient steeper.
				float p = _Params.y;
				w = (pow(w, p) + pow(1 - w, p * 4)) * 0.5;

				// Amplify.
			    w *= _Params.x;

				o.pos = mul(UNITY_MATRIX_MVP, input.pos);

				return o;
			}

			fixed4 frag(Output output) : COLOR{
				return fixed4(_Color);
			}

			ENDCG
		}
	} 
	FallBack Off
}
