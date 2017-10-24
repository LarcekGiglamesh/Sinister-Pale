#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System.Collections;

public class PlattformMovement : MonoBehaviour {

	private bool skipFurtherActions = false;									// Ueberspringe Update() wenn keine Aktion notwendig

	private bool doChangeDirectionInX = false;
	private bool doChangeDirectionInY = false;

	[Tooltip("Horizontale Bewegung der Plattform in Blick-Richtung")]
	public float MovingInX = 0.1f;												// Transformation per Update in X
	[Tooltip("Horizontale Bewegung der Plattform in entgegengesetzter Blick-Richtung sofern eine Patrouille stattfindet")]
	public float MovingInNegativeX = -0.1f;										// Transformation per Update in X

	[Tooltip("Vertikale Bewegung der Plattform in Blick-Richtung")]
	public float MovingInY = 0.1f;												// Transformation per Update in Y
	[Tooltip("Vertikale Bewegung der Plattform in Blick-Richtung sofern eine Patrouille stattfindet")]
	public float MovingInNegativeY = -0.1f;										// Transformation per Update in Y

	[Tooltip("Berechnet den horizontalen Endpunkt basierend auf Startpunkt")]
	public float maxMovementInX = 5.0f;										// Maximaler Fluchtpunkt in X
	[Tooltip("Berechnet den negativen horizontalen Endpunkt basierend auf Startpunkt")]
	public float maxNegativeMovementInX = -5.0f;										// Maximaler Fluchtpunkt in X

	[Tooltip("Berechnet den horizontalen Endpunkt basierend auf Startpunkt")]
	public float maxMovementInY = 5.0f;											// Maximaler Fluchtpunkt in Y
	[Tooltip("Berechnet den negativen horizontalen Endpunkt basierend auf Startpunkt")]
	public float maxNegativeMovementInY = -5.0f;											// Maximaler Fluchtpunkt in Y

	// Entscheidungsvariable fuer den Level Designer
	public enum plattformBehaviour{
		NoMovement,
		OnlyToLeft,
		OnlyToRight,
		OnlyToTop,
		OnlyToBottom,
		OnlyToUpLeft,
		OnlyToUpRight,
		OnlyToDownLeft,
		OnlyToDownRight,
		PatrouilHorizontalStartLeft,
		PatrouilHorizontalStartRight,
		PatrouilVerticalStartUp,
		PatrouilVerticalStartDown,
		PatrouilDiagonalFirstUpLeft,
		PatrouilDiagonalFirstUpRight,
		PatrouilDiagonalFirstDownLeft,
		PatrouilDiagonalFirstDownRight
	}

	[Tooltip("Entscheidungsmerkmal für die Bewegung der Plattform; Vorsicht: sehr genaue Darstellung der Bewegungen!")]
	public plattformBehaviour platMoveBehave = plattformBehaviour.NoMovement;

	public float minPositionForX;										// Minimaler X Wert fuer die Patrouille
	public float maxPositionForX;										// Maxmaler X Wert fuer die Patrouille										
	public float minPositionForY;										// Minimaler Y Wert fuer die Patrouille
	public float maxPositionForY;										// Maxmaler Y Wert fuer die Patrouille

	// Patrouille vorbereiten
	private bool isOnPatrol = false;
	private bool checkMoveHorizontal = false;
	private bool checkMoveVertical = false;

	private int debugCounter = 0;										// Debug Update Counter

	private float plattformSizeInY;

	// Use this for initialization
	void Start () {
		plattformSizeInY = renderer.bounds.size.y;
	}

	void OnTriggerEnter2D( Collider2D col ){

		// Debug.Log ("OnTriggerEnter2D > " + col.gameObject.ToString());

		float playerPosInY = col.transform.position.y;
		float plattformPosInY = transform.position.y;
		bool isOverPlattform = ((playerPosInY - plattformSizeInY) > plattformPosInY ? true : false);

		//Debug.Log ("[OnEnter] Check:  + playerPosInY +  |  + plattformPosInY +  |  + plattformSizeInY +  |  + isOverPlattform");
		//Debug.Log ("[OnEnter] Check: " + playerPosInY + " | " + plattformPosInY + " | " + plattformSizeInY + " | " + isOverPlattform);

		if (col.tag == "Player" && isOverPlattform) {
			// Destroy(col.gameObject.rigidbody2D);

			col.transform.parent = transform;
		}

	}

