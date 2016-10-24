using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClanManager  {

	static ClanManager _instance;
	public static ClanManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ClanManager();
			}
			return _instance;
		}
	}
	
	public void InstanceNull(){
		_instance = null;
	}

	public List<ClanList> listClanLocalRank = new List<ClanList>();
	public List<ClanList> listClanGlobalRank = new List<ClanList>();
	public List<ClanMemList> listMyClanMem = new List<ClanMemList>();
	public List<ClanList> listClanSearch =new List<ClanList>();
	public List<ClanMemList> listVisitClanMem = new List<ClanMemList>();

	
	[System.Serializable]
	public class ClanList{
		public string strClanName;
	
		public ClanList(){
		}
	}

	[System.Serializable]
	public class ClanMemList{
		public string strClanMemName;
		
		public ClanMemList(){
		}
	}
	
}
