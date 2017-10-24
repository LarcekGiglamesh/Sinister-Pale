#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour {

	public GameObject Player;
	public GameObject StartPosition;
	public Camera ScrollingCamera;
	public Camera DeathCamera;

	private bool boolSwitchOnce = true;
	//private GUI label;

	public GameObject RestartLevel;
	public GameObject SelectSavePoint_01;
	public GameObject SelectSavePoint_02;

	public GameObject returnToMenu;
	public GameObject quitGame;

	public GameObject SavePointLocation_01;
	public GameObject SavePointLocation_02;

	private GameObject ScoreLetter01;
	private GameObject ScoreLetter02;
	private GameObject ScoreLetter03;
	private GameObject ScoreLetter04;
	private GameObject ScoreLetter05;
	private GameObject ScoreLetter06;
	private GameObject HighScoreLetter01;
	private GameObject HighScoreLetter02;
	private GameObject HighScoreLetter03;
	private GameObject HighScoreLetter04;
	private GameObject HighScoreLetter05;
	private GameObject HighScoreLetter06;

	public Sprite[] colorSprites;

	void FixedUpdate(){

		int curScore = Player.GetComponent<PlayerLevelController> ().getCurrentScore ();

		Sprite[] currentScore = getTexturesForScore (curScore);

		ScoreLetter01.GetComponent<SpriteRenderer> ().sprite = currentScore [0];
		ScoreLetter02.GetComponent<SpriteRenderer> ().sprite = currentScore [1];
		ScoreLetter03.GetComponent<SpriteRenderer> ().sprite = currentScore [2];
		ScoreLetter04.GetComponent<SpriteRenderer> ().sprite = currentScore [3];
		ScoreLetter05.GetComponent<SpriteRenderer> ().sprite = currentScore [4];
		ScoreLetter06.GetComponent<SpriteRenderer> ().sprite = currentScore [5];

		// Globalen Highscore aus PlayerPrefs lesen
		int globalHighScore = PlayerPrefs.GetInt ("GlobalHighScore");
		// Globalen Rekord anpassen wenn notwendig
		globalHighScore = curScore > globalHighScore ? curScore : globalHighScore;
		// Globalen Highscore anpassen
		PlayerPrefs.SetInt ("GlobalHighScore", globalHighScore);

		currentScore = getTexturesForScore (globalHighScore);
		HighScoreLetter01.GetComponent<SpriteRenderer> ().sprite = currentScore [0];
		HighScoreLetter02.GetComponent<SpriteRenderer> ().sprite = currentScore [1];
		HighScoreLetter03.GetComponent<SpriteRenderer> ().sprite = currentScore [2];
		HighScoreLetter04.GetComponent<SpriteRenderer> ().sprite = currentScore [3];
		HighScoreLetter05.GetComponent<SpriteRenderer> ().sprite = currentScore [4];
		HighScoreLetter06.GetComponent<SpriteRenderer> ().sprite = currentScore [5];
	}

	private Sprite[] getTexturesForScore( int score ){

		Sprite[] spritedNumber = new Sprite[6];

		if (score < 100000) {
			spritedNumber[5] = colorSprites[0];
		}
		if (score < 10000) {
			spritedNumber[4] = colorSprites[0];
		}
		if (score < 1000) {
			spritedNumber[3] = colorSprites[0];
		}
		if (score < 100) {
			spritedNumber[2] = colorSprites[0];
		}
		if (score < 10) {
			spritedNumber[1] = colorSprites[0];
		}
		if (score < 1) {
			spritedNumber[0] = colorSprites[0];
		}

		int curCheck = 0;
		for (int i = 5; i >= 0; i--) {
			curCheck = score % 10;
			spritedNumber[i] = colorSprites[curCheck];
			score /= 10;
		}

		return spritedNumber;
	}

	// Use this for initialization
	void Start () {
		DeathCamera.camera.enabled = false;

		SavePointLocation_01 = GameObject.Find("RespawnLocation01");
		SavePointLocation_02 = GameObject.Find("RespawnLocation02");

		ScoreLetter01 = GameObject.Find ("ScoreDisplayLocation01");
		ScoreLetter02 = GameObject.Find ("ScoreDisplayLocation02");
		ScoreLetter03 = GameObject.Find ("ScoreDisplayLocation03");
		ScoreLetter04 = GameObject.Find ("ScoreDisplayLocation04");
		ScoreLetter05 = GameObject.Find ("ScoreDisplayLocation05");
		ScoreLetter06 = GameObject.Find ("ScoreDisplayLocation06");

		HighScoreLetter01 = GameObject.Find ("HighScoreDisplayLocation01");
		HighScoreLetter02 = GameObject.Find ("HighScoreDisplayLocation02");
		HighScoreLetter03 = GameObject.Find ("HighScoreDisplayLocation03");
		HighScoreLetter04 = GameObject.Find ("HighScoreDisplayLocation04");
		HighScoreLetter05 = GameObject.Find ("HighScoreDisplayLocation05");
		HighScoreLetter06 = GameObject.Find ("HighScoreDisplayLocation06");
	}

	void OnGUI(){
		//GUI.DrawTexture(new Rect(800, 300, 100, 200), 
	}

	private GameObject CastRay() {
		Ray ray = DeathCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
		if (hit) {
			return hit.collider.gameObject;
		}
		return null;
	}

	// Update is called once per frame
	void Update () {
	
		if ( Player.activeSelf ){
			return;
		}

		// Spieler tot ab dieser Position
		if (boolSwitchOnce) {
			boolSwitchOnce = false;

			ScrollingCamera.camera.enabled = false;
			DeathCamera.camera.enabled = true;
		}

		if (Input.GetMouseButtonDown (0)) {

			GameObject objectHitByCastRay = CastRay();

			if ( objectHitByCastRay == RestartLevel ){
				boolSwitchOnce = true;
				
				ScrollingCamera.camera.enabled = true;
				DeathCamera.camera.enabled = false;
				
				Application.LoadLevel(Application.loadedLevel);

				return;
			}

			if ( objectHitByCastRay == returnToMenu ){

				Application.LoadLevel ("MainMenu");

				return;
			}

			if ( objectHitByCastRay == quitGame ){

				Application.Quit();

				return;

			}

			if ( objectHitByCastRay == SelectSavePoint_01 ){
				boolSwitchOnce = true;
				
				ScrollingCamera.camera.enabled = true;
				DeathCamera.camera.enabled = false;
				
				// Application.LoadLevel(Application.loadedLevel);
				Player.SetActive(true);

				SavePointLocation_01 = GameObject.Find("RespawnLocation01");
				Player.transform.position = SavePointLocation_01.transform.position;

				GameObject monitorObject = GameObject.Find("Monitor");
				monitorObject.GetComponent<Monitor>().restartToSaveLocation(1);

				return;
			}

			if ( objectHitByCastRay == SelectSavePoint_02 ){
				boolSwitchOnce = true;
				
				ScrollingCamera.camera.enabled = true;
				DeathCamera.camera.enabled = false;
				
				// Application.LoadLevel(Application.loadedLevel);
				Player.SetActive(true);

				SavePointLocation_02 = GameObject.Find("RespawnLocation02");
				Player.transform.position = SavePointLocation_02.transform.position;

				GameObject monitorObject = GameObject.Find("Monitor");
				monitorObject.GetComponent<Monitor>().restartToSaveLocation(2);

				return;
			}

		}

	}
}
