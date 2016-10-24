using UnityEngine;
using System.Collections;


[System.Serializable]
public class CameraTimeDelay{
	public float t_R1finish=2.0f;
	public float t_PitStart=2.0f;
	public float t_PitInPlay01 = 3.0f;
	public float t_R2Finish01 = 1.5f;
	public float t_R2Result01 = 3.0f;
	public float t_R2Result02 = 3.0f;
	public float t_R2Result03 = 3.0f;
}

/*

[System.Serializable]
public struct pit_delay {
	
	public pit_delay(int t){
		pit_car_enter = 2.0f;
		pit_crew_run = 3.0f;pit_car_RightUp = 1.5f;
		pit_car_RightDown = 1.5f;pit_car_LeftUp = 1.5f;
		pit_car_Race2Start = 1.5f;pit_car_LeftDown = 1.5f;
		this.t =t;
		pit_delay_total = 0.0f;
	}
	
	public float pit_car_enter;
	public float pit_crew_run ;
	public float pit_car_RightUp ;
	public float pit_car_RightDown ;
	public float pit_car_LeftUp ;
	public float pit_car_LeftDown ;
	public float pit_car_Race2Start;
	public float pit_delay_total;
	private float t;
	public float Pit_Delay_Total(){
		pit_delay_total =pit_crew_run+pit_car_enter + pit_car_Race2Start
			+pit_car_RightDown +pit_car_RightUp+pit_car_LeftUp+pit_car_LeftDown;
		return pit_delay_total;
	}

}

*/
