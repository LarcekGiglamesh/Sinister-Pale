using UnityEngine;
using System.Collections;

public class HighscoreMenu : MonoBehaviour {

	public GameObject gotoMain;
	public GameObject exitGame;

	public GameObject Highscore01_Letter01;
	public GameObject Highscore01_Letter02;
	public GameObject Highscore01_Letter03;
	public GameObject Highscore01_Letter04;
	public GameObject Highscore01_Letter05;
	public GameObject Highscore01_Letter06;

	public GameObject Highscore02_Letter01;
	public GameObject Highscore02_Letter02;
	public GameObject Highscore02_Letter03;
	public GameObject Highscore02_Letter04;
	public GameObject Highscore02_Letter05;
	public GameObject Highscore02_Letter06;

	public GameObject Highscore03_Letter01;
	public GameObject Highscore03_Letter02;
	public GameObject Highscore03_Letter03;
	public GameObject Highscore03_Letter04;
	public GameObject Highscore03_Letter05;
	public GameObject Highscore03_Letter06;

	public GameObject Highscore04_Letter01;
	public GameObject Highscore04_Letter02;
	public GameObject Highscore04_Letter03;
	public GameObject Highscore04_Letter04;
	public GameObject Highscore04_Letter05;
	public GameObject Highscore04_Letter06;

	public GameObject Highscore05_Letter01;
	public GameObject Highscore05_Letter02;
	public GameObject Highscore05_Letter03;
	public GameObject Highscore05_Letter04;
	public GameObject Highscore05_Letter05;
	public GameObject Highscore05_Letter06;

	public Sprite[] numbers;

	void Start(){
		/*
		PlayerPrefs.SetInt("HighscoreTop1", 123456);
		PlayerPrefs.SetInt("HighscoreTop2", 10010);
		PlayerPrefs.SetInt("HighscoreTop3", 900);
		PlayerPrefs.SetInt("HighscoreTop4", 400);
		PlayerPrefs.SetInt("HighscoreTop5", 25);
		*/
	}
	
	void CastRay() {
		// Kamera
		Camera thisCam = GameObject.FindWithTag ("MainCamera").GetComponent<Camera> ();

		// Ray auf den Bildschirm feuern
		Ray ray = thisCam.ScreenPointToRay (Input.mousePosition);
		// Sofern der Strahl ein Object trifft, dies melden
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);

		if (hit.transform == null) {
			return;
		}

		if (hit.collider.gameObject == gotoMain) {
			Application.LoadLevel("MainMenu");
			return;
		}

		if (hit.collider.gameObject == exitGame) {
			Application.Quit();
			return;
		}
	}

	private Sprite[] getTexturesForScore( int score ){
		
		Sprite[] spritedNumber = new Sprite[6];
		int curCheck = 0;
		// string output = "|";
		for (int i = 5; i >= 0; i--) {
			curCheck = score % 10;
			spritedNumber[i] = numbers[curCheck];
			score /= 10;
			// output = output + curCheck + "|";
		}
		// Debug.Log(output);
		
		return spritedNumber;
	}

	void OnGUI(){

		Sprite[] display;
		
		int displayScore5 = PlayerPrefs.GetInt ("HighscoreTop5");
		display = getTexturesForScore (displayScore5);
		Highscore05_Letter01.GetComponent<SpriteRenderer>().sprite = display[0];
		Highscore05_Letter02.GetComponent<SpriteRenderer>().sprite = display[1];
		Highscore05_Letter03.GetComponent<SpriteRenderer>().sprite = display[2];
		Highscore05_Letter04.GetComponent<SpriteRenderer>().sprite = display[3];
		Highscore05_Letter05.GetComponent<SpriteRenderer>().sprite = display[4];
		Highscore05_Letter06.GetComponent<SpriteRenderer>().sprite = display[5];
		// Debug.Log ("Score5: " + displayScore5);
		
		int displayScore4 = PlayerPrefs.GetInt ("HighscoreTop4");
		display = getTexturesForScore (displayScore4);
		Highscore04_Letter01.GetComponent<SpriteRenderer>().sprite = display[0];
		Highscore04_Letter02.GetComponent<SpriteRenderer>().sprite = display[1];
		Highscore04_Letter03.GetComponent<SpriteRenderer>().sprite = display[2];
		Highscore04_Letter04.GetComponent<SpriteRenderer>().sprite = display[3];
		Highscore04_Letter05.GetComponent<SpriteRenderer>().sprite = display[4];
		Highscore04_Letter06.GetComponent<SpriteRenderer>().sprite = display[5];
		// Debug.Log ("Score4: " + displayScore4);
		
		int displayScore3 = PlayerPrefs.GetInt ("HighscoreTop3");
		display = getTexturesForScore (displayScore3);
		Highscore03_Letter01.GetComponent<SpriteRenderer>().sprite = display[0];
		Highscore03_Letter02.GetComponent<SpriteRenderer>().sprite = display[1];
		Highscore03_Letter03.GetComponent<SpriteRenderer>().sprite = display[2];
		Highscore03_Letter04.GetComponent<SpriteRenderer>().sprite = display[3];
		Highscore03_Letter05.GetComponent<SpriteRenderer>().sprite = display[4];
		Highscore03_Letter06.GetComponent<SpriteRenderer>().sprite = display[5];
		// Debug.Log ("Score3: " + displayScore3);
		
		int displayScore2 = PlayerPrefs.GetInt ("HighscoreTop2");
		display = getTexturesForScore (displayScore2);
		Highscore02_Letter01.GetComponent<SpriteRenderer>().sprite = display[0];
		Highscore02_Letter02.GetComponent<SpriteRenderer>().sprite = display[1];
		Highscore02_Letter03.GetComponent<SpriteRenderer>().sprite = display[2];
		Highscore02_Letter04.GetComponent<SpriteRenderer>().sprite = display[3];
		Highscore02_Letter05.GetComponent<SpriteRenderer>().sprite = display[4];
		Highscore02_Letter06.GetComponent<SpriteRenderer>().sprite = display[5];
		// Debug.Log ("Score2: " + displayScore2);
		
		int displayScore1 = PlayerPrefs.GetInt ("HighscoreTop1");
		display = getTexturesForScore (displayScore1);
		Highscore01_Letter01.GetComponent<SpriteRenderer>().sprite = display[0];
		Highscore01_Letter02.GetComponent<SpriteRenderer>().sprite = display[1];
		Highscore01_Letter03.GetComponent<SpriteRenderer>().sprite = display[2];
		Highscore01_Letter04.GetComponent<SpriteRenderer>().sprite = display[3];
		Highscore01_Letter05.GetComponent<SpriteRenderer>().sprite = display[4];
		Highscore01_Letter06.GetComponent<SpriteRenderer>().sprite = display[5];
		// Debug.Log ("Score1: " + displayScore1);

	}


	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButtonDown (0)) {
			CastRay();
		}
	}
}
