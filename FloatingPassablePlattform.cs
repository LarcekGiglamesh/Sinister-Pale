#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System.Collections;

public class FloatingPassablePlattform : MonoBehaviour {

	private bool skipFurtherActions = false;							// Ueberspringe Update() wenn keine Aktion notwendig

	[Tooltip("Wenn nicht gesetzt, bewegt sich die Figur dauerhaft in die Blickrichtung")]
	public bool isOnPatrol = true;										// Patrouille aktiv? (Vor- und Zurueck Laufen)
	[Tooltip("Aktuelle Blickrichtung. Sofern bewegend: Laufrichtung")]
	public bool startMovingLeft = true;									// Blick- und Laufrichtung zu Beginn Links?
	[Tooltip("Aktuell: Transformation in X-Richtung (wird noch auf das Geschwindigkeits Modell angepasst)")]
	public float MovingInX = 1.0f;										// Transformation per Update in X
	[Tooltip("ToBeImplemented | Aktuell: Transformation in Y-Richtung (wird noch auf das Geschwindigkeits Modell angepasst)")]
	public float MovingInY = 0.0f;										// Transformation per Update in Y

	[Tooltip("Berechnet den horizontalen Endpunkt basierend auf Startpunkt")]
	public float maxMovementInX = 50.0f;								// Maximaler Fluchtpunkt in X
	[Tooltip("ToBeImplemented | Berechnet den horizontalen Endpunkt basierend auf Startpunkt")]
	public float maxMovementInY = 0.0f;									// Maximaler Fluchtpunkt in Y


	private float playerSizeInX = 0.0f;									// Breite des Spielers
	private float playerSizeInY = 0.0f;									// Hoehe des Spielers
	private float minPositionForX;										// Minimaler X Wert fuer die Patrouille
	private float maxPositionForX;										// Maxmaler X Wert fuer die Patrouille										
	private float minPositionForY;										// Minimaler Y Wert fuer die Patrouille
	private float maxPositionForY;										// Maxmaler Y Wert fuer die Patrouille

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

	private int debugCounter = 0;										// Debug Update Counter

	// Use this for initialization
	void Start () {

	}


	void Awake(){

		// Sofern keine Bewegung in X oder Y vorhanden
		if (MovingInX == 0 && MovingInY == 0) {
			// Ueberspringe aktionen
			skipFurtherActions = true;
		}

		float curPosX = transform.position.x;			// Bestimme die Startposition der Plattform in X
		float curPosY = transform.position.y;			// Bestimme die Startposition der Plattform in Y

		// Bestimme die Zielpositionen in X und Y
		minPositionForX = curPosX - maxMovementInX;
		maxPositionForX = curPosX + maxMovementInX;
		minPositionForY = curPosY - maxMovementInY;
		maxPositionForY = curPosY + maxMovementInY;


		// Finde das GameObject des Spielers
		player = GameObject.Find ("Player");

		// Bestimme die Groesse des Spielers
		playerSizeInX = player.renderer.bounds.size.x;
		playerSizeInY = player.renderer.bounds.size.y;

		// Breite und Hoehe dieser Plattform bestimmen
		plattformWidth = this.renderer.bounds.size.x;
		plattformHeight = this.renderer.bounds.size.y;
	}

	void FixedUpdate() {

	}

	// Fuehre eine Bewegung der Plattform ueber die bekannte X Position durch
	float MovePlattform( float currentPositionInX ){

		// Debug.Log ("[" + debugCounter + "] Current X Position: " + currentPositionInX + " | Move by: " + MovingInX + " | Moving to Left? > " + startMovingLeft + " | (MIN|MAX): ("+minPositionForX+"|"+maxMovementInX+")");
		// Debug.Log ("(MIN|MAX) = ( " + minPositionForX + " | " + maxPositionForX + " )");
		
		// Sofern sich die Plattform nach links bewegt
		if (startMovingLeft) {
			
			// Debug.Log("Cur|Move|Min = " + currentPositionInX + "," + MovingInX + "," + minPositionForX);
			
			// Ziehe einen Bewegungsschritt ab
			currentPositionInX -= MovingInX;
			
			// umdrehen sofern minimalwert erreicht
			if ( minPositionForX >= currentPositionInX ){
				// plattform auf den Minimalwert setzen
				currentPositionInX = minPositionForX;
				
				// sofern eine Patrouille aktiv ist, Bewegung umschalten
				if ( isOnPatrol ){
					// Bewegung nach Rechts initiieren
					startMovingLeft = false;
				}else{
					skipFurtherActions = true;
				}
				
			}
			
		} else {
			
			// Debug.Log("Cur|Move|Max = " + currentPositionInX + "," + MovingInX + "," + maxPositionForX);
			
			// Fuege einen Bewegungsschritt hinzu
			currentPositionInX += MovingInX;
			
			// umdrehen sofern maximalwert erreicht
			if ( maxPositionForX <= currentPositionInX ){
				// plattform auf Maximalwert setzen
				currentPositionInX = maxPositionForX;
				
				// sofern eine Patrouille aktiv ist, Bewegung umschalten
				if ( isOnPatrol ){
					// Bewegung nach Links initiieren
					startMovingLeft = true;
				}else{
					skipFurtherActions = true;
				}
			}
			
		}

		// gebe die neue Position in X zurueck
		return currentPositionInX;
	}


