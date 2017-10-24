#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	private bool skipFurtherActions = false;									// Ueberspringe Update() wenn keine Aktion notwendig
	
	private bool doChangeDirectionInX = false;
	private bool doChangeDirectionInY = false;
	
	[Tooltip("Horizontale Bewegung des Gegners in Blick-Richtung")]
	public float MovingInX = 1f;												// Transformation per Update in X
	[Tooltip("Horizontale Bewegung des Gegners in entgegengesetzter Blick-Richtung sofern eine Patrouille stattfindet")]
	public float MovingInNegativeX = -1f;										// Transformation per Update in X
	
	[Tooltip("Vertikale Bewegung des Gegners in Blick-Richtung")]
	public float MovingInY = 1f;												// Transformation per Update in Y
	[Tooltip("Vertikale Bewegung des Gegners in Blick-Richtung sofern eine Patrouille stattfindet")]
	public float MovingInNegativeY = -1f;										// Transformation per Update in Y
	
	[Tooltip("Berechnet den horizontalen Endpunkt basierend auf Startpunkt")]
	public float maxMovementInX = 10.0f;										// Maximaler Fluchtpunkt in X
	[Tooltip("Berechnet den negativen horizontalen Endpunkt basierend auf Startpunkt")]
	public float maxNegativeMovementInX = -10.0f;										// Maximaler Fluchtpunkt in X
	
	[Tooltip("Berechnet den horizontalen Endpunkt basierend auf Startpunkt")]
	public float maxMovementInY = 10.0f;											// Maximaler Fluchtpunkt in Y
	[Tooltip("Berechnet den negativen horizontalen Endpunkt basierend auf Startpunkt")]
	public float maxNegativeMovementInY = -10.0f;											// Maximaler Fluchtpunkt in Y
	
	// Entscheidungsvariable fuer den Level Designer
	public enum enemyMovementPattern{
		NoMovement,
		OnlyToLeft,
		OnlyToRight,
		OnlyToTop,
		OnlyToBottom,
		PatrouilHorizontalStartLeft,
		PatrouilHorizontalStartRight,
		PatrouilVerticalStartUp,
		PatrouilVerticalStartDown,
	}
	
	[Tooltip("Entscheidungsmerkmal für die Bewegung des Gegners; Vorsicht: sehr genaue Darstellung der Bewegungen!")]
	public enemyMovementPattern enemyMovePattern = enemyMovementPattern.NoMovement;
	
	public float minPositionForX;										// Minimaler X Wert fuer die Patrouille
	public float maxPositionForX;										// Maxmaler X Wert fuer die Patrouille										
	public float minPositionForY;										// Minimaler Y Wert fuer die Patrouille
	public float maxPositionForY;										// Maxmaler Y Wert fuer die Patrouille
	
	// Patrouille vorbereiten
	private bool isOnPatrol = false;
	private bool checkMoveHorizontal = false;
	private bool checkMoveVertical = false;
	
	private int debugCounter = 0;										// Debug Update Counter

	public bool isFacingRight = false;

	// Use this for initialization
	void Start () {
		
	}
	
	
	void Awake(){
		
		// Sofern keine Bewegung in X oder Y vorhanden
		if (MovingInX == 0 && MovingInY == 0) {
			// Ueberspringe aktionen
			skipFurtherActions = true;
		}
		
		if (enemyMovePattern == enemyMovementPattern.NoMovement) {
			GetComponent<EnemyMovement>().enabled = false;
			return;
		}
		
		float curPosX = transform.position.x;			// Bestimme die Startposition des Gegners in X
		float curPosY = transform.position.y;			// Bestimme die Startposition des Gegners in Y

		maxMovementInX = Mathf.Abs (maxMovementInX);
		maxMovementInY = Mathf.Abs (maxMovementInY);
		maxNegativeMovementInX = Mathf.Abs (maxNegativeMovementInX);
		maxNegativeMovementInY = Mathf.Abs (maxNegativeMovementInY);
		// MovingInX = Mathf.Abs (MovingInX);
		// MovingInY = Mathf.Abs (MovingInY);
		
		switch (enemyMovePattern) {
			
			// Geradlinig einfach
		case enemyMovementPattern.OnlyToLeft:
			minPositionForX = curPosX - maxMovementInX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY;
			maxPositionForY = curPosY;
			break;
			
		case enemyMovementPattern.OnlyToRight:
			minPositionForX = curPosX + maxMovementInX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY;
			maxPositionForY = curPosY;
			break;
			
		case enemyMovementPattern.OnlyToTop:
			minPositionForX = curPosX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY + maxMovementInY;
			maxPositionForY = curPosY;
			
			break;
			
		case enemyMovementPattern.OnlyToBottom:
			minPositionForX = curPosX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY - maxMovementInY;
			maxPositionForY = curPosY;
			break;
			
			// ========================================

			// Patrouille einfach
			
		case enemyMovementPattern.PatrouilHorizontalStartLeft:
			minPositionForX = curPosX - maxMovementInX;
			maxPositionForX = curPosX + maxNegativeMovementInX;
			minPositionForY = curPosY;
			maxPositionForY = curPosY;
			break;
			
		case enemyMovementPattern.PatrouilHorizontalStartRight:
			minPositionForX = curPosX + maxMovementInX;
			maxPositionForX = curPosX - maxNegativeMovementInX;
			minPositionForY = curPosY;
			maxPositionForY = curPosY;
			break;
			
		case enemyMovementPattern.PatrouilVerticalStartUp:
			minPositionForX = curPosX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY + maxMovementInY;
			maxPositionForY = curPosY - maxNegativeMovementInY;
			break;
			
		case enemyMovementPattern.PatrouilVerticalStartDown:
			minPositionForX = curPosX;
			maxPositionForX = curPosX;
			minPositionForY = curPosY - maxMovementInY;
			maxPositionForY = curPosY + maxNegativeMovementInY;
			break;

		default:
			break;
		}
		
	}
	
	void FixedUpdate(){
		
		// Debug.Log ("FixedUpdate() called");
		
	}

	/* Drehe den Charakter um sofern Blickrichtung nicht der Bewegungsrichtung entspricht */
	void Flip(){
		// Variable der Blickrichtung umdrehen
		isFacingRight = !isFacingRight;
		// aktuelle Transformation merken
		Vector3 theScale = transform.localScale;
		// die Blickrichtung innerhalb des Vektors umdrehen
		theScale.x *= -1;
		// neue Transformation uebernehmen
		transform.localScale = theScale;
	}
	
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
		
		switch (enemyMovePattern) {
			
			// Geradlinig einfach
		case enemyMovementPattern.OnlyToLeft:
			stepInX = MovingInX * -1;
			stepInY = 0;
			isOnPatrol = false;
			checkMoveHorizontal = true;
			break;
			
		case enemyMovementPattern.OnlyToRight:
			stepInX = MovingInX;
			stepInY = 0;
			isOnPatrol = false;
			checkMoveHorizontal = true;
			break;
			
		case enemyMovementPattern.OnlyToTop:
			stepInX = 0;
			stepInY = MovingInY;
			isOnPatrol = false;
			checkMoveVertical = true;
			break;
			
		case enemyMovementPattern.OnlyToBottom:
			stepInX = 0;
			stepInY = MovingInY * -1;
			isOnPatrol = false;
			checkMoveVertical = true;
			break;

			// ========================================
			
			// Patrouille einfach
			
		case enemyMovementPattern.PatrouilHorizontalStartLeft:
			stepInX = ( !doChangeDirectionInX ? MovingInX : MovingInNegativeX ) * -1;
			stepInY = 0;
			isOnPatrol = true;
			break;
			
		case enemyMovementPattern.PatrouilHorizontalStartRight:
			stepInX = ( !doChangeDirectionInX ? MovingInX : MovingInNegativeX ) ;
			stepInY = 0;
			isOnPatrol = true;
			break;
			
		case enemyMovementPattern.PatrouilVerticalStartUp:
			stepInX = 0;
			stepInY = ( !doChangeDirectionInY ? MovingInY : MovingInNegativeY ) ;
			isOnPatrol = true;
			break;
			
		case enemyMovementPattern.PatrouilVerticalStartDown:
			stepInX = 0;
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
		
		//		Debug.Log ("doMovement: " + enemyMovePattern.ToString() + " | doPatrol: " + isOnPatrol);
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
			GetComponent<EnemyMovement>().enabled = !checkResult;

		}

		if ( doChangeDirectionInX != isFacingRight && ( enemyMovePattern == enemyMovementPattern.PatrouilHorizontalStartLeft || enemyMovePattern == enemyMovementPattern.PatrouilHorizontalStartRight ) ){
			Flip ();
		}
		isFacingRight = doChangeDirectionInX;
		
		// Neue Position festsetzen
		Vector3 newPosition = new Vector3( currentPositionInX, currentPositionInY, currentPositionInZ );
		// Neue Position uebernehmen
		transform.position = newPosition;
		
	}
}
