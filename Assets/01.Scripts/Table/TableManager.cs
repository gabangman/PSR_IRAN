
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;
using Pathfinding.Serialization.JsonFx;


// 엑셀 테이블 관리자.
public class TableManager : MonoBehaviour
{
	static TableManager instance;
	
	[SerializeField] bool patchable;
	//string patchURL = "http://dev.pangalore.com/~gs/patch";
	//[NonSerialized] public const string versionFileName = "version.txt";
	//[NonSerialized] public const string nameOfEncryptedPatchFileListFile = "0.gs";
	//[NonSerialized] public const string password = "w9v7@#%j89s";
	//[NonSerialized] public const string salt = "Gold Scramble";
	
	// DataSetting.xlsx의 CustomConvert 쉬트의 값과 일치해야 한다.
	/*
	public enum ResourceType
	{
		None = 0,
		Dollar = 1,
		Coin = 2,
		Free = 3,
	}

	public static string GetResourceName(ResourceType resourceType)
	{
		switch (resourceType)
		{
		case ResourceType.Dollar:
			return "Dollar";
		case ResourceType.Coin:
			return "Coin";
		case ResourceType.Free:
			return "Free";
		default:
			return null;
		}
	}
	*/
	
	//public TextAsset Common_Car_StatusTable;
	//public TextAsset Common_Crew_StatusTable;
	//public TextAsset Common_Sponsor_StatusTable;
	//public TextAsset Upgrade_Car_RatioTable;
	//public TextAsset Upgrade_Crew_RatioTable;
	//public TextAsset enTable;
	//public TextAsset koTable;
	//public TextAsset ExpTable;
	//public TextAsset TrackTable;
	//public TextAsset CostTable;
	//public TextAsset AttendTable;
	//public TextAsset ModeChampionTable;
	//public TextAsset Common_CardTable;
	//public TextAsset Common_RewardTable;
	//public TextAsset AICarTable;
	//public TextAsset AICrewTable;

	public EnStorage enStorageLink;
	public KoStorage koStorageLink;
	public Common_Car_Status Common_Car_Link;
	public Common_Crew_Status Common_Crew_Link;
	public Common_Sponsor_Status Common_Sponsor_Link;


	public Common_Exp_Range Common_Exp_Link;
	public Common_Cost Common_Cost_Link;
	public Common_Track Common_Track_Link;
	public Common_Mode_Champion Common_Mode_Champion_Link;
	//public Common_Card Common_Card_Link;

	public Common_Reward Common_Reward_Link;
	public Upgrade_Car_Ratio Upgrade_Car_Link;
	public Upgrade_Crew_Ratio Upgrade_Crew_Link;
	public Common_Attend Common_Attend_Link;
	public ModeAICar modeaicarLink;

	public ModeAICrew modeaicrewLink;
	//public Common_WeeklyReward Common_WeeklyRewardLink;
	public Common_Items Common_ItemsLink;
	public Common_Team Common_TeamLink;
	public Common_Class Common_ClassLink;

	public Common_Achieve Common_ALink;
	public Common_Material Common_matLink;
	public Common_Mix Common_MixLink;
	public Common_Lucky Common_LuckyLink;
	public Common_VIP vipLink;

	public Common_ClubAI clubaiLink;
	public UpgradeCarCost carcostLink;
	public UpgradeCrewCost crewcostLink;
	public ModeAIDelay modeaiLink;
	public Common_Ratio RatioLink;

	string Common_Car_StatusTableString;
	string Common_Sponsor_StatusTableString;
	string Common_Crew_StatusTableString;
	string Upgrade_Car_RatioTableString;
	string Upgrade_Crew_RatioTableString;

	string Common_Mode_ChampionString;
	string enTableString;
	string koTableString;
	string ExpTableString;
	string TrackTableString;

	string CostTableString;
//	string CardTableString;
	string RewardTableString;
	string AttendTableString;
	string AICarTableString;

	string AICrewTableString;
//	string WeeklyRewardString;
	string ItemsString;
	string TeamString;
	string ClassString;

	string AchieveString;
	string MatString;
	string MixString;
	string LuckyString;
	string VIPString;

	string ClubAIString;
	string UpCarString;
	string UpCrewString;
	string AIDelayString;
	string RatioString;
	string PackageString;

