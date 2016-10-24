using UnityEngine;
using System.Collections;
using System.Data;

//NOTE: 	No need to clear the results as it's either overwritten in Sync Mode or Cleared just before 
//	        overwriting it in Async Mode. If you want to clear it manually, use the following code:
//
//			results.Clear();
//			results.Value = new DictionaryEntry("",null);
//
//NOTE: Single parameter (tree) can be build of the Root value. Code example:
//
//			SoapTree parameters = new SoapTree(new DictionaryEntry("ZipCode","20500"));
//
//NOTE: A Single rooted tree is build like: 
//
//			SoapTree parameters = new SoapTree(new DictionaryEntry("parameters",null));
//		
//			parameters.Children.Add(new DictionaryEntry("Query", "Opensock"));
//			parameters.Children.Add(new DictionaryEntry("AppId",BingAppId));
//				//NOTE: Key, null signals wrapper tag.
//				SoapTreeNode sources = parameters.Children.Add(new DictionaryEntry("Sources",null));
//				sources.Children.Add(new DictionaryEntry("SourceType","Web"));
//			parameters.Children.Add(new DictionaryEntry("Image", ""));
//			
//NOTE: Emulate MultiRoot Tree by using an empty Key value. Code:
//
//			SoapTree parameters = new SoapTree(new DictionaryEntry("",null));
//
//			parameters.Children.Add(new DictionaryEntry("servername","whois.tucows.com"));
//			parameters.Children.Add(new DictionaryEntry("port","43"));
//			parameters.Children.Add(new DictionaryEntry("domain","www.ecubicle.net"));
//

/// <summary>
/// Test script for the SoapObject 
///
/// Author: 	G.W. van der vegt
/// Version: 	1.0
/// Date:		17-06-2009
///
/// SoapObject uses SimpleTree from: 
/// 	This collection of non-binary tree data structures created by Dan Vanderboom.
/// 	Critical Development blog: http://dvanderboom.wordpress.com
/// 	Original Tree<T> blog article: http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/
/// 	Linked-in: http://www.linkedin.com/profile?viewProfile=&key=13009616&trk=tab_pro
/// </summary>
public class webGUIScript : MonoBehaviour
{
	public void Awake()
	{
		//Placeholder
	}
	
	void Start()
	{
		mUser = new UserService();
		mResult = new EventResult();
		//StartCoroutine("CheckRoutine");
	}
	
	/// <summary>
	/// -
	/// </summary>		
	public void OnApplicationQuit()
	{
		//Placeholder
	}


	IEnumerator CheckRoutine(){
		for(;;){
			if(mResult.RequestMethod != null)
			{
				Utility.LogWarning(mResult.RequestMethod);
				yield break;
			}
			yield return new WaitForSeconds(0.15f);
		}

	}


	public EventResult mResult;
	public UserService mUser;
	IEnumerator checkroutinTest(){
		yield return null;
		for(;;){
			if(mResult.RequestMethod != null)
			{
				Utility.LogWarning(mResult.RequestMethod);
				yield break;
			}
			yield return new WaitForSeconds(0.15f);
			Utility.Log(mResult.ResultCode);
		}
	}

