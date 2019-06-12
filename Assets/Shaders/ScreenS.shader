// Copyright ©Seth Piezas

Shader "Custom/ScreenS"
{
    Properties
    {
        _OpeningTex ("Opening Texture", 2D) = "white" {}  
        _SelectionTex ("Selection Texture", 2D) = "white" {}  
        _CrossTex ("XO Texture", 2D) = "white" {}    
        _WinnerTex ("Winner Tex", 2D) = "white" {}
              
        _Screen ("Screen", Float) = 0
        _Selector ("Selector", int) = 0
        _WinnerSelector  ("Winner", int) = 0
        _GS ("GS", int) = 0
        
        _Color ("Color", Color) = (1,1,1,1)
        _BaseColor ("BaseColor", Color) = (0,0,0,0)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Alpha ("Alpha", Range(0,1)) = 1.0
        _NoiseTex ("Noise Tex", 2D) = "white" {}
        _NoiseSelector ("NoiseUV", Vector) = (0,0,1,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        float _Screen;
        sampler2D _OpeningTex;
        sampler2D _SelectionTex;
        sampler2D _CrossTex;
        sampler2D _WinnerTex;
        sampler2D _MainTex;
        sampler2D _NoiseTex;
        half4 _NoiseSelector;
        int _GS;
        int _Selector;
        int _WinnerSelector;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _BaseColor;
        half _Alpha;
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_CrossTex;
        };


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        void vert (inout appdata_full v) {
//          v.vertex.xyz += v.normal * _Amount;
        }
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
        
            half v = 0;
            float b0 = 0.2;
            if(_Selector == 1) b0 = 0.42;
            if(_Selector == 2) b0 = 0.65;
            float b1 = b0 + 0.2;
            if(_Screen < 1.0) {
                v = tex2D(_OpeningTex, IN.uv_MainTex).r;
            } else if(_Screen < 2.0) {
                v = tex2D(_SelectionTex, IN.uv_MainTex).r;
                float2 uv = IN.uv_MainTex.xy;
                if(uv.y > b0 && uv.y < b1 && uv.x > 0.1 && uv.x < 0.9) v = 1.0 - v;
            } else if(_Screen < 3.0) {
                half4 tmp = tex2D(_CrossTex, IN.uv_CrossTex);
                int x = (int)IN.uv_CrossTex.x;
                int y = (int)IN.uv_CrossTex.y;
                int l = y*3 + x;
                int bits = (_GS >> (l*2));
                v = tmp.r;
                if(bits & 0x01 == 1) {
                    int xo = bits & 0x02;
                    if(xo == 0) {
                        v += tmp.g;
                    } else {
                        v += tmp.b;
                    }
                }
                
            } else if(_Screen < 4.0) {
                half4 tmp = tex2D(_WinnerTex, IN.uv_MainTex);
                if(_WinnerSelector == 0) v = tmp.r;
                if(_WinnerSelector == 1) v = tmp.g;
                if(_WinnerSelector == 2) v = tmp.b;
            }
            half noise = tex2D(_NoiseTex, IN.uv_MainTex*0.5 + _NoiseSelector.xy).r*_NoiseSelector.z + (1.0 - _NoiseSelector.z);
            half vhold = max(sin(IN.uv_MainTex.y*10 + _Time*20), 0)*0.7 + 0.3;
            half h = sin(IN.uv_MainTex.y*600)*0.5 + 0.5;
            v *= h;
            v += 0.1*h;
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color + _BaseColor;
            o.Albedo = _BaseColor.rgb*_Alpha*noise;
            o.Emission = c.rgb*v*3*_Alpha*vhold*noise;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = v*_Alpha*vhold;
            //o.Emission = half3(noise, noise, noise);        
        }
        ENDCG
    }
    FallBack "Diffuse"
}
