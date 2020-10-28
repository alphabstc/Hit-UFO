using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundControllor : MonoBehaviour {
	private IUserAction action;
	private float speed;
	private GameObject explosion;
	void Start(){
		action = GameDirector.GetInstance().CurrentSceneControllor as IUserAction;
		speed = 3f;
	}
	public void loadRoundData(int round)
	{
		
		switch (round)
		{
		case 1:    

			break;
		case 2:    
			
			speed = 2.5f;
			explosion = Instantiate (Resources.Load<GameObject> ("Prefabs/ParticleSystem2"), new Vector3(0, -100, 0), Quaternion.identity);
			action.setting (speed,explosion);
			break;
		case 3:
			
			speed = 2f;
			explosion = Instantiate (Resources.Load<GameObject> ("Prefabs/ParticleSystem3"), new Vector3(0, -100, 0), Quaternion.identity);
			action.setting (speed,explosion);
			break;
		}
	}
}
