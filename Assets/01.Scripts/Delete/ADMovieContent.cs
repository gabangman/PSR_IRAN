using UnityEngine;
using System.Collections;

public class ADMovieContent : MonoBehaviour {

	public UILabel[] lbtext;
	public Transform timeBGAD1, timeArrowAD1;
	public Transform timeBGAD2, timeArrowAD2;
	void Start(){	
		lbtext[1].text = KoStorage.GetKorString("72702");
		lbtext[2].text = KoStorage.GetKorString("72703");// "연료받기?";
		lbtext[3].text = KoStorage.GetKorString("72704");// "연료받기?";
		lbtext[4].text = KoStorage.GetKorString("72705");// "연료받기?";
		lbtext[5].text = KoStorage.GetKorString("72706");// "연료받기?";

		lbtext[6].text = KoStorage.GetKorString("72707");// "연료받기?";
		lbtext[7].text  = KoStorage.GetKorString("72708");
		lbtext[8].text  = KoStorage.GetKorString("72708");
//		lbtext[0].text = AD1Time;
//		lbtext[9].text = AD2Time;
		Vungle.init( "5583d39b6ebf47221a0002d3", "5583d39b6ebf47221a0002d3" );

	}

	void OnClose(){
		NGUITools.FindInParents<SubMenuWindow>(gameObject).OnCloseClick();
	}

	void OnWatch1(){

		Utility.LogWarning("OnWatch1");
	//	UnityAdsHelper.ShowAd();
	}


	void OnWatch2(){
		Utility.LogWarning("OnWatch2");
		Vungle.playAd();
		Vungle.setSoundEnabled( false );
	}

	void OnReward1(){
		//Utility.LogWarning("OnReward1");
	}

	void OnReward2(){
		//Utility.LogWarning("OnReward2");
	}

	void OnEnable()
	{
		Vungle.onAdStartedEvent += onAdStartedEvent;
		Vungle.onAdEndedEvent += onAdEndedEvent;
		Vungle.onAdViewedEvent += onAdViewedEvent;
		Vungle.onCachedAdAvailableEvent += onCachedAdAvailableEvent;
	}
	
	
	void OnDisable()
	{
		Vungle.onAdStartedEvent -= onAdStartedEvent;
		Vungle.onAdEndedEvent -= onAdEndedEvent;
		Vungle.onAdViewedEvent -= onAdViewedEvent;
		Vungle.onCachedAdAvailableEvent -= onCachedAdAvailableEvent;
	}
	
	
	void onAdStartedEvent()
	{
		Utility.LogWarning( "onAdStartedEvent" );
	}
	
	
	void onAdEndedEvent()
	{
		Utility.LogWarning( "onAdEndedEvent" );
	}
	
	
	void onAdViewedEvent( double watched, double length )
	{
		Utility.LogWarning( "onAdViewedEvent. watched: " + watched + ", length: " + length );
	}
	
	
	void onCachedAdAvailableEvent()
	{
		Utility.LogWarning( "onCachedAdAvailableEvent" );
	}



}
