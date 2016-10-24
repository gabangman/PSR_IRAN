Shader "0PGL/Water(PlaceHolder)" 
{

//Diffuse Vertex Colored

	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
	    _MainTex ("Base (RGB)", 2D) = "white" {}
	}
	
	SubShader 
	{
	    Tags { "RenderType"="Opaque" }
	    LOD 150
		UsePass "VertexLit/SHADOWCOLLECTOR"
	
		CGPROGRAM
		#pragma surface surf Lambert noforwardadd
		
		sampler2D _MainTex;
		fixed4 _Color;
		
		struct Input 
		{
		    float2 uv_MainTex;	
		    fixed3 color : COLOR;
		};
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
		    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;	
		    o.Albedo = c.rgb * IN.color.rgb;	
		}
		
		ENDCG
	}
	
	Fallback "Mobile/VertexLit"

}