	string ClubAIExpString;
	string ClubRewardString;
	public static bool initialized { get; private set; }
	
	void Awake()
	{
	
		instance = this;
		initialized = false;
		if(initialized){
			//Utility.LogWarning("tableClear");
			//ClearTable();
		}
	}
	
	void OnDestroy()
	{
		instance = null;
	}
	/*
	public static Common_Car_Status CarStatus
	{
		get { return instance.Common_Car_Link; }
	}
	public static Upgrade_Crew_Ratio CrewRatio
	{
		get { return instance.Upgrade_Crew_Link; }
	}
	public static Upgrade_Car_Ratio CarRatio
	{
		get { return instance.Upgrade_Car_Link; }
	}
	public static Common_Sponsor_Status SponsorStatus
	{
		get { return instance.Common_Sponsor_Link; }
	}
	public static Common_Crew_Status CrewStatus
	{
		get { return instance.Common_Crew_Link; }
	}
	*/
	public static EnStorage en
	{
		get { return instance.enStorageLink; }
	}

	//public static KoStorage ko
	//{
	//	get { return instance.koStorageLink; }
	//}

	public static void Initialize()
	{
		instance.StartCoroutine(instance.InitializeByUsingCoroutine());
	}
	
	IEnumerator InitializeByUsingCoroutine()
	{
		yield return StartCoroutine(InitializeTableString());
		yield return StartCoroutine(InitializeStorageSetFile());
		ClearTable();
		//Utility.Log("table");
		initialized = true;
	}
	
