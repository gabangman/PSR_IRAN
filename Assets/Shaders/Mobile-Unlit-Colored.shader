Shader "0PGL/Mobile/Unlit-Colored(Iris-shot)" {
//UNlit
//Texture
//Overlay

    Properties
    {
		_Color ("Main Color", Color) = (1,1,1,1) //jks main color added
        _MainTex ("Base (RGB) Trans (A)", 2D ) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Opaque" }
        ZWrite Off
       
        Pass
        {
            CGPROGRAM

                #include "UnityCG.cginc"
                #pragma vertex vert
                #pragma fragment frag

                struct v2f
                {
                    fixed4 pos : SV_POSITION;
                    fixed2 uv : TEXCOORD0;
                };

				fixed4 _Color;
                sampler2D _MainTex;
                fixed4 _MainTex_ST;

                v2f vert(appdata_full v)
                {
                    v2f o;
                    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                    return o;
                }

                fixed4 frag(v2f i) : COLOR
                {
                    fixed4 c = tex2D(_MainTex, i.uv) * _Color;  //jks main color added
                    
                    return c;
                }

            ENDCG
        }
    }
}