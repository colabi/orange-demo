// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/CheapGdRay"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_Falloff ("Falloff", Float) = 1.0
		_Power ("Power", Float) = 1.0
		_Size ("Size", Float) = 1.0
		_Center ("Center", Vector) = (0,0,0,1)
		_Top ("Top", Float) = 1.0
		_Flare ("Flare", Float) = 1.0
		_C0 ("C0", Float) = 1.0
		_C1 ("C1", Float) = -1.0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		Cull Back
		Blend One One
		ZWrite Off
		LOD 100

		Pass
		{
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 lpos : TANGENT;
				float4 cpos : NORMAL;
			};

			float4 _Center;
			float _Falloff;
			float4 _Color;
			float _Power;
			float _Size;
			float _Top;
			float _Flare;
			float _C0;
			float _C1;
			
			v2f vert (appdata v)
			{
				v2f o;
				if(v.vertex.x > 0) {
					v.vertex.x = _C0;
				} else {
					v.vertex.x = _C1;				
				}
				v.vertex.xz *= _Size;
				if(v.vertex.y > 0) {
					v.vertex.y *= _Top;
					v.vertex.xz *= _Flare;
				}
				v.vertex.y *= 2;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.lpos =  mul(UNITY_MATRIX_MV, v.vertex);
				o.cpos =  mul(UNITY_MATRIX_MV, _Center);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv0 = i.cpos.xy;//i.vertex.w;
				float2 uv1 = i.lpos.xy;//i.lpos.w;
				float v = length(uv0 - uv1)*_Falloff;
				v = max(0.0, 1.0 - v);
				v = pow(v,_Power);
				float kill = 1.0;
				/*
				if(abs(i.lpos.x) > _Top) {
					kill = 0.0;
				}
				*/
				fixed4 col = fixed4(_Color.rgb*v, 0);
				return col;
			}
			ENDCG
		}
		/*
					Pass
		{
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 lpos : TANGENT;
				float4 cpos : NORMAL;
			};

			float4 _Center;
			float _Falloff;
			float4 _Color;
			float _Power;
			float _Size;
			float _Top;
			float _Flare;
			float _C0;
			float _C1;
			
			v2f vert (appdata v)
			{
				v2f o;
				v.vertex.xyz *= 0.97;
				if(v.vertex.x > 0) {
					v.vertex.x = _C0;
				} else {
					v.vertex.x = _C1;				
				}
				v.vertex.xz *= _Size;
				if(v.vertex.y > 0) {
					v.vertex.y *= _Top;
					v.vertex.xz *= _Flare;
				}
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.lpos =  mul(UNITY_MATRIX_MV, v.vertex);
				o.cpos =  mul(UNITY_MATRIX_MV, _Center);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv0 = i.cpos.xy;//i.vertex.w;
				float2 uv1 = i.lpos.xy;//i.lpos.w;
				float v = length(uv0 - uv1)*_Falloff;
				v = max(0.0, 1.0 - v);
				v = pow(v,_Power);
				float kill = 1.0;
				/*
				if(abs(i.lpos.x) > _Top) {
					kill = 0.0;
				}
				*/
				fixed4 col = fixed4(_Color.rgb*v, 0);
				return col;
			}
			ENDCG
		}
Pass
		{
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 lpos : TANGENT;
				float4 cpos : NORMAL;
			};

			float4 _Center;
			float _Falloff;
			float4 _Color;
			float _Power;
			float _Size;
			float _Top;
			float _Flare;
			float _C0;
			float _C1;
			
			v2f vert (appdata v)
			{
				v2f o;
				v.vertex.xyz *= 0.94;
				if(v.vertex.x > 0) {
					v.vertex.x = _C0;
				} else {
					v.vertex.x = _C1;				
				}
				v.vertex.xz *= _Size;
				if(v.vertex.y > 0) {
					v.vertex.y *= _Top;
					v.vertex.xz *= _Flare;
				}
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.lpos =  mul(UNITY_MATRIX_MV, v.vertex);
				o.cpos =  mul(UNITY_MATRIX_MV, _Center);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv0 = i.cpos.xy;//i.vertex.w;
				float2 uv1 = i.lpos.xy;//i.lpos.w;
				float v = length(uv0 - uv1)*_Falloff;
				v = max(0.0, 1.0 - v);
				v = pow(v,_Power);
				float kill = 1.0;
				/*
				if(abs(i.lpos.x) > _Top) {
					kill = 0.0;
				}
				*/
				fixed4 col = fixed4(_Color.rgb*v, 0);
				return col;
			}
			ENDCG
		}
		*/
	}
}
