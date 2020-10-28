using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneControllor{
	void LoadResources ();
}

public interface IUserAction{
	void Hit (Vector3 pos);
	void Restart ();
	int GetScore();
	int GetRound ();
	void GameOver ();
	bool isCounting();
	int getEmitTime ();
	void setting(float speed,GameObject explosion);
}