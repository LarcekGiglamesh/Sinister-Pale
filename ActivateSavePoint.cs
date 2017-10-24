using UnityEngine;
using System.Collections;

public class ActivateSavePoint : MonoBehaviour {

	// Reload Objekt fuer den Mittelpunkt
	public GameObject TargetReloadGameObject;
	// 
	public int SaveStateForScript;

	void Start(){
		// Reload Objekt inaktiv setzen
		TargetReloadGameObject.SetActive(false);
	}

	void OnTriggerEnter2D( Collider2D col ){
		// Nur wenn ein Spieler den Save Point erreicht, wird dieser im GameOver Screen sichtbar
		if ( col.gameObject.tag.Equals("Player") ){
			// Ziel aktivieren
			TargetReloadGameObject.SetActive(true);

			// Debug.Log ("Save Number: " + SaveStateForScript);

			GameObject obj = GameObject.Find("Monitor");
			if ( SaveStateForScript == 1) { 
				obj.GetComponent<Monitor>().SaveListsForFirstSaveLocation(); 
			}
			if ( SaveStateForScript == 2) { 
				obj.GetComponent<Monitor>().SaveListsForSecondSaveLocation();
			}

			// Dieses Skript deaktivieren
			GetComponent<ActivateSavePoint>().enabled = false;
			// Dieses GameObjekt zumindest mal unsichtbar machen
			this.gameObject.SetActive(false);
		}
	}

}
