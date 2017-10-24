using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GameObject gameObject_MenuStart;
	public GameObject gameObject_MenuSetting;
	public GameObject gameObject_MenuHighScore;
	public GameObject gameObject_MenuGameQuit;

	public Sprite startOriginal;
	public Sprite startAlternate;
	public Sprite controlOriginal;
	public Sprite controlAlternate;
	public Sprite highscoreOriginal;
	public Sprite highscoreAlternate;
	public Sprite exitgameOriginal;
	public Sprite exitgameAlternate;

	private float updateNext = 0.0f;
	private float EverySoOften = 0.2f;

	// Warte, dann beende das Spiel (Applikation+Editor)
	IEnumerator WaitSomeSecondsBeforeEndingGame(float waitSeconds)
	{
		// Warte die vorgegebene Zeit
		yield return new WaitForSeconds(waitSeconds);
		// Spiel als Applikation beenden
		Application.Quit();
		// Spiel im Editor beenden
		// UnityEditor.EditorApplication.isPlaying = false; 
	}

	void CastRay() {
		// Ray auf den Bildschirm feuern
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		// Sofern der Strahl ein Object trifft, dies melden
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);

		// Menu Start 
		if ( hit.collider.gameObject == gameObject_MenuStart ) {
			// Debug.Log (hit.collider.gameObject.name + " hit");
			// Starte das Spiel
			Application.LoadLevel("Final - The Level");
			return;
		}

		// Menu Settings
		if ( hit.collider.gameObject == gameObject_MenuSetting ) {
			// Debug.Log (hit.collider.gameObject.name + " hit");
			return;
		}

		// Menu Highscore
		if ( hit.collider.gameObject == gameObject_MenuHighScore ) {
			// Debug.Log (hit.collider.gameObject.name + " hit");
			Application.LoadLevel("HighscoreMenu");
			return;
		}

		// Menu GameQuit
		if ( hit.collider.gameObject == gameObject_MenuGameQuit ) {
			// Debug.Log (hit.collider.gameObject.name + " hit");
			StartCoroutine(WaitSomeSecondsBeforeEndingGame(1.0f));
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			// Strahl auf die Szene schiessen
			CastRay ();
		}


	}
}
