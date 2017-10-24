using UnityEngine;
using System.Collections;

public class WolkenbuddyBehaviour : MonoBehaviour {

	public float relativePositionOnScreenInX = -100.0f;
	public float relativePositionOnScreenInY = -10.0f;

	public int currentState = 0;

	private GameObject buddy;
	private GameObject player;
	private Animator anim;

	private float curLevelTime;
	private float changeStateF;
	private int changeStateI;
	private int changeToState;

	// Use this for initialization
	void Start () {
		buddy = GameObject.Find ("WolkenbuddyOnCamera");
		anim = buddy.GetComponent<Animator>();
		player = GameObject.Find ("Player");

		float curPosInX = buddy.transform.position.x;
		float curPosInY = buddy.transform.position.y;
		Vector3 targetBuddyPosition = new Vector3( curPosInX - relativePositionOnScreenInX, curPosInY - relativePositionOnScreenInY , -4.0f);
		buddy.transform.position = targetBuddyPosition;
	}

	// Aktualisierung nicht zu haeufig notwendig
	void FixedUpdate(){
		// 1.5 Schritten

		// MinimalWert
		if ( curLevelTime <= 3.0f ) 
		{ 
			// Minimale Animation abspielen
			changeToState = 10;
		}
		// erst ab 18 Sekunden abwärts eine änderung vollführen
		else if ( curLevelTime <= 18.0f ){
			// Entscheidungsvariable als FLOAT
			changeStateF = curLevelTime / 1.5f;
			/* 
				18.0: 	12		-> 1
				16.5:	11		-> 2
				15.0:	10		-> 3
				...
				3.0:	02		-> 
				1.5:	01
			 */
			// Entscheidungsvariable als INT fuer SWITCH Anweisung
			changeStateI = (int) changeStateF;
			// Werte die aktuelle Zeit aus
			switch(changeStateI){
			case 12: 	changeToState = 1;	break;
			case 11: 	changeToState = 2;	break;
			case 10: 	changeToState = 3;	break;
			case 9: 	changeToState = 4;	break;
			case 8: 	changeToState = 5;	break;
			case 7: 	changeToState = 6;	break;
			case 6: 	changeToState = 7;	break;
			case 5: 	changeToState = 8;	break;
			case 4: 	changeToState = 9;	break;
			// Ansonsten
			default: 	changeToState = 9; break;
			}
		}else{
			// Ansonsten maximale Sichtbarkeit gewaehren
			changeToState = 0;
		}
		// Setze Animation
		anim.SetInteger("buddyState", changeToState);
	}

	// Lese aktuelle Level Zeit aus
	void Update() {
		curLevelTime = player.GetComponent<PlayerLevelController>().levelTimer;
	}

}
