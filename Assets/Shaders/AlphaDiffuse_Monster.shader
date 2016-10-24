Shader "0PGL/Character/AlphaDiffuse_Monster" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

SubShader {
    Tags {"RenderType"="Transparent" "Queue"="Transparent"}
    // Render into depth buffer only
	UsePass "VertexLit/SHADOWCOLLECTOR"
	UsePass "VertexLit/SHADOWCASTER"
    
    Pass 
    {
        ColorMask 0
    }
    // Render normally
    Pass {
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        Material {
            Diffuse [_Color]
            Ambient [_Color]
        }
        Lighting On
        SetTexture [_MainTex] {
            Combine texture * primary DOUBLE, texture * primary
        } 
    }
}
}

//Properties {
//	_Color ("Main Color", Color) = (1,1,1,1)
//	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
//}
//
//SubShader {
//	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
//	LOD 200
//
//CGPROGRAM
//#pragma surface surf Lambert alpha
//
//sampler2D _MainTex;
//fixed4 _Color;
//
//struct Input 
//{
//	float2 uv_MainTex;
//};
//
//void surf (Input IN, inout SurfaceOutput o) 
//{
//	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
//	o.Albedo = c.rgb;
//	o.Alpha = c.a;
//}
//ENDCG
//}
//
//Fallback "Transparent/VertexLit"
//}
