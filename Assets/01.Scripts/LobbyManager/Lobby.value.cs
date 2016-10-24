using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
public partial class LobbyManager : MonoBehaviour {

	public GameObject  lobby,myFuel;
	public GameObject TableShop;
	public Camera[] LobbyCamList;
	public GameObject elevator;
	public GameObject luckBox;
	public GameObject RotateTable;
	//public UILabel lbSeason;
	public GameObject rankObj;
	public GameObject MenuTop, MenuCenter;
	public GameObject MenuBottom;//, worldmap;
	public GameObject gLevel;
	public UISprite spLevelBar;
	public UISprite spriteSeason;
	private enum buttonState {MAP_EVENT,MY_SHOP,CAR_SHOW,CAR_SHOP,CONTAINER_SHOP,ONINVEN,TEAM,CLAN,MAP_CLAN,MAP_RANK,MAP_RACE,MAP_CHAMPION,MAPTORACEMODE,RACEMODE,
		BEFROECOIN, NOTHING, WAIT, MENU,TEAM_UP_CAR,TEAM_UP_CREW, MAP_PVP,
		MYCAR, LOBBY,OVER,START, MAP, MAPTOLOBBY, MYCREW, CAR_SHOP_DEAL,
		UPGRADE_CAR, UPGRADE_CREW, TITLETOLOBBY, Sponsor, LEVELUP};
	private buttonState btnstate;
	private buttonState beforeState;
	private cameraAniCtrl camAni, camAni_Tour;
	private StringBuilder sb = new StringBuilder();
	private GameObject activeObject;
	
	bool isLobby;
	bool isPause = true;
	GameObject ElevatorCar = null;
	
	string CrewUpNameStock = null;
	string CarUpNameStock = null;
	string CrewUpNameTour = null;
	string CarUpNameTour = null;
	
	TableShopaction _table;
	
	
	bool isTeamInfo = false;
	GameObject TipInfo = null;
	string strTip = string.Empty;
	bool isShowWin = false;
	System.Action OnBackCall, OnBackFunction;
	
	
	private GameObject[] raceoff;
	private GameObject raceinfo, status;
	bool isCreateCarItem = false;
	bool isCreateCrewItem = false;
	bool isLobbyRotation = false;
	bool isCoinShop = false;
	bool isShop = false;
	Mode rMode;
	bool isTeamRotate;
	string myCrewName, myCarName;
}