	IEnumerator InitializeTableString()
	{
		InitializeTableStringFromTextAsset();
		yield return null;
		/*
		if (patchable)
		{
			string versionFilePath = Application.persistentDataPath + "/" + versionFileName;
			yield return StartCoroutine(DownloadAndVerify(patchURL + "/" + versionFileName, versionFilePath));
			string serverVersion = File.ReadAllText(versionFilePath);
			if (!serverVersion.EndsWith(".0"))
			{
				string patchFileListFilePath = Application.persistentDataPath + "/" + nameOfEncryptedPatchFileListFile;
				
				const string versionKey = "Version";
				if (!PlayerPrefs.HasKey(versionKey) ||
				    serverVersion != GSCryptography.Decrypt(password, salt, Convert.FromBase64String(PlayerPrefs.GetString(versionKey))))
				{
					yield return StartCoroutine(DownloadAndVerify(patchURL + "/" + nameOfEncryptedPatchFileListFile, patchFileListFilePath));
					
					foreach (KeyValuePair<string, string> patchFileNamePair in LoadPatchFileNames())
						yield return StartCoroutine(DownloadAndVerify(patchURL + "/" + patchFileNamePair.Value
						                                              , Application.persistentDataPath + "/" + patchFileNamePair.Value));
					
					PlayerPrefs.SetString(versionKey,
					                      Convert.ToBase64String(GSCryptography.Encrypt(password, salt, Encoding.UTF8.GetBytes(serverVersion))));
				}
				
				// 존재하는 파일만 로드하게 하면 간단한데, 그러면 다운로드한 파일을 사용자가 임의로 지워 버렸을 때 이전 버전으로 오동작한다.
				// 그래서 이렇게 파일 목록을 읽어서 처리했는데, 지저분하므로 더 좋은 방법을 생각해야 한다.
				foreach (KeyValuePair<string, string> patchFileNamePair in LoadPatchFileNames())
				{
					string fileName = patchFileNamePair.Key;
					string encryptedFileName = patchFileNamePair.Value;
					
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
					if (fileNameWithoutExtension == commonDefenceStatusTable.name)
						commonDefenceStatusTableString = LoadDownloadedTable(encryptedFileName);
					//					else if (fileNameWithoutExtension == Common_PossessionTable.name)
					//						commonUnitStatusTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == Common_PossessionTable.name)
						Common_PossessionTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == Common_SingleMapsTable.name)
						Common_SingleMapsTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == commonUnitStatusTable.name)
						commonUnitStatusTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == costTable.name)
						costTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == enTable.name)
						enTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == experienceTable.name)
						experienceTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == koTable.name)
						koTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == lvBasicStatusTable.name)
						lvBasicStatusTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == lvBuildingStatusTable.name)
						lvBuildingStatusTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == lvDefenceStatusTable.name)
						lvDefenceStatusTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == lvHeadQuarterStatusTable.name)
						lvHeadQuarterStatusTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == lvSkillStatusTable.name)
						lvSkillStatusTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == lvUnitStatusTable.name)
						lvUnitStatusTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == shopTable.name)
						shopTableString = LoadDownloadedTable(encryptedFileName);
					else if (fileNameWithoutExtension == obstacleTable.name)
						obstacleTableString = LoadDownloadedTable(encryptedFileName);
				}
			}
		}*/
	}
	IEnumerator InitializeStorageSetFile()
	{
		InitializeStorageLink();
		yield return null;
	}
	/*
	Dictionary<string, string> LoadPatchFileNames()
	{
		return JsonReader.Deserialize<Dictionary<string, string>>(GSCryptography.Decrypt(password, salt, File.ReadAllBytes(
			Application.persistentDataPath + "/" + nameOfEncryptedPatchFileListFile)));
	}
	
	string LoadDownloadedTable(string encryptedFileName)
	{
		return GSCryptography.Decrypt(password, salt, 
		                              File.ReadAllBytes(Application.persistentDataPath + "/" + encryptedFileName));
	}
	
	IEnumerator Download(string sourceFileURL, string targetFilePath)
	{
		for (;;)
			using (WWW downloader = new WWW(sourceFileURL))
		{
			yield return downloader;
			
			if (String.IsNullOrEmpty(downloader.error))
			{
				File.WriteAllBytes(targetFilePath, downloader.bytes);
				break;
			}
			else
			{
				Utility.LogWarning(sourceFileURL + " download error: " + downloader.error);
				yield return new WaitForSeconds(1);
			}
		}
	}
	
	IEnumerator DownloadAndVerify(string sourceFileURL, string targetFilePath)
	{
		for (;;)
		{
			yield return StartCoroutine(Download(sourceFileURL, targetFilePath));
			
			const string md5Extension = ".md5";
			yield return StartCoroutine(Download(sourceFileURL + md5Extension, targetFilePath + md5Extension));
			
			if (GSCryptography.GetMD5(File.ReadAllBytes(targetFilePath)) == File.ReadAllText(targetFilePath + md5Extension))
				break;
		}
	}
	*/
	void InitializeTableStringFromTextAsset()
	{
		Common_Car_StatusTableString = Common_Car_StatusTable.text;
		Common_Sponsor_StatusTableString = Common_Sponsor_StatusTable.text;
		Common_Crew_StatusTableString = Common_Crew_StatusTable.text;
		ExpTableString = ExpTable.text;
		TrackTableString = TrackTable.text;

		CostTableString = CostTable.text;
	//	CardTableString = Common_CardTable.text;
		RewardTableString = Common_RewardTable.text;
		Upgrade_Car_RatioTableString = Upgrade_Car_RatioTable.text;
		Upgrade_Crew_RatioTableString = Upgrade_Crew_RatioTable.text;

		//enTableString = enTable.text;
		koTableString = koTable.text;
		Common_Mode_ChampionString = ModeChampionTable.text;
		AttendTableString = AttendTable.text;
		AICarTableString = AICarTable.text;

		AICrewTableString = AICrewTable.text;
	//	WeeklyRewardString = WeeklyRewardTable.text;
		ItemsString = ItemTable.text;
		TeamString = TeamTable.text;
		ClassString = ClassTable.text;

		AchieveString = AchieveTable.text;
		MatString = MatTable.text;
		MixString = MixTable.text;
		LuckyString = LuckyTable.text;
		VIPString = VIPTable.text;

		ClubAIString = ClubAITable.text;
		UpCarString = UpCarTable.text;
		UpCrewString = UpCrewTable.text;
		AIDelayString = AIDelayTable.text;
		RatioString = RatioTable.text;

		PackageString = PackageTable.text;
		ClubAIExpString = ClubAIExpTable.text;
		ClubRewardString = ClubRewardTable.text;
	}

	public TextAsset Common_Car_StatusTable;
	public TextAsset Common_Sponsor_StatusTable;
	public TextAsset Common_Crew_StatusTable;
	public TextAsset ExpTable;
	public TextAsset TrackTable;

