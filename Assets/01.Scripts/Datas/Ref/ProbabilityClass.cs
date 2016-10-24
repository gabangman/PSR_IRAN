using UnityEngine;
using System.Collections;

public class ProbabilityClass{


	public ProbabilityClass(){
	
	}
	public int DealerCarClass(){
		uint idx = 0;
		idx =Well512.Next(0,10000);
		//	int pro = rewardProb.MatchPro_D*100;
		if(idx < 0)	return 3101;
		else if(idx < 500)return 3102;
		else if(idx < 3500) return 3103;
		else if(idx < 8900) return 3104;
		else if(idx < 9900) return 3105;
		else if(idx < 10000) return 3106;
		else return 3101;
	}

	public int LuckyBoxCarProbability(int mSeason, int boxColor){
		uint idx = 0;
		int reID = 0;
		for(int i=0; i < 1000; i++){
			idx =Well512.Next(0,24);
			idx += 1000;
			int carID = (int)idx;
			if(Common_Car_Status.Get(carID).ReqLV <= mSeason){
				reID = carID;
				break;
			}
		}

		if(reID >= 1024 || reID <= 1000) return 1000; 
		return reID;
	}

	public int LuckyBoxClassProbability(int mSeason, int boxColor){
		uint idx = 0;
		int reID = 0;
		idx =Well512.Next(0,10000);
		if(boxColor == 0){
			//silver
			if(idx < 3500){ reID = 0;
			}else if( idx < 6600){ reID = 1;
			}else if(idx < 9400){ reID = 2;
			}else if(idx < 9700){ reID = 3;
			}else if(idx < 9900){ reID = 4;
			}else if(idx < 10000){reID = 5;
			}
		}else{
			//Gold
			if(idx < 3500){ reID = 2;
			}else if( idx < 8500){ reID = 3;
			}else if(idx < 9500){ reID = 4;
			}else if(idx < 10000){ reID = 5;
			}
		}
		return reID;
	}

	public int RegularRaceClass(Common_Reward.Item rewardProb){
		uint idx = 0;
		int reID = 0;
		idx =Well512.Next(0,10000);
		//	int pro = rewardProb.MatchPro_D*100;
		if(idx < rewardProb.MatchPro_D*100)	return 3101;
		else if(idx < rewardProb.MatchPro_D*100+rewardProb.MatchPro_C*100)return 3102;
		else if(idx < rewardProb.MatchPro_D*100+rewardProb.MatchPro_C*100+rewardProb.MatchPro_B*100) return 3103;
		else if(idx < rewardProb.MatchPro_D*100+rewardProb.MatchPro_C*100+rewardProb.MatchPro_B*100+ rewardProb.MatchPro_A*100) return 3104;
		else if(idx < rewardProb.MatchPro_D*100+rewardProb.MatchPro_C*100+rewardProb.MatchPro_B*100+ rewardProb.MatchPro_A*100+rewardProb.MatchPro_S*100) return 3105;
		else if(idx <rewardProb.MatchPro_D*100+rewardProb.MatchPro_C*100+rewardProb.MatchPro_B*100+ rewardProb.MatchPro_A*100+rewardProb.MatchPro_S*100+ rewardProb.MatchPro_SS*100) return 3106;
		else return 3101;
	}
	public int EvoProbability(int percentage){
		if(percentage >= 9) return 1;
		return CaculateEvoRange(percentage);
	}

	protected int[] CreateRandomValue(int count){
		int[] arr = new int[count];
		for(int i = 0; i < arr.Length; i++){
			arr[i] = i;
		}
		
		for(int i = 0; i < arr.Length; i++){
			int n = Random.Range(0, count);
			int temp = arr[i];
			arr[i] = arr[n];
			arr[n] = temp;
		}
		return arr;
	}

	private int CaculateEvoRange(int mNum){
		uint mR =Well512.Next(0,10000);
		int mCnt = 0;
		if(mR > mNum*1000) mCnt = 0;
		else if(mR <= mNum*1000) mCnt = 1; 

		return mCnt;
	}
}


	public class Well512
	{
		static uint[] state = new uint[16];
		static uint index = 0;
		
		static Well512()
		{
		System.Random random = new System.Random((int)System.DateTime.Now.Ticks);	
			for (int i = 0; i < 16; i++)
			{
				state[i] = (uint) random.Next();
			}
		}
		
		internal static uint Next(int minValue, int maxValue)
		{
			return (uint)((Next() % (maxValue - minValue)) + minValue);
		}
		
		public static uint Next(uint maxValue)
		{
			return Next() % maxValue;
		}
		
		public static uint Next()
		{
			uint a, b, c, d;
			
			a = state[index];
			c = state[(index + 13) & 15];
			b = a ^ c ^ (a << 16) ^ (c << 15);
			c = state[(index + 9) & 15];
			c ^= (c >> 11);
			a = state[index] = b ^ c;
			d = a ^ ((a << 5) & 0xda442d24U);
			index = (index + 15) & 15;
			a = state[index];
			state[index] = a ^ b ^ d ^ (a << 2) ^ (b << 18) ^ (c << 28);
			
			return state[index];
		}
	}

