Shader "0PGL/Character/khd_rim2"

{

	Properties 

	{
		_ApplyBuffColor("_ApplyBuffColor", Color) = (1.0, 1.0, 1.0, 1.0)
		
		_RimColor("_RimColor", Color) = (0.358209,0.358209,0.358209,1)

		_RimPower("_RimPower", Range(0.1,10) ) = 1.122058  //jks give wider range for tunning

		_Glossiness("_Glossiness", Range(0,10) ) = 0.4300518

		_SpecularColor("_SpecularColor", Color) = (0.5447761,0.5447761,0.5447761,1)

		_DiffuseColor("_DiffuseColor", Color) = (0.5447761,0.5447761,0.5447761,1)

		_diffuse("_diffuse", 2D) = "black" {}
		
	

	}

	

	SubShader 

	{

		Tags

		{

			"Queue"="Geometry"

			"IgnoreProjector"="False"

			"RenderType"="Opaque"

		}



		

		Cull Off

		ZWrite On

		ZTest LEqual

		ColorMask RGBA

		Fog

		{

		}





CGPROGRAM

#pragma surface surf BlinnPhongEditor  vertex:vert

#pragma target 2.0

		float4 _ApplyBuffColor;

		float4 _RimColor;

		float _RimPower;

		float _Glossiness;

		float4 _SpecularColor;

		float4 _DiffuseColor;

		sampler2D _diffuse;



		struct EditorSurfaceOutput {

			half3 Albedo;

			half3 Normal;

			half3 Emission;

			half3 Gloss;

			half Specular;

			half Alpha;

			half4 Custom;

			half4 BuffColor;
		};

			

		inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)

		{

			half3 spec = light.a * s.Gloss;

			half4 c;

			c.rgb = (s.Albedo * light.rgb + light.rgb * spec);

			c.a = s.Alpha;

			return c * s.BuffColor;
		}



		inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)

		{

			half3 h = normalize (lightDir + viewDir);

			

			half diff = max (0, dot ( lightDir, s.Normal ));

			

			float nh = max (0, dot (s.Normal, h));

			float spec = pow (nh, s.Specular*128.0);

			

			half4 res;

			res.rgb = _LightColor0.rgb * diff;

			res.w = spec * Luminance (_LightColor0.rgb);

			res *= atten * 2.0;



			return LightingBlinnPhongEditor_PrePass( s, res );

		}

			

		struct Input 

		{

			float2 uv_diffuse;

			float3 viewDir;

		};



		void vert (inout appdata_full v, out Input o) 

		{

		}

			



		void surf (Input IN, inout EditorSurfaceOutput o) 

		{

			o.Normal = float3(0.0,0.0,1.0);

			o.Alpha = 1.0;

			o.Albedo = 0.0;

			o.Emission = 0.0;

			o.Gloss = 0.0;

			o.Specular = 0.0;

			o.Custom = 0.0;

				

			float4 Tex2D0=tex2D(_diffuse,(IN.uv_diffuse.xyxy).xy) * _DiffuseColor;

			float4 Fresnel0_1_NoInput = float4(0,0,1,1);

			float4 Fresnel0=(1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel0_1_NoInput.xyz ) )).xxxx;

			

			float4 Multiply0=_RimColor;

			if (Fresnel0.x < 1) 

			{

				float4 Pow0=pow(Fresnel0,_RimPower.xxxx);

				Multiply0 *= Pow0;

			}

			else //jks if dot product of normal vector and look vector is less than 0, don't apply rim.

			{

				Multiply0=0;

			}

			o.Albedo = Tex2D0;

			o.Emission = Multiply0;

			o.Specular = _Glossiness.xxxx;

			o.Gloss = _SpecularColor;

			o.Normal = normalize(o.Normal);

			o.BuffColor = _ApplyBuffColor;
		}

ENDCG



	}

	Fallback "Diffuse"

}