#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System.Collections;

public class CollectBigLight : MonoBehaviour {

	void OnTriggerEnter2D( Collider2D col ){
		Debug.Log ("OnTriggerEnter2D:   " +col.gameObject.name);
	}

	void OnCollisionStay2D( Collision2D col ){
		Debug.Log ("OnCollisionStay2D:  " +col.gameObject.name);
	}

	// Use this for initialization
	void OnCollisionEnter2D( Collision2D col ){
		Debug.Log ("OnCollisionEnter2D: "+col.gameObject.name);

		giveStats();

		Destroy(this.gameObject);
	}

	private void giveStats(){

		int addToScore = 0;
		float addToTime = 0.0f;
		
		if (this.tag == "LightSmall") {
			addToScore = 15;
			addToTime = 2.0f;
		}
		
		if (this.tag == "LightBig") {
			addToScore = 100;
			addToTime = 3.0f;
		}
		
		GameObject player = GameObject.Find ("Player");
		player.GetComponent<PlayerLevelController>().CurrentScore += addToScore;
		player.GetComponent<PlayerLevelController>().levelTimer += addToTime;

	}
}
