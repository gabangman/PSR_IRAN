using UnityEngine;

[AddComponentMenu("NGUI/Examples/Spin With Mouse")]
public class SpinWithMouse : MonoBehaviour
{
	public Transform target;
	public float speed = 1f;
	
	Transform mTrans;
	
	void Start ()
	{
		mTrans = transform;
	}
	
	void OnDrag (Vector2 delta)
	{
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
		
		if (target != null)
		{
			target.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * target.localRotation;
		}
		else
		{
			mTrans.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * mTrans.localRotation;
		}
	}
}

/*
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Spin With Mouse")]
public class SpinWithMouse : MonoBehaviour
{
	public Transform target;
	public Transform ShopTarget, TeamTarget;
	public float speed = 1f;
	public float maxValue, minValue;
	Transform mTrans;

	void Awake(){
		//Debug.Log("awake");
	}

	void OnEnable(){

	}

	void Start ()
	{
		mTrans = TeamTarget.transform;
	}


	void ChangeTarget(bool b){
		if(b) mTrans = TeamTarget.transform;
		else mTrans = ShopTarget.transform;
	//	firstAngle = mTrans.localEulerAngles;
	}

	void OnPress (bool isPressed){
	
	}

	void OnDrag (Vector2 delta)
	{
	
		if(delta.x > 30 || delta.x < -30) return;
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
		float rotateValue = mTrans.localEulerAngles.y;
		if(rotateValue > 180) rotateValue -=360;

		if(rotateValue > maxValue){
			mTrans.localEulerAngles = new Vector3(0, maxValue, 0);
			//mTrans.localRotation = Quaternion.Euler(0f, -0.5f * -0.1f * speed, 0f) * mTrans.localRotation;
			//return;
		}else if(rotateValue < minValue){
			mTrans.localEulerAngles = new Vector3(0, minValue, 0);
			//mTrans.localRotation = Quaternion.Euler(0f, -0.5f * +0.1f * speed, 0f) * mTrans.localRotation;
			//return;
		}
		mTrans.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * mTrans.localRotation;
		//Quaternion rotationY=Quaternion.Euler(0, rotateValue, 0)*Quaternion.Euler(0, orbitSpeedY, 0);
		//mTrans.rotation=rotationY;
		//orbitSpeedY*=(1-Time.deltaTime*3);
		//Debug.Log(" min " + mTrans.localEulerAngles+ " val " + rotateValue);
		return;
		/*
		if (target != null)
		{
			float rotateValue = mTrans.localEulerAngles.y;
			rotateValue  = Mathf.Clamp(rotateValue,10,200);
			if(rotateValue >= maxValue) {
				mTrans.localEulerAngles = maxRot;
				Debug.Log(" max " + mTrans.localEulerAngles + " val " + rotateValue);
				return;
			}
			if(rotateValue <= minValue) {
				mTrans.localEulerAngles = minRot;
				Debug.Log(" min " + mTrans.localEulerAngles+ " val " + rotateValue);
				return;
			}
			target.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * target.localRotation;
			//Debug.Log(target.localEulerAngles);
		}
		else
		{
			//mTrans.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * mTrans.localRotation;
		}*/
	//}
//}
