using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using SimpleJSON;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;

[System.Serializable]
public class ClubSponsor {
	public List<ClubSponDetail> listClub;

	[System.Serializable]
	public class ClubSponDetail{
		public int sponTeamId;
		public int sponId;
		public long SponRemainTime;
		public int freeSpon;
		public System.DateTime sponDateTime;
	}

	public ClubSponsor(){
	
	}

	public void Set(){
		listClub = new List<ClubSponDetail>();
	}
	public void setTeamSponCompare(){
		if(listClub.Count ==0) return;
		ClubSponDetail md;
		int teamCnt = GV.listmyteaminfo.Count;	
		for(int i = 0; i < teamCnt; i++){
				if(GV.listmyteaminfo[i].freeSpon != 2){
				int teamId = GV.listmyteaminfo[i].TeamCode;
				md = listClub.Find(obj=> obj.sponTeamId == teamId);
				if(md == null){
					
				}else{
					if(md.sponId != 1300){
						System.DateTime nowTime = NetworkManager.instance.GetCurrentDeviceTime();
						System.TimeSpan pTime = new System.DateTime(md.SponRemainTime) - nowTime;
						if(pTime.TotalHours >0){
							GV.listmyteaminfo[i].SponID = md.sponId;
							GV.listmyteaminfo[i].sponDateTime = md.sponDateTime;
							GV.listmyteaminfo[i].SponRemainTime = md.SponRemainTime;
							GV.listmyteaminfo[i].freeSpon = 1;
						}else{
							GV.listmyteaminfo[i].SponID = 1300;
							GV.listmyteaminfo[i].freeSpon = 0;
						}	  
					}
				}
			}
		
		}
	
	}
	public void SetTeamSpon(int sponId, int teamId, long sponTime, System.DateTime dateTime){
		//ClubSponDetail mD = new ClubSponDetail();
		ClubSponDetail md =	listClub.Find(obj=> obj.sponTeamId == teamId);
		if(md == null) {
			md = new ClubSponDetail();
			md.sponId = sponId;
			md.sponTeamId = teamId;
			md.SponRemainTime = sponTime;
			md.freeSpon = 1;
			md.sponDateTime = dateTime;
			listClub.Add(md);
		}else{
			md.sponId = sponId;
			md.sponTeamId = teamId;
			md.SponRemainTime = sponTime;
			md.sponDateTime = dateTime;
			md.freeSpon = 1;
		}
	}
	public void unSetTeamSpon(int teamId){
		ClubSponDetail md = listClub.Find(obj=> obj.sponTeamId == teamId);
		if(md != null) md.sponId = 1300;
	}
}

public class ClubSponInfo{
	public static ClubSponInfo instance;// = new myAccount();
	public ClubSponsor mClubSpon = new ClubSponsor();
	public ClubSponInfo(){
	
	}
	public void GetSponInfo(){
		if(!EncryptedPlayerPrefs.HasKey("clubSpon")){
			instance.mClubSpon.Set();
			var b = new BinaryFormatter();
			var m = new MemoryStream();
			b.Serialize(m, instance.mClubSpon);
			EncryptedPlayerPrefs.SetString("clubSpon", 
			                               Convert.ToBase64String(m.GetBuffer()) );
		}
		
		var data = EncryptedPlayerPrefs.GetString("clubSpon");
		if(!string.IsNullOrEmpty(data))
		{
			var b = new BinaryFormatter();
			var m = new MemoryStream(Convert.FromBase64String(data));
			instance.mClubSpon = (ClubSponsor)b.Deserialize(m);
		}
	}
	
	public void SaveSponInfo()
	{
		if(instance == null) return;
		var b = new BinaryFormatter();
		var m = new MemoryStream();
		b.Serialize(m, instance.mClubSpon);
		EncryptedPlayerPrefs.SetString("clubSpon", Convert.ToBase64String(m.GetBuffer()));
	}
}

