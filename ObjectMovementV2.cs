using UnityEngine;
using System.Collections;

public class ObjectMovementV2 : MonoBehaviour {

	public float velocityInX = 1.0f;
	public float velocityInY = 0.0f;
	
	private int directionX = 0;
	private int directionY = 0;
	private int nextDirectionX = 0;
	private int nextDirectionY = 0;
	
	public float minPositionForX = 0.0f;
	public float maxPositionForX = 0.0f;
	public float minPositionForY = 0.0f;
	public float maxPositionForY = 0.0f;

	private float check = 0.0f;
	private bool isFacingRight = true;

	public enum enemyMovementPattern{
		NoMovement,
		OnlyToLeft,
		OnlyToRight,
		OnlyToTop,
		OnlyToBottom,
		PatrouilHorizontalStartLeft,
		PatrouilHorizontalStartRight,
		PatrouilVerticalStartUp,
		PatrouilVerticalStartDown
	}

	public enemyMovementPattern enemyMovePattern;

	void Start(){

		float currentPositionInX = transform.localPosition.x;
		float currentPositionInY = transform.localPosition.y;

		switch (enemyMovePattern) {
			
		// Geradlinig einfach
		case enemyMovementPattern.OnlyToLeft:
			directionX = -1;
			nextDirectionX = 0;
			break;

		case enemyMovementPattern.OnlyToRight:
			directionX = 1;
			nextDirectionX = 0;
			break;

		case enemyMovementPattern.OnlyToTop:
			directionY = 1;
			nextDirectionY = 0;
			break;

		case enemyMovementPattern.OnlyToBottom:
			directionY = -1;
			nextDirectionY = 0;
			break;

		/* ******************************************************* *
		 * 
		 * ******************************************************* */

		// Patrouille
		case enemyMovementPattern.PatrouilHorizontalStartLeft:
			directionX = -1;
			nextDirectionX = 1;
			break;

		
		case enemyMovementPattern.PatrouilHorizontalStartRight:
			directionX = 1;
			nextDirectionX = -1;
			break;
			
		case enemyMovementPattern.PatrouilVerticalStartUp:
			directionY = 1;
			nextDirectionY = -1;
			break;
			
		case enemyMovementPattern.PatrouilVerticalStartDown:
			directionY = -1;
			nextDirectionY = 1;
			break;
		}

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

	void Update()
	{
		if (Time.time <= check) {
			return;
		} else {
			check += 0.5f;
		}

		float localX = transform.localPosition.x;
		float localY = transform.localPosition.y;

		// Debug.Log ("Aktuelle Position bei: ("+localX+"|"+localY+")");
		// Debug.Log ("Im Vergleich zu: ("+minPositionForX+"|"+maxPositionForX+") >> " + (localX < minPositionForX));

		int tempDirection;
		if (localX < minPositionForX) {
			tempDirection = directionX;
			directionX = nextDirectionX;
			nextDirectionX = tempDirection;
			Flip ();
		} else if (localX > maxPositionForX) {
			tempDirection = directionX;
			directionX = nextDirectionX;
			nextDirectionX = tempDirection;
			Flip ();
		}

		if (localY < minPositionForY) {
			tempDirection = directionY;
			directionY = nextDirectionY;
			nextDirectionY = tempDirection;
			// Debug.Log ("[MIN-Y] Wechsel Bewegung nach: " + directionY);
		} else if (localY > maxPositionForY) {
			tempDirection = directionY;
			directionY = nextDirectionY;
			nextDirectionY = tempDirection;
			// Debug.Log ("[MAX-Y] Wechsel Bewegung nach: " + directionY);
		}

		bool leaveEnabled = true;
		if (directionX == 0) {
			if ( enemyMovePattern == enemyMovementPattern.OnlyToLeft || enemyMovePattern == enemyMovementPattern.OnlyToRight ){
				leaveEnabled = false;
			}
		}
		if (directionY == 0) {
			if ( enemyMovePattern == enemyMovementPattern.OnlyToTop || enemyMovePattern == enemyMovementPattern.OnlyToBottom ){
				leaveEnabled = false;
			}
		}

		Vector2 newVector;
		if (leaveEnabled == false) {
			newVector = new Vector2 (0.0f, 0.0f);
		} else {
			newVector = new Vector2( velocityInX * directionX , velocityInY * directionY );
		}
		// Debug.Log ("Neuer Vektor in Richtung: ("+newVector.x+"|"+newVector.y+")");
		rigidbody2D.velocity = newVector;
		gameObject.GetComponent<ObjectMovementV2> ().enabled = leaveEnabled;
	}

}