	/*void OnTriggerStay2D( Collider2D col ){

		float playerPosInY = col.transform.position.y;
		float plattformPosInY = transform.position.y;
		bool isOverPlattform = ((playerPosInY - plattformSizeInY) > plattformPosInY ? true : false);

		// Debug.Log ("OnTriggerStay2D > " + col.gameObject.ToString());

		//Debug.Log ("[OnStay] Check:  + playerPosInY +  |  + plattformPosInY +  |  + plattformSizeInY +  |  + isOverPlattform");
		//Debug.Log ("[OnStay] Check: " + playerPosInY + " | " + plattformPosInY + " | " + plattformSizeInY + " | " + isOverPlattform);

		if (col.tag == "Player" && isOverPlattform) {
			col.transform.parent = transform;
		}

	}*/

	void OnTriggerExit2D( Collider2D col ){

		if (col.tag == "Player") {
			col.transform.parent = null;
		}
	}


	void Awake(){

		// Sofern keine Bewegung in X oder Y vorhanden
		if (MovingInX == 0 && MovingInY == 0) {
			// Ueberspringe aktionen
			skipFurtherActions = true;
		}

		if (platMoveBehave == plattformBehaviour.NoMovement) {
			GetComponent<PlattformMovement>().enabled = false;
			return;
		}

		float curPosX = transform.position.x;			// Bestimme die Startposition der Plattform in X
		float curPosY = transform.position.y;			// Bestimme die Startposition der Plattform in Y

		// Bestimme die Zielpositionen in X und Y
		// minPositionForX = curPosX + maxMovementInX;
		// maxPositionForX = curPosX + maxNegativeMovementInX;
		// minPositionForY = curPosY + maxMovementInY;
		// maxPositionForY = curPosY + maxNegativeMovementInY;
		// if (true)	return;

		maxMovementInX = Mathf.Abs (maxMovementInX);
		maxMovementInY = Mathf.Abs (maxMovementInY);
		maxNegativeMovementInX = Mathf.Abs (maxNegativeMovementInX);
		maxNegativeMovementInY = Mathf.Abs (maxNegativeMovementInY);
		// MovingInX = Mathf.Abs (MovingInX);
		// MovingInY = Mathf.Abs (MovingInY);

		switch (platMoveBehave) {
			
			// Geradlinig einfach
		case plattformBehaviour.OnlyToLeft:
			minPositionForX = curPosX - maxMovementInX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY;
			maxPositionForY = curPosY;
			break;
			
		case plattformBehaviour.OnlyToRight:
			minPositionForX = curPosX + maxMovementInX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY;
			maxPositionForY = curPosY;
			break;
			
		case plattformBehaviour.OnlyToTop:
			minPositionForX = curPosX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY + maxMovementInY;
			maxPositionForY = curPosY;

			break;
			
		case plattformBehaviour.OnlyToBottom:
			minPositionForX = curPosX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY - maxMovementInY;
			maxPositionForY = curPosY;
			break;
			
			// ========================================
			
			// Geradlinig komplex
		case plattformBehaviour.OnlyToUpLeft:
			minPositionForX = curPosX - maxMovementInX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY + maxMovementInY;
			maxPositionForY = curPosY;
			break;
			
		case plattformBehaviour.OnlyToUpRight:
			minPositionForX = curPosX + maxMovementInX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY + maxMovementInY;
			maxPositionForY = curPosY;
			break;
			
		case plattformBehaviour.OnlyToDownLeft:
			minPositionForX = curPosX - maxMovementInX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY - maxMovementInY;
			maxPositionForY = curPosY;
			break;
			
		case plattformBehaviour.OnlyToDownRight:
			minPositionForX = curPosX + maxMovementInX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY - maxMovementInY;
			maxPositionForY = curPosY;
			break;
			
			// ========================================
			
			// Patrouille einfach
			
		case plattformBehaviour.PatrouilHorizontalStartLeft:
			minPositionForX = curPosX - maxMovementInX;
			maxPositionForX = curPosX + maxNegativeMovementInX;
			minPositionForY = curPosY;
			maxPositionForY = curPosY;
			break;
			
		case plattformBehaviour.PatrouilHorizontalStartRight:
			minPositionForX = curPosX + maxMovementInX;
			maxPositionForX = curPosX - maxNegativeMovementInX;
			minPositionForY = curPosY;
			maxPositionForY = curPosY;
			break;
			
		case plattformBehaviour.PatrouilVerticalStartUp:
			minPositionForX = curPosX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY + maxMovementInY;
			maxPositionForY = curPosY - maxNegativeMovementInY;
			break;
			
		case plattformBehaviour.PatrouilVerticalStartDown:
			minPositionForX = curPosX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY - maxMovementInY;
			maxPositionForY = curPosY + maxNegativeMovementInY;
			break;
			
			// ========================================
			
			// Patrouille komplex
			
		case plattformBehaviour.PatrouilDiagonalFirstUpLeft:
			minPositionForX = curPosX - maxMovementInX;
			maxPositionForX = curPosX + maxNegativeMovementInX;
			minPositionForY = curPosY + maxMovementInY;
			maxPositionForY = curPosY - maxNegativeMovementInY;
			break;
			
		case plattformBehaviour.PatrouilDiagonalFirstUpRight:
			minPositionForX = curPosX + maxMovementInX;
			maxPositionForX = curPosX - maxNegativeMovementInX;
			minPositionForY = curPosY + maxMovementInY;
			maxPositionForY = curPosY - maxNegativeMovementInY;
			break;
			
		case plattformBehaviour.PatrouilDiagonalFirstDownLeft:
			minPositionForX = curPosX - maxMovementInX;
			maxPositionForX = curPosX + maxNegativeMovementInX;
			minPositionForY = curPosY - maxMovementInY;
			maxPositionForY = curPosY + maxNegativeMovementInY;
			break;
			
		case plattformBehaviour.PatrouilDiagonalFirstDownRight:
			minPositionForX = curPosX + maxMovementInX;
			maxPositionForX = curPosX - maxNegativeMovementInX;
			minPositionForY = curPosY - maxMovementInY;
			maxPositionForY = curPosY + maxNegativeMovementInY;
			break;
			
		default:
			break;
		}

	}

