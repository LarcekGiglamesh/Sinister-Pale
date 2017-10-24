using UnityEngine;
using System.Collections;

public class PlattformMoveOnJump : MonoBehaviour {

	// Umschalter fuer das Plattform Bewegungs Skript
	private bool changeEnabled = false;

	// Am Start Variable setzen
	void Start(){
		// Zur Sicherheit
		changeEnabled = false;
	}

	// Pruefen ob ein Spieler auf die Plattform gesprungen ist
	void OnCollisionEnter2D( Collision2D col ){
		// Wenn ein Spieler auf die Plattform gesprungen ist
		if ( col.gameObject.tag == "Player" ){
			// Schalter umlegen
			changeEnabled = true;
		}
	}

	// Eine relativ haeufige Pruefung reicht
	void FixedUpdate () {
		// Sofern Sprung erkannt wurde
		if ( changeEnabled == true ){
			// Plattform Bewegung aktivieren
			GetComponent<PlattformMovement>().enabled = true;
			// Dieses Skript deaktivieren
			GetComponent<PlattformMoveOnJump>().enabled = false;
			// Neuen BoxCollider2D fuer die Befestigung des Spielers erstellen
			BoxCollider2D temp = (BoxCollider2D) gameObject.AddComponent("BoxCollider2D");
			// BoxCollider2D als Trigger festsetzen
			temp.isTrigger = true;
		}

	}
}