	public void OnGUI()
	{
		int y = 10;
		int h = 30;
		
		if (GUI.Button(new Rect(10, y, 150, h), "Resource Test"))
		{

			mResult = mUser.GetUserSession (1, "aabbccddefg");
			Utility.LogWarning(mResult.RequestTime);
			Utility.Log(mResult.ResultCode);
			//   soap.SoapUrl = "http://www.jasongaylord.com/webservices/zipcodes.asmx";
			//  soap.Details = "//diffgr:diffgram/NewDataSet/Details";
			// soap.Service = "ZipCodeToDetails";
			//   soap.ServiceNS = "http://www.jasongaylord.com/webservices/zipcodes";
			
			// SoapTree parameters = new SoapTree(new DictionaryEntry("ZipCode", "20500"));
			
			// results = soap.RequestSync(parameters, 10);
			/*
					C:\Program Files (x86)\Unity\Editor\Data\Mono\bin>wsdl -out:J:\Project_PitStop\t
					runk\Client_G\Assets\01.Scripts\webClient\PitInRacingResource.cs http://14.63.21
					7.156:6060/PitInRacing/Resource.asmx?wsdl */
			

		//	UserService user = new UserService();
		//	EventResult re =  user.GetUserSession (1, "aabbccddefg");
		//	StartCoroutine("checkroutinTest");
		
		//	return;
		//	Utility.LogWarning(re.RequestMethod);

		//	if(re.ResultCode == 0){
		//		Utility.LogWarning(re.ResultReference);
		//	}else{
		//		Utility.LogWarning(re.ResultCode);		
		//	}
		//
		//	return;

			PitInRacingResource pit = new PitInRacingResource();
		//	System.Data.DataSet set = pit.GetCrewInfoList();
		//		Utility.Log(set.ToString());
		//		Utility.Log((System.Data.DataSet)pit.GetSeasonInfoList());
		//	EventResult result =  pit.UpdatePlatformInfo(1000, "test", "test1");
			//Utility.Log(result.BattelEvent);
			System.Data.DataSet set = pit.GetCrewInfoList();
			
			Utility.Log(set.Tables.Count);
			Utility.Log(set.Tables[0].Columns.Count); // 가로
			Utility.Log(set.Tables[0].Rows.Count); // 세로 

			Utility.Log(set.GetXmlSchema());
			System.Xml.XmlDocument xdoc1 = new System.Xml.XmlDocument();
			xdoc1.LoadXml(set.GetXml());
			System.Xml.XmlNode node2 = xdoc1.DocumentElement;
			if(node2.HasChildNodes){
				Utility.Log("childnodes yes");
				System.Xml.XmlNodeList nodelist = node2.ChildNodes;
				for(int i = 0; i < nodelist.Count;i++){
					//Utility.Log(nodelist[i].InnerXml);
					System.Xml.XmlNode temp = nodelist[i];
					System.Xml.XmlNodeList nodelists = temp.ChildNodes;
					for(int j=0; j < nodelists.Count; j++)
					{
						//Utility.Log(nodelists[j].InnerXml);
						Utility.Log(nodelists[j].InnerText);
					//	Utility.Log(nodelists[j].LastChild.Name);
					//	Utility.Log(nodelists[j].LastChild.InnerXml);
					}
				}
			}else{
				Utility.Log("childnodes no");
			}
			return;/*
			System.Data.DataSet ds = new DataSet("myDataSet");
			System.Data.DataTable t = ds.Tables.Add("Items");
			t.Columns.Add("id", typeof(int));
			t.Columns.Add("Item", typeof(string));
			
			//string str  = System.Data.DataSet.
			
			string str = ds.GetXmlSchema();
			System.Data.DataSet customerDataSet = new DataSet("DataSetTest");
			customerDataSet.Tables.Add(new DataTable("Customers"));
			customerDataSet.Tables["Customers"].Columns.Add("Name", typeof(string));
			customerDataSet.Tables["Customers"].Columns.Add("CountryRegion", typeof(string));
			customerDataSet.Tables["Customers"].Rows.Add("Juan", "Spain");
			customerDataSet.Tables["Customers"].Rows.Add("Johann", "Germany");
			customerDataSet.Tables["Customers"].Rows.Add("John", "UK");
			str = customerDataSet.GetXmlSchema();
			Utility.Log(str);	
			str = customerDataSet.GetXml();
			Utility.Log(str);	
			System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
			xdoc.LoadXml(str);
			System.Xml.XmlNode node1 = xdoc.DocumentElement;
			if(node1.HasChildNodes){
				Utility.Log("childnodes yes");
				System.Xml.XmlNodeList nodelist = node1.ChildNodes;
				for(int i = 0; i < nodelist.Count;i++){
					Utility.Log(nodelist[i].InnerXml);
					System.Xml.XmlNode temp = nodelist[i];
					System.Xml.XmlNodeList nodelists = temp.ChildNodes;
					for(int j=0; j < nodelists.Count; j++)
					{
						Utility.Log(nodelists[j].InnerXml);
						Utility.Log(nodelists[j].InnerText);
						Utility.Log(nodelists[j].LastChild.Name);
						Utility.Log(nodelists[j].LastChild.InnerXml);
					}
				}
			}else{
				Utility.Log("childnodes no");
			}
			System.Xml.XmlNodeList nodelists1 = xdoc.SelectNodes("/DataSetTest");//
			foreach(System.Xml.XmlNode xn in nodelists1){
				Utility.Log(xn["Customers"]["Name"].InnerText);
			}
			
			//	System.Xml.XmlNodeList nodes=  xdoc.SelectNodes("Customers");
			//	Utility.Log(nodes.Count);
			//	foreach(System.Xml.XmlNode n in nodes){
			//		Utility.Log(n.SelectSingleNode("Name").InnerText);
			//	}
			//	System.Data.DataSet germanyCustomers = customerDataSet.Clone();
			//	DataRow[] copyRows = 
			//		customerDataSet.Tables["Customers"].Select("CountryRegion = 'Germany'");
			
			//	DataTable customerTable = germanyCustomers.Tables["Customers"];
			
			//	foreach (DataRow copyRow in copyRows)
			//		customerTable.ImportRow(copyRow);
			
			//	Utility.Log(ds.DataSetName);
			//		Utility.Log(ds.GetXml());
			//		Utility.Log(ds.Tables[0].Rows.Count);
			//		for(int i = 0; i < ds.Tables[0].Rows.Count;i++){
			//			Utility.Log(ds.Tables[0].Rows[i].ToString());
			//		}
			
			//	System.IO.MemoryStream streamXml = new System.IO.MemoryStream(System.Text.Encoding.Default.GetBytes(ds.GetXmlSchema()));
			//Utility.Log(ds.ReadXml(ds.GetXml));
			//		System.Data.DataTable newT = new DataTable();
			//		newT.ReadXmlSchema(set.GetXmlSchema());
			//		var datasetxml = new System.Data.DataSet();
			//		datasetxml.ReadXmlSchema(streamXml);
			//		foreach (DataTable dataTable in datasetxml.Tables)
			//			dataTable.EndLoadData();
			//Utility.Log(datasetxml.ReadXmlSchema(streamXml));
			//	foreach(DataColumn c in datasetxml.Tables.Columns){
			//		Utility.Log(string.Format("{0} : {1}", c.ColumnName, c.DataType.Name));
			//	}
			
		*/
			
		}
		y += h + 5;
		
		if (GUI.Button(new Rect(10, y, 150, h), "UserServiceTest"))
		{
		
		}
		y += h + 5;
		
		if (GUI.Button(new Rect(10, y, 150, h), "Other Test"))
		{

		}
		y += h + 5;
		
		//Please substitue your own Bing Developers Key here...

		if (GUI.Button(new Rect(10, y, 150, h), "Insert Test"))
		{
		
		}
		y += h + 5;
		
		Rect clientrect = new Rect();
		
		clientrect.x = 10;
		clientrect.y = y;
		clientrect.width = 400;
		clientrect.height = 20;
		y += 20 + 5;
		
		GUI.Label(clientrect, "webGUI");
		
		clientrect.x = 10;
		clientrect.y = y;
		clientrect.width = 400;
		clientrect.height = 20;
		y += 20 + 5;
		/*
		if (results != null)
		{
			GUI.Label(clientrect, results.ToString());
			//Utility.LogWarning(results.ToString());
			
		}*/
		
		clientrect.x = 10;
		clientrect.y = y;
		clientrect.width = 400;
		clientrect.height = 200;
		y += 50 + 5;
		/*
		if (results != null)
		{
			string msg = "";
			
		//	EnumParmTree(results, ref msg);
			
			GUI.Label(clientrect, msg);
			//Utility.LogWarning(msg);
		}*/
	}
}

