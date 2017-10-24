using UnityEngine;
using System.Collections;

public class Test_GetPlayerHere : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {

		string debugMessage = "X:%x% | Y:%y% | Z:%z%";
		debugMessage = debugMessage.Replace("%x%", transform.position.x.ToString());
		debugMessage = debugMessage.Replace("%y%", transform.position.y.ToString());
		debugMessage = debugMessage.Replace("%z%", transform.position.z.ToString());
		Debug.Log ("Plattform Position: " + debugMessage);

		player.transform.position = transform.position;

		debugMessage = "X:%x% | Y:%y% | Z:%z%";
		debugMessage = debugMessage.Replace("%x%", player.transform.position.x.ToString());
		debugMessage = debugMessage.Replace("%y%", player.transform.position.y.ToString());
		debugMessage = debugMessage.Replace("%z%", player.transform.position.z.ToString());
		Debug.Log ("Player Position:    " + debugMessage);
	}
	
}
