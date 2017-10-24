using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

	public int offsetX = 2;
		
	public bool hasARightBuddy = false;
	public bool hasALeftBuddy = false;
	
	public bool reverseScale = false;
	
	private float spriteWidth = 0f;
	private Camera cam;
	private Transform myTransform;
	
	void Awake(){
		cam = Camera.main;
		myTransform = transform;
	}
	
	void Start(){
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = sRenderer.sprite.bounds.size.x;
		spriteWidth = sRenderer.bounds.size.x;
	}
	
	void Update(){
		if (hasALeftBuddy == false || hasARightBuddy == false){
			// calculate the cameras extend (half the width) of what the camera can see in world coordinates
			float camHorizontalExtend = cam.orthographicSize * Screen.width/Screen.height;
			
			// calculate the x position where the camera can see the edge of the sprite (element)
			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtend;
			float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth/2) + camHorizontalExtend;
			// checking if we can see the edge of the element and then calling MakeNewBuddy if we can
			if ( cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
			{
				MakeNewBuddy(1);
				hasARightBuddy = true;
			}
			else if ( cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false )
			{
				MakeNewBuddy(-1);
				hasALeftBuddy = true;
			}
		}
		
	}
	
	// a function that creates a buddy on the side required
	void MakeNewBuddy ( int rightOrLeft ){
		// calculating the new position for our new buddy
		Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
		// Instatiating our new body and storing him in a variable
		Transform newBuddy = Instantiate (myTransform, newPosition, myTransform.rotation) as Transform;
		// if not tilable let's reverse the x size of our object to get rid of ugly seams
		if ( reverseScale == true ){
			newBuddy.localScale = new Vector3(newBuddy.localScale.x*-1f, newBuddy.localScale.y, newBuddy.localScale.z);
		}
		
		newBuddy.parent = myTransform.parent;
		
		if (rightOrLeft > 0) {
			newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
		}
		else
		{
			newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
		}
	}

}
