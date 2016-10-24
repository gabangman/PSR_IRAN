using UnityEngine;
using System.Collections;

public class camerAni : MonoBehaviour {

	// Use this for initialization
	//public GameObject cameraposition;
	
//	private Transform[] position, path;
	void Start(){
	//	position = cameraposition.GetComponentsInChildren<Transform>();
		
	//	Utility.Log (position[1].name);
		
	//	foreach(Transform tr in position){
//			Utility.Log (tr.name + " " + position.Length 	);
	//	}
	}
/*	
	Transform searchPosition(string str){
		Transform trans = null;
		foreach(Transform tr in position){
			if(tr.name == str){
				trans = tr;
				break;
			}
		}
		
		return trans;
	}
	
	public void InitCamera(){
		//gameObject.transform = 
	//	Transform tr = searchPosition("cartocrew");
	//	gameObject.transform.position = tr.position;
	//	gameObject.transform.rotation = tr.rotation;
		//gameObject.GetComponent<CameraRotation>().enabled = true;
	}
	
	public void MakeMoveAnimation(string str){
	//	gameObject.GetComponent<CameraRotation>().enabled = false;
		Transform tr = searchPosition(str);
		AnimationCurve posX = AnimationCurve.EaseInOut(0, transform.position.x, 1.0f, tr.position.x);
		AnimationCurve posY = AnimationCurve.EaseInOut(0,transform.position.y, 1.0f, tr.position.y);
		AnimationCurve posZ = AnimationCurve.EaseInOut(0, transform.position.z, 1.0f,tr.position.z);
		AnimationCurve rotX = AnimationCurve.EaseInOut(0, transform.rotation.x, 1.0f, tr.rotation.x);
		AnimationCurve rotY = AnimationCurve.EaseInOut(0, transform.rotation.y, 1.0f, tr.rotation.y);
		AnimationCurve rotZ = AnimationCurve.EaseInOut(0, transform.rotation.z, 1.0f, tr.rotation.z);
		
		
		AnimationClip clip = new AnimationClip();
		
		clip.SetCurve("", typeof(Transform), "localPosition.x", posX);
		clip.SetCurve("", typeof(Transform), "localPosition.y", posY);
		clip.SetCurve("", typeof(Transform), "localPosition.z", posZ);
		clip.SetCurve("", typeof(Transform), "localRotation.x", rotX);
		clip.SetCurve("", typeof(Transform), "localRotation.y", rotY);
		clip.SetCurve("", typeof(Transform), "localRotation.z", rotZ);
		//AnimationEvent evt = new AnimationEvent ();
		//evt.time = 0.26f-0.03f*(i);
		//evt.functionName = "TargetAniEnd";
		//clip.AddEvent (evt);
		if(str == "MyTeam_Car"){
			AnimationEvent evt = new AnimationEvent ();
			evt.time = 1.0f;
			evt.functionName = "FirstPostion";
			clip.AddEvent (evt);
			
		}
		animation.AddClip(clip, "moving");
		animation.Play("moving");
		
	
	}
	
	void FirstPostion(){
		//gameObject.GetComponent<CameraRotation>().enabled = true;
		Utility.Log ("postion " + gameObject.transform.position);
	}
	
	
	public void MakeMoveAnimationTest(string str){
		gameObject.GetComponent<CameraRotation>().enabled = false;
		//Transform tr = searchPosition(str);
		//var t = cameraposition.transform.GetChild(0).gameObject;
		//Transform[] tr = gameObject.GetComponentsInChildren<Transform>(true);
		AnimationCurve px = new AnimationCurve();
		AnimationCurve py = new AnimationCurve();
		AnimationCurve pz = new AnimationCurve();
		
		AnimationCurve rx = new AnimationCurve();
		AnimationCurve ry = new AnimationCurve();
		AnimationCurve rz = new AnimationCurve();
		AnimationCurve rw = new AnimationCurve();
		int i = 0;
		px.AddKey(new Keyframe(0.2f*i, gameObject.transform.position.x));
		py.AddKey(new Keyframe(0.2f*i, gameObject.transform.position.y));
		pz.AddKey(new Keyframe(0.2f*i, gameObject.transform.position.z));
		
		rx.AddKey(new Keyframe(0.2f*i, gameObject.transform.rotation.x));
		ry.AddKey(new Keyframe(0.2f*i, gameObject.transform.rotation.y));
		rz.AddKey(new Keyframe(0.2f*i, gameObject.transform.rotation.z));
		rw.AddKey(new Keyframe(0.2f*i, gameObject.transform.rotation.w));
		i++;
		foreach(Transform _tr in position){
			px.AddKey(new Keyframe(0.2f*i, _tr.transform.position.x));
			py.AddKey(new Keyframe(0.2f*i, _tr.transform.position.y));
			pz.AddKey(new Keyframe(0.2f*i, _tr.transform.position.z));
			
			rx.AddKey(new Keyframe(0.2f*i, _tr.transform.rotation.x));
			ry.AddKey(new Keyframe(0.2f*i, _tr.transform.rotation.y));
			rz.AddKey(new Keyframe(0.2f*i, _tr.transform.rotation.z));
			rw.AddKey(new Keyframe(0.2f*i, _tr.transform.rotation.w));
			
			
			
			
			i++;
		
		}
		
		AnimationClip clip1 = new AnimationClip();
		clip1.SetCurve("", typeof(Transform), "localPosition.x", px);
		clip1.SetCurve("", typeof(Transform), "localPosition.y", py);
		clip1.SetCurve("", typeof(Transform), "localPosition.z", pz);
		
		clip1.SetCurve("", typeof(Transform), "localRotation.x", rx);
		clip1.SetCurve("", typeof(Transform), "localRotation.y", ry);
		clip1.SetCurve("", typeof(Transform), "localRotation.z", rz);
		clip1.SetCurve("", typeof(Transform), "localRotation.w", rw);
		
		animation.AddClip(clip1, "moving1");
		animation.Play("moving1");
		return;
	
		
		
	}

	public void testmove(){
		Transform tr = searchPosition("MyTeam_Car");

			iTween.MoveTo ( gameObject, iTween.Hash( "path", iTweenPath.GetPath("testpath") 
			                                        , "time", 2.0f
			                                        , "easetype", iTween.EaseType.linear
		                                        	, "looktarget", tr.gameObject
			                                        , "looktime", 0.1f
			                                        ));
			
			

	
	}

	
	
	
	//public Transform[] waypointArray;
//	float percentsPerSecond = 0.2f; // %2 of the path moved per second
//	float currentPathPercent = 0.0f; //min 0, max 1
	
	void Update () 
	{
		//currentPathPercent += percentsPerSecond * Time.deltaTime;
		//iTween.PutOnPath(gameObject, position, currentPathPercent);
	}
	

	
	void OnDrawGizmos () {
		// make a new array of waypoints, then set it to all of the transforms in the current object
		//var waypoints = gameObject.GetComponentsInChildren( Transform );
	//	path = cameraposition.GetComponentsInChildren<Transform>();
		// now loop through all of them and draw gizmos for each of them
	//	foreach ( Transform waypoint  in path ) {
		//	Gizmos.DrawSphere( waypoint.position, 15.0f );
	//	}
		
	}

*/


}
