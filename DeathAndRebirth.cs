#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System.Collections;

public class DeathAndRebirth : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Bei Kollision
	void OnCollisionEnter2D( Collision2D col ){
		
		if (col.collider.tag == "Player") {
			col.gameObject.SetActive(false);
		}
	}
}
