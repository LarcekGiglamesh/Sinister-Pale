#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System.Collections;

public class PassablePlattform : MonoBehaviour {

	private float playerSizeInX = 0.0f;									// Breite des Spielers
	private float playerSizeInY = 0.0f;									// Hoehe des Spielers

	// Plattform ist durchschreitbar, dient als Schalter
	private bool isPassable = false;
	// Definiere 2 Rechtecke ueber der aktuellen Position und der Groesse des Protagonisten
	// Befindet sich der Charakter ausgehend der Linken unteren Ecke in Rechteck 1 		-> Plattform begehbar
	// Befindet sich der Charakter ausgehend der Rechten unteren Ecke in Rechteck 2 	-> Plattform begehbar
	// Charakter weder in Rechteck 1 noch Rechteck 2									-> Plattform durchlaessig
	private float passableMin_X1;										// Rechteck 1 untere linke Position in X
	private float passableMax_X1;										// Rechteck 1 untere rechte Position in X
	private float passableMin_X2;										// Rechteck 2 untere linke Position in X
	private float passableMax_X2;										// Rechteck 2 untere rechte Position in X
	private float passableMin_Y1Y2;										// Rechteck 1+2, untere Position in Y
	private float passableMax_Y1Y2;										// Rechteck 1+2, obere Position in Y
	private GameObject player;											// Spieler Objekt
	private float plattformWidth;										// Breite der Plattform
	private float plattformHeight;										// Hoehe der Plattform

	// private int debugCounter = 0;										// Debug Update Counter

	void Awake(){

		// float curPosX = transform.position.x;			// Bestimme die Startposition der Plattform in X
		// float curPosY = transform.position.y;			// Bestimme die Startposition der Plattform in Y

		// Finde das GameObject des Spielers
		player = GameObject.Find ("Player");

		// Bestimme die Groesse des Spielers
		playerSizeInX = player.renderer.bounds.size.x;
		playerSizeInY = player.renderer.bounds.size.y;

		// Breite und Hoehe dieser Plattform bestimmen
		plattformWidth = this.renderer.bounds.size.x;
		plattformHeight = this.renderer.bounds.size.y;
	}

	// Pruefe ob ein Spieler basierend auf seine Position in X in der Naehe ist
	bool playerIsNearby( float currentPositionInX ){

		// Falls nicht gefunden, gebe FALSE zurueck
		if (player == null) {
			return false;
		}

		// Rueckgabewert (fuer die besser uebersicht)
		bool setPlattformPassable = true;

		// Feste Groesse gegen Pixel Anhaften an Plattform Rand
		float threshold = 0.5f;
		// aktuelle Position in Y Richtung, bisher nicht benoetigt
		float currentPositionInY = transform.position.y;

		passableMin_X1 = currentPositionInX - plattformWidth;
		passableMax_X1 = currentPositionInX;
		passableMin_X2 = currentPositionInX;
		passableMax_X2 = currentPositionInX + plattformWidth;

		passableMin_Y1Y2 = currentPositionInY + plattformHeight;										// Aktuelle Position der Plattform in Y
		passableMax_Y1Y2 = passableMin_Y1Y2 + playerSizeInY + plattformHeight - threshold;	// Ueber die doppelte Hoehe der Plattform nach oben

		//Debug.Log ("\n ================================== ");
		//Debug.Log ("Rechteck 1: [xMin|xMax] zu [yMin|yMax] : ["+passableMin_X1+"|"+passableMax_X1+"] zu ["+passableMin_Y1Y2+"|"+passableMax_Y1Y2+"]");
		//Debug.Log ("Rechteck 2: [xMin|xMax] zu [yMin|yMax] : ["+passableMin_X2+"|"+passableMax_X2+"] zu ["+passableMin_Y1Y2+"|"+passableMax_Y1Y2+"]");

		// Aktuelle Spieler Positionen ermitteln
		float curPlayerPosInX = player.transform.position.x; 
		//float curPlayerPosInX_2 = curPlayerPosInX + playerSizeInX;
		float curPlayerPosInY = player.transform.position.y + playerSizeInY + threshold;

		// Sofern in Linkem Bereich: TRUE
		if (curPlayerPosInX >= passableMin_X1 && curPlayerPosInX <= passableMax_X1 && curPlayerPosInY > passableMin_Y1Y2 && curPlayerPosInY < passableMax_Y1Y2) {
			setPlattformPassable = false;
		}

		// Sofern in Rechtem Bereich: TRUE
		if (curPlayerPosInX >= passableMin_X2 && curPlayerPosInX <= passableMax_X2 && curPlayerPosInY > passableMin_Y1Y2 && curPlayerPosInY < passableMax_Y1Y2) {
			setPlattformPassable = false;
		}

		return setPlattformPassable;

	}


	// Update is called once per frame
	void Update () {
	
		gameObject.collider2D.enabled = true;

		// Lese aktuelle Position in X 
		float currentPositionInX = transform.position.x;

		// Plattform durchlaessig machen, falls Spieler in der Naehe ist
		gameObject.collider2D.enabled = playerIsNearby(currentPositionInX);
	}
}