	public TextAsset CostTable;
//	public TextAsset Common_CardTable;
	public TextAsset Common_RewardTable;
	public TextAsset Upgrade_Car_RatioTable;
	public TextAsset Upgrade_Crew_RatioTable;

	public TextAsset koTable;
	public TextAsset ModeChampionTable;
	public TextAsset AttendTable;
	public TextAsset AICarTable;

	public TextAsset AICrewTable;
//	public TextAsset WeeklyRewardTable;
	public TextAsset ItemTable;
	public TextAsset TeamTable;
	public TextAsset ClassTable;

	public TextAsset AchieveTable;
	public TextAsset MatTable;
	public TextAsset MixTable;
	public TextAsset LuckyTable;
	public TextAsset VIPTable;

	public TextAsset ClubAITable;
	public TextAsset UpCarTable;
	public TextAsset UpCrewTable;
	public TextAsset AIDelayTable;
	public TextAsset RatioTable;

	public TextAsset PackageTable;
	/*void InitializeTableStringFromTextAsset()
	{
		//Common_Car_StatusTableString = UserDataManager.instance._itemcreate.getTextAsset("Common_Car_Status").text;
		TextAsset tx =(TextAsset)AccountManager.instance.txtbundle.Load("Common_Car_Status", typeof(TextAsset));
		Common_Car_StatusTableString = tx.text;
		//Utility.Log(tx.text);
		//Common_Car_StatusTableString = Common_Car_StatusTable.text;
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("Common_Sponsor_Status", typeof(TextAsset));
		//	Common_Sponsor_StatusTableString = Common_Sponsor_StatusTable.text;
		Common_Sponsor_StatusTableString = tx.text;
		
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("Common_Crew_Status", typeof(TextAsset));
		//	Common_Crew_StatusTableString = Common_Crew_StatusTable.text;
		Common_Crew_StatusTableString = tx.text;
		
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("Exp_Range", typeof(TextAsset));
		//	ExpTableString = ExpTable.text;
		ExpTableString = tx.text;
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("Common_Track_Status", typeof(TextAsset));
		//	TrackTableString = TrackTable.text;
		TrackTableString = tx.text;
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("Cost_Table", typeof(TextAsset));
		//	CostTableString = CostTable.text;
		CostTableString = tx.text;
		
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("Common_Card", typeof(TextAsset));
		//	CardTableString = Common_CardTable.text;
		CardTableString = tx.text;
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("Common_Reward", typeof(TextAsset));
		//	RewardTableString = Common_RewardTable.text;
		RewardTableString = tx.text;
		
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("Upgrade_Car_Ratio", typeof(TextAsset));
		//	Upgrade_Car_RatioTableString = Upgrade_Car_RatioTable.text;
		Upgrade_Car_RatioTableString = tx.text;
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("Upgrade_Crew_Ratio", typeof(TextAsset));
		//	Upgrade_Crew_RatioTableString = Upgrade_Crew_RatioTable.text;
		Upgrade_Crew_RatioTableString = tx.text;
		
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("EN", typeof(TextAsset));
		//enTableString = enTable.text;
		enTableString = tx.text;
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("KO", typeof(TextAsset));
		//koTableString = koTable.text;
		koTableString = tx.text;
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("Mode_Championship", typeof(TextAsset));
		//Common_Mode_ChampionString = ModeChampionTable.text;
		Common_Mode_ChampionString = tx.text;
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("Common_Attend", typeof(TextAsset));
		//AttendTableString = AttendTable.text;
		AttendTableString = tx.text;
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("Mode_AI_Car", typeof(TextAsset));
		//AICarTableString = AICarTable.text;
		AICarTableString = tx.text;
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("Mode_AI_Crew", typeof(TextAsset));
		//AICrewTableString = AICrewTable.text;
		AICrewTableString = tx.text;
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("WeeklyReward", typeof(TextAsset));
		WeeklyRewardString = tx.text;
		
		tx =(TextAsset)AccountManager.instance.txtbundle.Load("Common_Items", typeof(TextAsset));
		if(tx != null) 
			ItemsString = tx.text;
		
		tx = null;
	}*/
	public TextAsset ClubAIExpTable;
	public TextAsset ClubRewardTable;

