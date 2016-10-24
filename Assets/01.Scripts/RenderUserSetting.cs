using UnityEngine;
using System.Collections;

public class RenderUserSetting : MonoBehaviour {

	private bool fog;
	private Color fogColor;
	private FogMode fogMode;// = FogMode.ExponentialSquared;
	private float fogDensity;// = 0.01f;
	private float linearFogStart;// = 0.0f;
	private float linearFogEnd;// = 300.0f;
	private Color ambientLight;// = new Color(0.2f, 0.2f, 0.2f, 1.0f);
	private float haloStrength;// = 0.5f;
	private float flareStrength;// = 1.0f;
	private float flareFadeSpeed;
	private Material Mat;
	public void SetRaceRender(string strScene){
		//Utility.LogWarning("SetRaceRender " + strScene);
		switch(strScene){
		case "Track1":{
			fog = true;
			fogColor = new Color32(125,222,231,255);
			fogMode = FogMode.Exponential;
			fogDensity = 0.002f;
			linearFogEnd = 300;
			linearFogStart = 0.0f;
			ambientLight = new Color32(114,143,148,255);
			haloStrength = 0.5f;
			flareStrength = 1;
			flareFadeSpeed = 3;
			Mat = null;
		}break;
		case "Track2":{
			fog = true;
			fogColor = new Color32(124,172,155,255);
			fogMode = FogMode.Exponential;
			fogDensity = 0.002f;
			linearFogEnd = 300;
			linearFogStart = 0.0f;
			ambientLight = new Color32(82,80,84,255);
			haloStrength = 0.5f;
			flareStrength = 1;
			flareFadeSpeed = 3;	Mat = null;
		}break;
			
		case "Track3":{
			fog = true;
			fogColor = new Color32(175,176,92,255);
			fogMode = FogMode.Exponential;
			fogDensity = 0.001f;
			linearFogEnd = 300;
			linearFogStart = 0.0f;
			ambientLight = new Color32(82,80,84,255);
			haloStrength = 0.5f;
			flareStrength = 1;
			flareFadeSpeed = 3;	Mat = null;
		}
			break;
		case "Track4":{
			fog = true;
			fogColor = new Color32(22,54,86,255);
			fogMode = FogMode.Exponential;
			fogDensity = 0.001f;
			linearFogEnd = 300;
			linearFogStart = 0.0f;
			ambientLight = new Color32(39,37,41,255);
			haloStrength = 0.5f;
			flareStrength = 1;
			flareFadeSpeed = 3;	Mat = null;
		}
			break;
		case "Track5":
		{
			fog = true;
			fogColor = new Color32(223,206,136,255);
			fogMode = FogMode.Exponential;
			fogDensity = 0.002f;
			linearFogEnd = 300;
			linearFogStart = 0.0f;
			ambientLight = new Color32(111,109,114,255);
			haloStrength = 0.5f;
			flareStrength = 1;
			flareFadeSpeed = 3;	Mat = null;
		}
			break;
		case "Track6":{
			fog = true;
			fogColor = new Color32(231,184,125,255);
			fogMode = FogMode.Exponential;
			fogDensity = 0.002f;
			linearFogEnd = 300;
			linearFogStart = 0.0f;
			ambientLight = new Color32(114,143,148,255);
			haloStrength = 0.5f;
			flareStrength = 1;
			flareFadeSpeed = 3;	Mat = null;
		}
			break;
		case "Weekly":
		{
			fog = true;
			fogColor = new Color32(146,216,216,255);
			fogMode = FogMode.Exponential;
			fogDensity = 0.002f;
			linearFogEnd = 300;
			linearFogStart = 0.0f;
			ambientLight = new Color32(82,80,84,255);
			haloStrength = 0.5f;
			flareStrength = 1;
			flareFadeSpeed = 3;	Mat = null;
		}
			break;
		case "Lobby":
		{
			fog = false;
			fogColor = new Color32(43,41,40,255);
			fogMode = FogMode.Exponential;
			fogDensity = 0.05f;
			linearFogEnd = 300;
			linearFogStart = 0.0f;
			ambientLight = new Color32(126,126,126,255);
			haloStrength = 0.5f;
			flareStrength = 1;
			flareFadeSpeed = 3;	Mat = null;
		}
			break;
		case "Event_Track1":
		{
			fog = true;
			fogColor = new Color32(176,182,147,255);
			fogMode = FogMode.Exponential;
			fogDensity = 0.003f;
			linearFogEnd = 300;
			linearFogStart = 0.0f;
			ambientLight = new Color32(82,80,84,255); //skyCube_mat2

			haloStrength = 0.5f;
			flareStrength = 1;
			flareFadeSpeed = 3; Mat	 = (Material)Resources.Load("SkyBox/Sky2/SkyCube_Mat2",typeof(Material));
		}
			break;
		case "Drag_Track1":
		{
			fog = true;
			fogColor = new Color32(49,78,103,255);
			fogMode = FogMode.Exponential;
			fogDensity = 0.005f;
			linearFogEnd = 300;
			linearFogStart = 0.0f;
			ambientLight = new Color32(31,38,37,255); //skyCube_mat1
			haloStrength = 0.5f;
			flareStrength = 1;
			flareFadeSpeed = 3;	Mat = (Material)Resources.Load("SkyBox/Sky1/SkyCube_Mat1",typeof(Material)); 
		}
			break;
		default:
		{
			fog = false;
			fogColor = new Color32(128,128,128,255);
			fogMode = FogMode.ExponentialSquared;
			fogDensity = 0.01f;
			linearFogEnd = 300;
			linearFogStart = 0.01f;
			ambientLight = new Color32(51,51,51,255);
			haloStrength = 0.5f;
			flareStrength = 1;
			flareFadeSpeed = 3;	Mat = null;

		}
			break;
		}
		renderSettingInit();
	}

	void renderSettingInit() {
		RenderSettings.fog = fog;
		RenderSettings.fogColor = fogColor;
		RenderSettings.fogMode = fogMode;
		RenderSettings.fogDensity = fogDensity;
		RenderSettings.fogStartDistance = linearFogStart;
		RenderSettings.fogEndDistance = linearFogEnd;
		RenderSettings.ambientLight = ambientLight;
		RenderSettings.haloStrength = haloStrength;
		RenderSettings.flareStrength = flareStrength;
		RenderSettings.flareFadeSpeed = flareFadeSpeed;
		RenderSettings.skybox = Mat;
	}


}
