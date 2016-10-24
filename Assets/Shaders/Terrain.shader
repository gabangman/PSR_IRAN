Shader "0PGL/Terrain" 
{

//Diffuse Vertex Colored

	Properties 
	{
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
		
		struct Input 
		{
		    float2 uv_MainTex;	
		    fixed3 color : COLOR;
		};
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
		    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);	
		    o.Albedo = c.rgb * IN.color.rgb;	
		}
		
		ENDCG
	}
	
	Fallback "0PGL/Mobile/VertexLit"

}