	bool playerIsNearby( float currentPositionInX ){

		// Falls nicht gefunden, gebe FALSE zurueck
		if (player == null) {
			return false;
		}

		// Rueckgabewert (fuer die besser uebersicht)
		bool setPlattformPassable = true;

		// Debug.Log ("Size of Player Object in [X|Y]: [" + playerSizeInX + "|" + playerSizeInY + "]");

		// Feste Groesse gegen Pixel Anhaften an Plattform Rand
		float threshold = 0.5f;
		// aktuelle Position in Y Richtung, bisher nicht benoetigt
		float currentPositionInY = transform.position.y;

		passableMin_X1 = currentPositionInX - playerSizeInX + threshold;			// Aktuelle Position der Plattform um Spielergroesse nach links verschoben + Threshold nach rechts
		passableMax_X1 = passableMin_X1 + plattformWidth - threshold;				// Linke Position (s.o.) ueber die Breite der Plattform + Threshold nach links
		passableMin_X2 = currentPositionInX + playerSizeInX + threshold;			// Aktuelle Position der Plattform um Spielergroesse nach rechts geschoben - Threshold nach links
		passableMax_X2 = passableMin_X2 + plattformWidth - threshold;				// Linke Position (s.o.) ueber die Breite der Plattform - Threshold nach links

		passableMin_Y1Y2 = currentPositionInY + plattformHeight;										// Aktuelle Position der Plattform in Y
		passableMax_Y1Y2 = passableMin_Y1Y2 + playerSizeInY + plattformHeight;	// Ueber die doppelte Hoehe der Plattform nach oben

		// Aktuelle Spieler Positionen ermitteln
		float curPlayerPosInX = player.collider2D.transform.position.x;
		float curPlayerPosInY = player.collider2D.transform.position.y + playerSizeInY + threshold;

		// Sofern in Linkem Bereich: TRUE
		if (curPlayerPosInX >= passableMin_X1 && curPlayerPosInX <= passableMax_X1 && curPlayerPosInY > passableMin_Y1Y2 && curPlayerPosInY < passableMax_Y1Y2) {
			setPlattformPassable = false;
		}

		// Sofern in Rechtem Bereich: TRUE
		if (curPlayerPosInX >= passableMin_X2 && curPlayerPosInX <= passableMax_X2 && curPlayerPosInY > passableMin_Y1Y2 && curPlayerPosInY < passableMax_Y1Y2) {
			setPlattformPassable = false;
		}

		// Debug.Log (" ===================================== ");
		// Debug.Log ("Player Position (x|y): ("+curPlayerPosInX+"|"+curPlayerPosInY+")");
		// Debug.Log ("Rechteck 1: [MinX|MaxX] ["+passableMin_X1+"|"+passableMax_X1+"]         zu [MinY|MaxY] ["+passableMin_Y1Y2+"|"+passableMax_Y1Y2+"]");
		// Debug.Log ("Rechteck 2: [MinX|MaxX] ["+passableMin_X2+"|"+passableMax_X2+"]         zu [MinY|MaxY] ["+passableMin_Y1Y2+"|"+passableMax_Y1Y2+"]");
		// Debug.Log ("Passing? >> " + setPlattformPassable);

		return setPlattformPassable;

	}


	// Update is called once per frame
	void Update () {
	
		// Sofern keine Aktion notwendig
		if (skipFurtherActions) {
			// tue nichts, springe aus Methode heraus
			return;
		}

		// debugCounter++;

		// Lese aktuelle Position in X aus
		float currentPositionInX = transform.position.x;		
		// Bestimme die neue Position in X basierend auf die aktuelle Position sowie den globalen Werten
		currentPositionInX = MovePlattform (currentPositionInX );

		// 
		gameObject.collider2D.enabled = playerIsNearby(currentPositionInX);

		// Neue Position festsetzen
		Vector2 newPosition = new Vector2( currentPositionInX, transform.position.y );
		// Neue Position uebernehmen
		transform.position = newPosition;

	}
}
