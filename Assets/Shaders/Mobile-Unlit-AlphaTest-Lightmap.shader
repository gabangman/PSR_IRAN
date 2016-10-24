Shader "0PGL/Mobile/Transparent/Unlit-AlphaTest-Lightmap" {
//UNlit
//Lightmap support
//Texture
//AlphaTest

    Properties
    {
//		_Color ("Main Color", Color) = (1,1,1,1) //jks main color added
        _MainTex ("Base (RGB) Trans (A)", 2D ) = "white" {}
		_Cutoff ("Base Alpha cutoff", Range (0,.9)) = .5
   }

    SubShader
    {
        Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout" }
        //ZWrite Off
        //Blend SrcAlpha OneMinusSrcAlpha 
        
        Cull Off
              
        Pass
        {			
            CGPROGRAM

                #include "UnityCG.cginc"
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF

                struct v2f
                {
                    half4 color : COLOR;
                    fixed4 pos : SV_POSITION;
                    fixed2 uv[2] : TEXCOORD0;
                };

//				fixed4 _Color;
                sampler2D _MainTex;
                fixed4 _MainTex_ST;
				float _Cutoff;

                #ifdef LIGHTMAP_ON
                fixed4 unity_LightmapST;
                sampler2D unity_Lightmap;
                #endif

                v2f vert(appdata_full v)
                {
                    v2f o;
                    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                    o.uv[0] = TRANSFORM_TEX(v.texcoord, _MainTex);

                    #ifdef LIGHTMAP_ON
                    o.uv[1] = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    #endif
                    
                    return o;
                }

                fixed4 frag(v2f i) : COLOR
                {
                    fixed4 c = tex2D(_MainTex, i.uv[0]);// * _Color;  //jks main color added

                    #ifdef LIGHTMAP_ON
                    c.rgb *= DecodeLightmap(tex2D(unity_Lightmap, i.uv[1]));
                    #endif
				
					clip(c.a - _Cutoff);
		            //c.a *= i.color.a;

                    return c;
                }

            ENDCG
        }
    
     }
}