	/*void FixedUpdate(){

		// Debug.Log ("FixedUpdate() called");

	}*/

	// Update is called once per frame
	void Update () {
	
		// Debug.Log ("Update() called");

		debugCounter++;

		// Lese aktuelle Position in X und Y aus
		float currentPositionInX = transform.position.x;
		float currentPositionInY = transform.position.y;
		float currentPositionInZ = transform.position.z;

		// Schrittweite für X und Y vorbereiten
		float stepInX = 0;
		float stepInY = 0;

		switch (platMoveBehave) {

			// Geradlinig einfach
			case plattformBehaviour.OnlyToLeft:
			stepInX = MovingInX * -1;
			stepInY = 0;
			isOnPatrol = false;
			checkMoveHorizontal = true;
			break;

			case plattformBehaviour.OnlyToRight:
			stepInX = MovingInX;
			stepInY = 0;
			isOnPatrol = false;
			checkMoveHorizontal = true;
			break;

			case plattformBehaviour.OnlyToTop:
			stepInX = 0;
			stepInY = MovingInY;
			isOnPatrol = false;
			checkMoveVertical = true;
			break;

			case plattformBehaviour.OnlyToBottom:
			stepInX = 0;
			stepInY = MovingInY * -1;
			isOnPatrol = false;
			checkMoveVertical = true;
			break;

			// ========================================

			// Geradlinig komplex
			case plattformBehaviour.OnlyToUpLeft:
			stepInX = MovingInX * -1;
			stepInY = MovingInY;
			isOnPatrol = false;
			checkMoveHorizontal = true;
			checkMoveVertical = true;
			break;
			
			case plattformBehaviour.OnlyToUpRight:
			stepInX = MovingInX;
			stepInY = MovingInY;
			isOnPatrol = false;
			checkMoveHorizontal = true;
			checkMoveVertical = true;
			break;
			
			case plattformBehaviour.OnlyToDownLeft:
			stepInX = MovingInX * -1;
			stepInY = MovingInY * -1;
			isOnPatrol = false;
			checkMoveHorizontal = true;
			checkMoveVertical = true;
			break;
			
			case plattformBehaviour.OnlyToDownRight:
			stepInX = MovingInX;
			stepInY = MovingInY * -1;
			isOnPatrol = false;
			checkMoveHorizontal = true;
			checkMoveVertical = true;
			break;

			// ========================================

			// Patrouille einfach

			case plattformBehaviour.PatrouilHorizontalStartLeft:
			stepInX = ( !doChangeDirectionInX ? MovingInX : MovingInNegativeX ) * -1;
			stepInY = 0;
			isOnPatrol = true;
			break;

			case plattformBehaviour.PatrouilHorizontalStartRight:
			stepInX = ( !doChangeDirectionInX ? MovingInX : MovingInNegativeX ) ;
			stepInY = 0;
			isOnPatrol = true;
			break;

			case plattformBehaviour.PatrouilVerticalStartUp:
			stepInX = 0;
			stepInY = ( !doChangeDirectionInY ? MovingInY : MovingInNegativeY ) ;
			isOnPatrol = true;
			break;

			case plattformBehaviour.PatrouilVerticalStartDown:
			stepInX = 0;
			stepInY = ( !doChangeDirectionInY ? MovingInY : MovingInNegativeY ) * -1;
			isOnPatrol = true;
			break;

			// ========================================

			// Patrouille komplex

			case plattformBehaviour.PatrouilDiagonalFirstUpLeft:
			stepInX = ( !doChangeDirectionInX ? MovingInX : MovingInNegativeX ) * -1;
			stepInY = ( !doChangeDirectionInY ? MovingInY : MovingInNegativeY );
			isOnPatrol = true;
			break;
			
			case plattformBehaviour.PatrouilDiagonalFirstUpRight:
			stepInX = ( !doChangeDirectionInX ? MovingInX : MovingInNegativeX );
			stepInY = ( !doChangeDirectionInY ? MovingInY : MovingInNegativeY );
			isOnPatrol = true;
			break;
			
			case plattformBehaviour.PatrouilDiagonalFirstDownLeft:
			stepInX = ( !doChangeDirectionInX ? MovingInX : MovingInNegativeX ) * -1;
			stepInY = ( !doChangeDirectionInY ? MovingInY : MovingInNegativeY ) * -1;
			isOnPatrol = true;
			break;
			
			case plattformBehaviour.PatrouilDiagonalFirstDownRight:
			stepInX = ( !doChangeDirectionInX ? MovingInX : MovingInNegativeX );
			stepInY = ( !doChangeDirectionInY ? MovingInY : MovingInNegativeY ) * -1;
			isOnPatrol = true;
			break;

			default:
				break;
		}
//		Debug.Log ("\n============================ ( " + debugCounter + " ) ========================================");
//		Debug.Log ("CurXPos: " + currentPositionInX + " | minPosX: " + minPositionForX + " | maxPosX: " + maxPositionForX);
//		Debug.Log ("CurYPos: " + currentPositionInY + " | minPosY: " + minPositionForY + " | maxPosY: " + maxPositionForY);

		// ausgehend der Schrittweite nun eine Positionsveraenderung in X durchfuehren
		currentPositionInX += stepInX;
		// sofern die Minimalposition oder die Maximalposition in X erreicht wird, schalte die Bewegung um, ansonsten belasse die Bewegungsrichtung
		doChangeDirectionInX = (minPositionForX >= currentPositionInX || maxPositionForX <= currentPositionInX ? !doChangeDirectionInX : doChangeDirectionInX);

		// ausgehend der Schrittweite nun eine Positionsveraenderung in Y durchfuehren
		currentPositionInY += stepInY;
		// sofern die Minimalposition oder die Maximalposition in Y erreicht wird, schalte die Bewegung um, ansonsten belasse die Bewegungsrichtung
		doChangeDirectionInY = (minPositionForY >= currentPositionInY || maxPositionForY <= currentPositionInY ? !doChangeDirectionInY : doChangeDirectionInY);

//		Debug.Log ("doMovement: " + platMoveBehave.ToString() + " | doPatrol: " + isOnPatrol);
//		Debug.Log ("Movement Step in X: " + stepInX + " | changeDirection: " + doChangeDirectionInX);
//		Debug.Log ("Movement Step in Y: " + stepInY + " | changeDirection: " + doChangeDirectionInY);

		// Bei einmaliger Bewegung
		if (!isOnPatrol) {
			// Zwischenwert setzen
			bool checkHorizontal = false;
			bool checkVertical = false;
			bool checkResult = false;
			// Pruefe ob Minimalwert oder Maximalwert in X erreicht
			checkHorizontal = ( minPositionForX >= currentPositionInX || maxPositionForX <= currentPositionInX ) && checkMoveHorizontal;
			// Pruefe ob Minimalwert oder Maximalwert in Y erreicht oder nehme bekannte Aussage, sofern diese TRUE ist
			checkVertical = ( minPositionForY >= currentPositionInY || maxPositionForY <= currentPositionInY ) && checkMoveVertical;
			//
			checkResult = checkHorizontal || checkVertical;
			// uebernehme Wahrheitswert zur Kontrolle der weiteren Bewegung
			// skipFurtherActions = checkResult;
			// Lese diese Komponente und deaktiviere sie
			GetComponent<PlattformMovement>().enabled = !checkResult;
		}

		// Neue Position festsetzen
		Vector3 newPosition = new Vector3( currentPositionInX, currentPositionInY, currentPositionInZ );
		// Neue Position uebernehmen
		transform.position = newPosition;

	}
}
