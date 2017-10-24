using UnityEngine;
using System.Collections;

public class MenuItems : MonoBehaviour {

	public Sprite original;
	public Sprite alternate;

	void OnMouseOver(){
		GetComponent<SpriteRenderer> ().sprite = alternate;
	}

	void FixedUpdate(){
		GetComponent<SpriteRenderer> ().sprite = original;
	}
}