	public TextAsset[] LanuguageTables;

	void InitializeStorageLink()
	{
		// 다른 테이블이 로컬라이제이션 테이블을 참조하므로, 로컬라이제이션 테이블을 제일 먼저 초기화해야 한다.

		//enStorageLink.SetDataFile(enTableString);
		TextAsset tx = null;
		string CountryString = string.Empty;
		string gCountry =  EncryptedPlayerPrefs.GetString("CountryCode");
		switch(gCountry){
		case "KOR":
			tx =(TextAsset)AccountManager.instance.txtbundle.Load("KO", typeof(TextAsset));break;
			//CountryString =koTableString;break;
		case "JPN":
			tx =(TextAsset)AccountManager.instance.txtbundle.Load("JP", typeof(TextAsset));break;
			//	CountryString = LanuguageTables[0].text;break;
		case "CHN":
			tx =(TextAsset)AccountManager.instance.txtbundle.Load("CN", typeof(TextAsset));break;
			//CountryString = LanuguageTables[1].text;break;
		case "FRA":
			tx =(TextAsset)AccountManager.instance.txtbundle.Load("FR", typeof(TextAsset));break;
			//CountryString=LanuguageTables[2].text;break;
	//	case "TUR":
	//		tx =(TextAsset)AccountManager.instance.txtbundle.Load("TR", typeof(TextAsset));break;
			//CountryString= LanuguageTables[3].text;break;
		case "RUS":
			tx =(TextAsset)AccountManager.instance.txtbundle.Load("RU", typeof(TextAsset));break;
			//	CountryString= LanuguageTables[4].text;break;
		case "USA":
			tx =(TextAsset)AccountManager.instance.txtbundle.Load("EN", typeof(TextAsset));break;
			//CountryString = LanuguageTables[5].text;break;
		case "GBR":
			tx =(TextAsset)AccountManager.instance.txtbundle.Load("EN", typeof(TextAsset));break;
			//CountryString=LanuguageTables[5].text;break;
		case "DEU":
			tx =(TextAsset)AccountManager.instance.txtbundle.Load("DE", typeof(TextAsset));break;
			//CountryString = LanuguageTables[7].text;break;
		case "ITA":
			tx =(TextAsset)AccountManager.instance.txtbundle.Load("IT", typeof(TextAsset));break;
			//CountryString =LanuguageTables[8].text;break;
		case "PRT":
			tx =(TextAsset)AccountManager.instance.txtbundle.Load("PT", typeof(TextAsset));break;
			//CountryString =LanuguageTables[9].text;break;
		case "ESP":
			tx =(TextAsset)AccountManager.instance.txtbundle.Load("ES", typeof(TextAsset));break;
			//CountryString =LanuguageTables[10].text;break;
		case "IDN":
			tx =(TextAsset)AccountManager.instance.txtbundle.Load("ID", typeof(TextAsset));break;
		case "MYS":
			tx =(TextAsset)AccountManager.instance.txtbundle.Load("EN", typeof(TextAsset));break;
		default: {
			tx =(TextAsset)AccountManager.instance.txtbundle.Load("EN", typeof(TextAsset));break;
			//CountryString = LanuguageTables[5].text;break;
		}
			LanuguageTables = null;
		}
		CountryString = tx.text;
		koStorageLink.SetDataFile(CountryString);
		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("03_01_car", typeof(TextAsset));
		Common_Car_Link.SetDataFile(Common_Car_StatusTableString); 

	
		Common_Crew_Link.SetDataFile(Common_Crew_StatusTableString); 

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("09_13_sponsor", typeof(TextAsset));
		Common_Sponsor_Link.SetDataFile(Common_Sponsor_StatusTableString); 

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("01_exp", typeof(TextAsset));
		Common_Exp_Link.SetDataFile(ExpTableString);

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("07_track", typeof(TextAsset));
		Common_Track_Link.SetDataFile(TrackTableString);

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("09_item_cost", typeof(TextAsset));
		Common_Cost_Link.SetDataFile(CostTableString);

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("24_race_champ", typeof(TextAsset));
		Common_Mode_Champion_Link.SetDataFile(Common_Mode_ChampionString);
		//Common_Card_Link.SetDataFile(CardTableString);

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("15_reward_race", typeof(TextAsset));

		Common_Reward_Link.SetDataFile(RewardTableString);

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("15_reward", typeof(TextAsset));
		Common_Attend_Link.SetDataFile(AttendTableString);

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("03_car_ai", typeof(TextAsset));
		modeaicarLink.SetDataFile(AICarTableString);

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("01_crew_ai", typeof(TextAsset));
		modeaicrewLink.SetDataFile(AICrewTableString);

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("05_car_up", typeof(TextAsset));
		Upgrade_Car_Link.SetDataFile(Upgrade_Car_RatioTableString);

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("02_crew_up", typeof(TextAsset));
		Upgrade_Crew_Link.SetDataFile(Upgrade_Crew_RatioTableString);

	//	Common_WeeklyRewardLink.SetDataFile(WeeklyRewardString);
	//	tx =(TextAsset)AccountManager.instance.txtbundle.Load("01_1_team", typeof(TextAsset));
		Common_TeamLink.SetDataFile(TeamString);
	//	if(ItemsString != null) {
	//		tx =(TextAsset)AccountManager.instance.txtbundle.Load("04_1_car_item", typeof(TextAsset));
			Common_ItemsLink.SetDataFile(ItemsString);
	//	}

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("03_car_class", typeof(TextAsset));
		Common_ClassLink.SetDataFile(ClassString);

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("16_achieve", typeof(TextAsset));
		Common_ALink.SetDataFile(AchieveString);
		Common_ALink.SetDataFileDaily();
		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("06_material", typeof(TextAsset));
		Common_matLink.SetDataFile(MatString);
		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("06_mix", typeof(TextAsset));
		Common_MixLink.SetDataFile(MixString);
		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("05_luckybox", typeof(TextAsset));
		Common_LuckyLink.SetDataFile(LuckyString);
		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("01_exp_vip", typeof(TextAsset));
		vipLink.SetDataFile(VIPString);

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("24_race_club", typeof(TextAsset));
		clubaiLink.SetDataFile(ClubAIString);
		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("05_car_upgrade", typeof(TextAsset));
		carcostLink.SetDataFile(UpCarString);
		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("02_crew_upgrade", typeof(TextAsset));
		crewcostLink.SetDataFile(UpCrewString);
		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("07_AI_control", typeof(TextAsset));
		modeaiLink.SetDataFile(AIDelayString);

		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("15_reward_ratio", typeof(TextAsset));
		RatioLink.SetDataFile(RatioString);
		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("09_item_package", typeof(TextAsset));
		Common_Cost_Link.SetDataFilePackage(PackageString);
		//tx =(TextAsset)AccountManager.instance.txtbundle.Load("01_exp_club", typeof(TextAsset));
		Common_Exp_Link.SetClubDataFile(ClubAIExpString);
		if(GV.ChSeasonID >= 6010){
			//tx =(TextAsset)AccountManager.instance.txtbundle.Load("15_reward_club", typeof(TextAsset));
			Common_Reward_Link.SetClubDataFile(ClubRewardString);
		}
	}
	
	void ClearTable()
	{
		  Common_Car_StatusTable = null;
		  Common_Crew_StatusTable=null;
		  Common_Sponsor_StatusTable= null;
		Upgrade_Car_RatioTable= null;
		Upgrade_Crew_RatioTable= null;
		//enTable= null;
		koTable= null;
		ExpTable= null;
		TrackTable= null;
		CostTable= null;
	//	enStorageLink= null;
	//	koStorageLink= null;
		ModeChampionTable= null;
	//	Common_CardTable= null;
		Common_RewardTable = null;
		AttendTable = null;
		AICarTable = null;
		AICrewTable = null;
		TeamTable = null;ItemTable = null;
	//	WeeklyRewardTable = null;
		MatTable = null;
		ClassTable = null;
		AchieveTable = null;
		MixTable = null;
		LuckyTable = null;
		ClubAITable = null; VIPTable = null;
		RatioTable = null;
		PackageTable = null;
		ClubAIExpTable = null;
		ClubRewardTable = null;
		UpCrewTable = null;
		UpCarTable = null;
		AIDelayTable = null;
		System.GC.Collect();
	}







}