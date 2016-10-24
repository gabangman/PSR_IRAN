Shader "0PGL/Water/Stream_Bottom_Blend" {
//jks shader for water with bottom texture blending
    Properties
    {
		_Color ("Main Color", Color) = (1,1,1,1) 
        _MainTex ("Base (RGB) Trans (A)", 2D ) = "white" {}
		_BlendTex ("Blend (RGB) Trans (A)", 2D ) = "white" {}
		_Blend ("Blend", Range (0,1)) = 0.5
		_BottomTex ("Bottom (RGB) Trans (A)", 2D ) = "white" {}		
		_BottomTexBlend ("Bottom Blend", Range (0,1)) = 0.5
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        
        Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off
        ZWrite Off
       
        Pass
        {
            CGPROGRAM

                #include "UnityCG.cginc"
                #pragma vertex vert
                #pragma fragment frag

                struct v2f
                {
                    fixed4 color : COLOR;
                    fixed4 pos : SV_POSITION;
                    fixed2 uv[4] : TEXCOORD0;
                };

				fixed4 _Color;
                sampler2D _MainTex;
                sampler2D _BlendTex;
                sampler2D _BottomTex;
                fixed4 _MainTex_ST;
                fixed4 _BlendTex_ST;
                fixed4 _BottomTex_ST;
                fixed _Blend;
                fixed _BottomTexBlend;

                v2f vert(appdata_full v)
                {
                    v2f o;
                    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                    o.uv[0] = TRANSFORM_TEX(v.texcoord, _MainTex);					
					o.uv[2] = TRANSFORM_TEX(v.texcoord, _BlendTex);
					o.uv[3] = TRANSFORM_TEX(v.texcoord, _BottomTex);
					                 
                    o.color = v.color;         
                         
                    return o;
                }

                fixed4 frag(v2f i) : COLOR
                {                    
                    fixed4 blendtex = tex2D(_BlendTex, i.uv[2].xy);		
                    fixed4 bottomtex = tex2D(_BottomTex, i.uv[3].xy);		
                    
                    fixed4 c = tex2D(_MainTex, i.uv[0]);  
                    
                    c = lerp(c, blendtex, _Blend) * _Color;
                    c = lerp(c, bottomtex, _BottomTexBlend);
                    c.a = bottomtex.a;
                    
                    return c;
                }

            ENDCG
        }
    }
	
	Fallback "0PGL/Mobile/VertexLit"

}