#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerLevelController : MonoBehaviour {

	[Tooltip("Aktuell gueltige Maximalgeschwindigkeit.")]
	public float currentMaxSpeed = 80.0f;						// aktuell gueltige maximalgeschwindigkeit (fuer: laufen, rennen, kriechen)
	[Tooltip("Blickrichtung nach Rechts?")]	
	private bool isFacingRight = true;							// blickrichtung (u.a. fuer animation und bewegung)
	[Tooltip("Benoetigt Werte in Hunderter Bereichen")]
	public float jumpForce = 700.0f;							// newton'sche kraft nach oben fuer einen sprung
	[Tooltip("Benoetigt Werte in Hunderter Bereichen")]
	public float jumpForceHorizontal = 900.0f;
	[Tooltip("Benoetigt Werte, Absprung nach Gegner Kill")]
	public float jumpForceAfterEnemyKill = 13000.0f;
	[Tooltip("Benoetigt Werte, Absprung nach Gegner Kill Horizontal")]
	public float jumpForceAfterEnemyKillHorizontal = 13000.0f;

	[Tooltip("Spieler befindet sich am Boden?")]
	private bool isGrounded = true;								// solange true: sprung erlaubt
	[Tooltip("Muss den Spieler beinhalten")]
	public Transform groundCheck;								// repraesentiert das Charakter Sprite
	public float groundRadius = 5.0f;							// Reichweite zur Ueberpruefung gueltiger Absprung Zonen
	[Tooltip("Gueltige Absprung Layer")]
	public LayerMask whatIsGround;								// Definition einer Gruppe gueltiger Absprung Zonen

	[Tooltip("Wird als Kontrolle ausgegeben")]
	public bool characterIsRunning = false;						// zur Identifizierung ob der Character rennt
	private bool characterDoJump = false;						// FixedUpdate sollte AddForce ausfuehren

	[Tooltip("Textur fuer die Score Anzeige im Spiel fuer das kleine Licht")]
	public Texture textureSmallLight;
	[Tooltip("Textur fuer die Score Anzeige im Spiel fuer das grosse Licht")]
	public Texture textureBigLight;
	[Tooltip("Halbtransparente Textur (Vignette Effekt)")]
	public Texture cameraEffectForLives;
	// Eine gesicherte Liste (typ eher uninteressant) fuer die Darstellung der kleinen Score Lichter
	private List<Vector3> listSmallLights;
	// Anzahl gesammelter kleinen Lichter fuer die Anzeige
	private int numberOfSmallLightsCollected = 0;
	// Eine gesicherte Liste (typ eher uninteressant) fuer die Darstellung der grossen Score Lichter
	private List<Vector3> listBigLights;
	// Anzahl gesammelter grossen Lichter fuer die Anzeige
	private int numberOfBigLightsCollected = 0;

	// Animator des Spielers, zustaendig fuer die Darstellung von Animationen
	private Animator anim;

	[Tooltip("Wartezeit fuer Krafteinwirkung nach Sprung")]
	public float WaitForSecondsValue = 0.0f;
	[Tooltip("Krafteinwirkung nach Sprung - Vertikal")]
	public float gravityForceAfterJumpVertical = 0.0f;
	[Tooltip("Krafteinwirkung nach Sprung gegen Blickrichtung - Horizontal")]
	public float gravityForceAfterJumpHorizontal = 0.0f;

	// aenderungsmerkmal fuer die Dunkelheitsueberlagerung
	private float changeDarkness;
	[Tooltip("Dunkelheitsfaktor (0-20)")]
	public int DarknessScale = -1;
	[Tooltip("Aktueller Score; Debug Ausgabemaske")]
	public int CurrentScore = 0;
	[Tooltip("Level Zeit")]
	public float levelTimer = 10.0f;
	// Abfrage fuer Time.time
	private float nextTime = 0.0f;
	// Naechste Abfrage um diese Rate erweitern
	private float nextRate = 0.5f;

	[Tooltip("Anzahl gewonnener Sekunden wenn ein Gegner erledigt wird.")]
	public float TimeGainedForEnemyKill = 0.5f;
	[Tooltip("Anzahl verlorener Sekunden wenn der Spieler Schaden durch Gegener nimmt.")]
	public float TimeLostForEnemyHitTaken = 5.0f;
	[Tooltip("Anzahl gewonnener Sekunden fuer das Sammeln eines Kleinen Lichtes")]
	public float TimeGainedForCollectingSmallLight = 1.5f;
	[Tooltip("Anzahl gewonnener Sekunden fuer das Sammeln eines Großen Lichtes")]
	public float TimeGainedForCollectingBigLight = 3.0f;

	// Game Objekte im Array fuer die Verdunklung
	private GameObject colorPlayerObject;
	private GameObject[] colorSmallLightObjects;
	private GameObject[] colorBigLightObjects;
	private GameObject[] colorEnemiesObjects;
	private GameObject[] colorBackgroundObjects;

	// Audio Clips zum Abspielen
	public AudioClip collectLight01;
	public AudioClip collectLight02;
	public AudioClip collectLight03;
	public AudioClip playerIsHurt01;
	public AudioClip playerIsHurt02;
	public AudioClip playerIsHurt03;
	public AudioClip playerDoJump01;
	public AudioClip playerDoJump02;
	public AudioClip playerDoJump03;
	public AudioClip sinisterOneSound;
	public AudioClip sinisterTwoSound;

	// Bewegung der Lichter im Score
	private List<float> smallLightOffset = null;
	private List<float> smallLightOffsetStep = null;
	private List<float> bigLightOffset = null;
	private List<float> bigLightOffsetStep = null;
	private const float maxOffsetY = 4.0f;
	private const float minOffsetY = -4.0f;

	// Objekte fuer den Pausen Modus
	// Halb Transparente Grafik
	public Texture pauseTexture;
	// Umschalter fuer die Pause
	public bool pauseModeOn = false;
	// Umschalter um die GameObjekte nach Verlassen der Pause einmalig wieder zu aktivieren
	private bool enableAfterPause = false;
	// sichert alle zu pausierenden Einheiten
	private GameObject[] pausedUnits;
	// sichert alle zu pausierenden Plattformen
	private GameObject[] pausedPlattforms;
	// Zur Kollisionsabfrage existieren GameObjekte mitsamt Collider
	// Zurueck zum Hauptmenu
	public GameObject OnPause_GotoMenu;
	// Spiel beenden
	public GameObject OnPause_ExitGame;

	// Maximaler Wert fuer die vertikale Geschwindigkeit
	private const float maxSpeedVertical = 230.0f;	//Gemessen wurden: 225.5726f


	private bool isOnPlattform = false;
	// Setter fuer Plattform Angabe
	public void setIsOnPlattform( bool set )	{	this.isOnPlattform = set;	}
	// Getter fuer Plattform Angabe
	public bool getIsOnPlattform()				{	return this.isOnPlattform;	}

	void Awake() {

		colorPlayerObject = gameObject;
		colorSmallLightObjects = GameObject.FindGameObjectsWithTag ("LightSmall");
		colorBigLightObjects = GameObject.FindGameObjectsWithTag ("LightBig");
		colorEnemiesObjects = GameObject.FindGameObjectsWithTag ("Enemy");
		colorBackgroundObjects = GameObject.FindGameObjectsWithTag ("Backgrounds");

	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

		// Listen vorbereiten
		listSmallLights = new List<Vector3>();
		listBigLights = new List<Vector3>();

		// PlayerPrefs Score zuruecksetzen
		PlayerPrefs.SetFloat("Score", 0);

		// Umgebungsvariablen setzen
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

		// Pausenmenu GameObjekte festsetzen
		OnPause_GotoMenu = GameObject.Find ("OnPause_GotoMenu");
		OnPause_GotoMenu.SetActive (false);
		OnPause_ExitGame = GameObject.Find ("OnPause_QuitGame");
		OnPause_ExitGame.SetActive (false);
	}

	// Warte Funktion
	IEnumerator WaitABit(){

		// Gestatte nur Werte zwischen 0.0 und 1.0 Sekunden
		WaitForSecondsValue = ( WaitForSecondsValue > 1.0f ? 1.0f : WaitForSecondsValue );
		WaitForSecondsValue = ( WaitForSecondsValue < 0.0f ? 0.0f : WaitForSecondsValue );
		// Warte die gewuenschte Zeit
		yield return new WaitForSeconds (WaitForSecondsValue);
	}

	// Warte Funktion
	IEnumerator WaitForGivenSeconds(float waitTime){
		// Warte die gewuenschte Zeit
		yield return new WaitForSeconds (waitTime);
	}

	// Jeden Frame
	void FixedUpdate (){
		// Interpretiere Bewegungstaste
		float move = Input.GetAxis ("Horizontal");

		// Sofern ein einzelner Sprung ausgefuehrt werden soll
		if (characterDoJump) {
			// Springe durch eine gegebener Kraft nach oben
			rigidbody2D.AddForce(new Vector2(jumpForceHorizontal, jumpForce));
			// Sprung erfolgt, kein weiterer Sprung moeglich
			characterDoJump = false;
			// Einen zufaelligen Sound abspielen
			SoundManager.instance.RandomizeSfx(playerDoJump01, playerDoJump02, playerDoJump03);
		}

		// Pruefe ob sich der Character nahe dem Boden befindet
		isGrounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		// Sprung Variable fuer die Animation setzen
		anim.SetBool("Ground", isGrounded);
		// Geschwindigkeit uebernehmen
		anim.SetFloat ("Speed", Mathf.Abs(move));
		// Sprung Geschwindigkeit fuer Animation uebernehmen
		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);

		// Debug.Log ("isGrounded: " +isGrounded + " | Move: " + move);

		float verticalVelocity = Mathf.Min( maxSpeedVertical, rigidbody2D.velocity.y);

		// Fuehre eine Geschwindigkeit in Bewegungsrichtung aus, behalte aktuelle vertikale Geschwindigkeit bei
		rigidbody2D.velocity = new Vector2 (move * currentMaxSpeed, verticalVelocity);

		// Sofern (Bewegung nach rechts und Blick nach links) oder (Bewegung nach links und Blick nach rechts): Drehe Charakter um
		if ((move > 0 && !isFacingRight) || (move < 0 && isFacingRight)) {
			// Charakter umdrehen
			Flip();
		}

		// Fixed Update nicht so haeufig wie Update daher hier Hintergruende neu laden
		colorBackgroundObjects = GameObject.FindGameObjectsWithTag ("Backgrounds");
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

	private bool WhileOnPause(){

		// Pause aktiv
		if (pauseModeOn && !enableAfterPause) {
			// Anzahl inaktiv zu setzender Objekte
			int numberOfObjects;		 
			// Alle Gegner als GameObjekte finden
			pausedUnits = GameObject.FindGameObjectsWithTag("Enemy");
			// Anzahl herauslesen
			numberOfObjects = pausedUnits.Length;
			// Alle GameObjekte inaktiv setzen
			for ( int i = 0 ; i < numberOfObjects ; i++ ){
				pausedUnits[i].SetActive(false);
			}
			// Alle Plattformen als GameObjekte finden
			pausedPlattforms = GameObject.FindGameObjectsWithTag("Plattform");
			// Anzahl herauslesen
			numberOfObjects = pausedPlattforms.Length;
			// Alle GameObjekte inaktiv setzen
			for ( int i = 0 ; i < numberOfObjects ; i++ ){
				pausedPlattforms[i].SetActive(false);
			}
			// MenuItems aktivieren
			OnPause_ExitGame.SetActive(true);
			OnPause_GotoMenu.SetActive(true);

			// Sofern Pause beendet wird, einmal die GameObjekte wieder aktiv setzen
			enableAfterPause = true;
		}
		
		if (pauseModeOn && enableAfterPause) {
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), pauseTexture);
			return true;
		}
		
		if (!pauseModeOn && enableAfterPause) {
			enableAfterPause = false;
			
			// Anzahl aktiv zu setzender Objekte
			int numberOfObjects;		 
			// Anzahl herauslesen
			numberOfObjects = pausedUnits.Length;
			// Alle GameObjekte aktiv setzen
			for ( int i = 0 ; i < numberOfObjects ; i++ ){
				pausedUnits[i].SetActive(true);
			}
			// Anzahl herauslesen
			numberOfObjects = pausedPlattforms.Length;
			// Alle GameObjekte aktiv setzen
			for ( int i = 0 ; i < numberOfObjects ; i++ ){
				pausedPlattforms[i].SetActive(true);
			}

			// MenuItems deaktivieren
			OnPause_ExitGame.SetActive(false);
			OnPause_GotoMenu.SetActive(false);
		}

		return false;

	}

	void OnGUI()
	{
		// Solange Pause, aktualisiere die Ingame Uebersicht nicht
		if ( WhileOnPause() ) {
			return;
		}

		// 
		if (levelTimer >= 0.0f){

			float newColor = Mathf.Min (10.0f, levelTimer);
			newColor = 0.75f - ( ( 10.0f - newColor ) * 0.05f );
			// newColor = 0.30f + ( ( 10.0f - newColor ) * 0.05f );
			// Debug.Log ("Setze neue Farbe auf: " + newColor);
			Color temp = new Color(newColor, newColor, newColor);

			foreach( GameObject obj in colorBackgroundObjects ){
				if ( obj != null ){
					obj.GetComponent<SpriteRenderer>().color = temp;
				}
			}
			
			float height = Screen.height;
			float width = Screen.width;

			float tempValue = Mathf.Min (18.0f, levelTimer);
			// tempValue /= 0.000128f;
			tempValue /= 0.05f;
			tempValue /= 200.0f;
			tempValue *= 0.512f;
			tempValue = 1.2f - tempValue;
			// tempValue = Mathf.Min (1.2f - tempValue, 1.0f);
			tempValue = Mathf.Min (tempValue, 1.0f);

			GUI.color = new Color(1.0f, 1.0f, 1.0f, tempValue);
			// Debug.Log("GUI.color: [r|g|b|a] " + GUI.color.r + "|" + GUI.color.g + "|" + GUI.color.b + "|" + GUI.color.a);
			GUI.DrawTexture (new Rect (0, 0, width, height), cameraEffectForLives);
		}

		int scaleOfLights = 15;
		int iRightBorder = (int) ( Screen.width * 0.9 );
		int iNumberOfSmallLightsRows = 0;
		int iNumberPerRow = (int) ( (double) Screen.width * 0.8 ) / scaleOfLights;
		int iCurrentRowPosX, iCurrentRowPosY = scaleOfLights;
		int randomOffsetY = 0;

		float moveBy = 0.1f;
		//bool moveUp = true;

		if ( numberOfSmallLightsCollected > 0 ){
			int iNumberOfRows = ( numberOfSmallLightsCollected / iNumberPerRow ) + 1;

			int iDrawCountInNextLine = ( numberOfSmallLightsCollected >= iNumberPerRow ? iNumberPerRow : numberOfSmallLightsCollected);
			iNumberOfSmallLightsRows += iNumberOfRows;

			int iLightIndex = 0;

			GUI.color = new Color32(255, 255, 255, 255);
			for ( int i = 0 ; i < iNumberOfRows ; i++ ){
				// Debug.Log ("["+debugCounter+"] \tZeichne '"+iDrawCountInNextLine+"' Elemente in Reihe '"+i+"'");
				for ( int j = 0 ; j < iDrawCountInNextLine ; j++ ){
					iCurrentRowPosX = (j+1)*scaleOfLights;
					iCurrentRowPosY = (i*scaleOfLights) - scaleOfLights;

					// Bestimme wie sich das aktuelle Licht bewegen soll

					if ( smallLightOffset[iLightIndex] > maxOffsetY ){
						smallLightOffsetStep[iLightIndex] = -0.1f;
					}else if (smallLightOffset[iLightIndex] < minOffsetY ){
						smallLightOffsetStep[iLightIndex] = 0.1f;
					}
					// Debug.Log ("Licht '"+j+"' von Position '"+smallLightOffset[j]+"' um '"+smallLightOffsetStep[j]+"' verschieben.");

					smallLightOffset[iLightIndex] += smallLightOffsetStep[iLightIndex];
					randomOffsetY = (int) smallLightOffset[iLightIndex];
					iLightIndex++;

					// if ( i == 0 ) 	Debug.Log("Bewege erstes Licht nach: " + smallLightOffset[i]);
					GUI.DrawTexture(new Rect( iRightBorder - iCurrentRowPosX,iCurrentRowPosY+randomOffsetY,64,64), textureSmallLight);
					// GUI.DrawTexture(new Rect( iCurrentRowPosX,iCurrentRowPosY,40,40), textureSmallLight);
				}
				// Debug.Log ("["+debugCounter+"] \tZiehe aus dem aktuellen Pool von '"+iDrawCountInNextLine+"' die Anzahl '"+iNumberPerRow+"' ab. Insgesamt existieren '"+numberOfSmallLightsCollected+"' Lichter.");
				// Ziehe aus dem gesamten Pool die Anzahl bereits gezeigter Lichter ab
				iDrawCountInNextLine = numberOfSmallLightsCollected - ( ( i+1 ) * iNumberPerRow );
				// Sofern mehr Lichter angezeigt werden sollen als moeglich, limitiere die Anzahl, ansonsten nehme sie
				iDrawCountInNextLine = ( iDrawCountInNextLine > iNumberPerRow ? iNumberPerRow : iDrawCountInNextLine );
			}
		}

		if ( numberOfBigLightsCollected > 0 ){
			int iNumberOfRows = ( numberOfBigLightsCollected / iNumberPerRow ) + 1;
			int iDrawCountInNextLine = ( numberOfBigLightsCollected >= iNumberPerRow ? iNumberPerRow : numberOfBigLightsCollected);

			int iLightIndex = 0;

			GUI.color = new Color32(255, 255, 255, 255);
			for ( int i = 0 ; i < iNumberOfRows ; i++ ){
				// Debug.Log ("["+debugCounter+"] \tZeichne '"+iDrawCountInNextLine+"' Elemente in Reihe '"+i+"'");
				for ( int j = 0 ; j < iDrawCountInNextLine ; j++ ){
					iCurrentRowPosX = (j+1)*scaleOfLights;
					iCurrentRowPosY = ((i+iNumberOfSmallLightsRows)*scaleOfLights) - scaleOfLights;

					if ( bigLightOffset[iLightIndex] > maxOffsetY ){
						bigLightOffsetStep[iLightIndex] = -0.1f;
					}else if (bigLightOffset[iLightIndex] < minOffsetY ){
						bigLightOffsetStep[iLightIndex] = 0.1f;
					}
					// Debug.Log ("Licht '"+j+"' von Position '"+smallLightOffset[j]+"' um '"+smallLightOffsetStep[j]+"' verschieben.");
					
					bigLightOffset[iLightIndex] += bigLightOffsetStep[iLightIndex];
					randomOffsetY = (int) bigLightOffset[iLightIndex];
					iLightIndex++;

					GUI.DrawTexture(new Rect(iRightBorder - iCurrentRowPosX,iCurrentRowPosY+randomOffsetY,64,64), textureBigLight);
				}
				// Debug.Log ("["+debugCounter+"] \tZiehe aus dem aktuellen Pool von '"+iDrawCountInNextLine+"' die Anzahl '"+iNumberPerRow+"' ab. Insgesamt existieren '"+numberOfSmallLightsCollected+"' Lichter.");
				// Ziehe aus dem gesamten Pool die Anzahl bereits gezeigter Lichter ab
				iDrawCountInNextLine = numberOfBigLightsCollected - ( ( i+1 ) * iNumberPerRow );
				// Sofern mehr Lichter angezeigt werden sollen als moeglich, limitiere die Anzahl, ansonsten nehme sie
				iDrawCountInNextLine = ( iDrawCountInNextLine > iNumberPerRow ? iNumberPerRow : iDrawCountInNextLine );
			}

		}

		// Debug.Log ("[!] Level Timer steht bei: " + levelTimer);

	}

	IEnumerator WaitForAnimationToFinish(GameObject destroyThisTarget, float waitSeconds)
	{
		Destroy (destroyThisTarget.collider2D);
		yield return new WaitForSeconds(waitSeconds);
		Destroy(destroyThisTarget);
	}

	void OnTriggerEnter2D( Collider2D col ){

		//Debug.Log ("Collision Detected! [target] > " + col.gameObject.ToString());

		// Kollision mit einem Licht
		if (col.tag == "LightSmall" || col.tag == "LightBig") {

			// Debug.Log ("Position des Lichts lag bei: ("+col.transform.position.x+"|"+col.transform.position.y+"|"+col.transform.position.z+")");

			// PlayerPrefs.SetInt("ScoreLightAdd", 0);
			if ( col.gameObject.tag == "LightSmall" ){
				// Debug.Log("Collected Item: LightSmall");
				CurrentScore += 15;
				// Zeichne ein weiteres Kleines Licht
				// Unnoetig, aber ich wollte einen Datentyp sichern
				listSmallLights.Add(col.gameObject.transform.position);
				numberOfSmallLightsCollected++;
				// Leveltimer anheben
				levelTimer += TimeGainedForCollectingSmallLight;

				// Animator finden
				Animator tempLight = col.GetComponentInParent<Animator>();
				// Wert zur Darstellung der Einsammlung aendern
				tempLight.SetBool("smallLightCollected", true);
				// Collider entfernen, 1 Sekunde warten, GameObjekt entfernen
				StartCoroutine (WaitForAnimationToFinish(col.gameObject, 0.8f));

				if (numberOfSmallLightsCollected > 0 && smallLightOffset == null) {
					smallLightOffset = new List<float>();
					smallLightOffsetStep = new List<float>();
				}
				float random = UnityEngine.Random.Range(-4.0f, 4.0f);
				smallLightOffset.Add(random);
				smallLightOffsetStep.Add(0.1f);
				// Debug.Log ("Neues Licht hinzufuegen und mit auftrieb 1.0f an Stelle '"+random+"' setzen.");
			}
			if ( col.gameObject.tag == "LightBig" ){
				// Debug.Log("Collected Item: LightBig");
				CurrentScore += 100;
				// Zeichne ein weiteres Großes Licht
				// Unnoetig, aber ich wollte einen Datentyp sichern
				listSmallLights.Add(col.gameObject.transform.position);		
				numberOfBigLightsCollected++;
				// Leveltimer anheben
				levelTimer += TimeGainedForCollectingBigLight;

				// Animator finden
				Animator tempLight = col.GetComponentInParent<Animator>();
				// Wert zur Darstellung der Einsammlung aendern
				tempLight.SetBool("bigLightCollected", true);
				// Collider entfernen, 1 Sekunde warten, GameObjekt entfernen
				StartCoroutine (WaitForAnimationToFinish(col.gameObject, 0.8f));

				if (numberOfBigLightsCollected > 0 && bigLightOffset == null) {
					bigLightOffset = new List<float>();
					bigLightOffsetStep = new List<float>();
				}
				bigLightOffset.Add(UnityEngine.Random.Range(-4.0f, 4.0f));
				bigLightOffsetStep.Add(0.1f);
			}

			// Einen zufaelligen Sound abspielen
			SoundManager.instance.RandomizeSfx(collectLight01, collectLight02, collectLight03);

			// Maximalwert fuer den Timer erreicht?
			if ( levelTimer > 20.0f ){
				// Unterschied mit Faktor 5 als Punktzahl addieren
				CurrentScore += (int)( Mathf.Round(levelTimer - 20.0f) * 5);
				// 
				levelTimer = 20.0f;

			}


		}
		
	}

	void OnCollisionEnter2D( Collision2D col ){

		// Kollision mit Gegner findet statt?
		if (col.collider.tag == "Enemy") {

			// Obere Y Position des Gegners aus seiner Position und Groeße ermitteln
			float enemyUpperPosition = col.transform.position.y + col.collider.bounds.size.y;
			// Debug.Log ("Gegner obere Y-Position  = "+col.transform.position.y+" + " + col.collider.bounds.size.y + " = " + enemyUpperPosition);
			// Eigene Position in Y bestimmen
			float ownPositionInY = transform.position.y;
			// Blickrichtung des Spielers lesen
			float move = ( isFacingRight ? 1.0f : -1.0f );

			// Sofern die Position des Spielers ueber dem Gegner liegt
			if( ownPositionInY > enemyUpperPosition ){
				// Fuehre eine Kraft nach oben aus
				rigidbody2D.AddForce(new Vector2(jumpForceAfterEnemyKillHorizontal * move, jumpForceAfterEnemyKill));
				// Pralle am Gegner in Blickrichtung und gegebener Kraft nach oben ab
				rigidbody2D.velocity = new Vector2 (move * currentMaxSpeed, rigidbody2D.velocity.y);

				// Einen abspielen
				if ( col.gameObject.name.Equals("SinisterOne") ){
					SoundManager.instance.RandomizeSfx(sinisterOneSound);
				}else{
					SoundManager.instance.RandomizeSfx(sinisterTwoSound);
				}

				// Zerstoere den Gegner
				Destroy (col.gameObject);

				// Timer um 0.5 Sekunden erweitern
				levelTimer += TimeGainedForEnemyKill;
			}else{
				// Aufgrund hoher Gravitaet muss eine Horizontale Kraft deutlich hoeher sein
				// Bestimme die horizontale Kraft entgegen der Blockrichtung
				float negativeHorizontalForce = jumpForceAfterEnemyKill * -5.0f * move;

				// Fuehre eine Kraft entgegen der Blickrichtung und nach oben hin aus
				rigidbody2D.AddForce(new Vector2(negativeHorizontalForce, jumpForceAfterEnemyKill));

				// Timer um 5 Sekunden reduzieren
				levelTimer -= Mathf.Abs (TimeLostForEnemyHitTaken);

				// Einen zufaelligen Sound abspielen
				SoundManager.instance.RandomizeSfx(playerIsHurt01, playerIsHurt02, playerIsHurt03);
			}

			// Wartezeit nach Sprung
			// WaitABit();
			// Entgegenwirkende Kraft
			// rigidbody2D.AddForce(new Vector2(gravityForceAfterJumpHorizontal*move, gravityForceAfterJumpVertical));
		}

	}

	public float getCurrentLevelTimer(){
		return this.levelTimer;
	}

	public void setCurrentLevelTimer( float time ){
		this.levelTimer = time;
	}

	public int getCurrentScore(){
		return this.CurrentScore;
	}

	public void setCurrentScore( int score ){
		this.CurrentScore = score;
	}

	public int returnNumberOfCollectedBigLights(){
		return this.numberOfBigLightsCollected;
	}

	public int returnNumberOfCollectedSmallLights(){
		return this.numberOfSmallLightsCollected;
	}

	public void resetLightCountTo( int numberOfSmallLightsStored, int numberOfBigLightsStored ){

		this.numberOfSmallLightsCollected = numberOfSmallLightsStored;
		this.numberOfBigLightsCollected = numberOfBigLightsStored;

	}


	void CastRay() {
		// Kamera
		Camera thisCam = GameObject.FindWithTag ("MainCamera").GetComponent<Camera>();

		// Ray auf den Bildschirm feuern
		Ray ray = thisCam.ScreenPointToRay (Input.mousePosition);
		// Sofern der Strahl ein Object trifft, dies melden
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);

		// Gehe zum Menu
		if (hit.collider.gameObject == OnPause_GotoMenu) 
		{
			enableAfterPause = true;
			pauseModeOn = false;
			Time.timeScale = 1;

			// Pausenmenu GameObjekte festsetzen
			OnPause_GotoMenu = GameObject.Find ("OnPause_GotoMenu");
			OnPause_GotoMenu.SetActive (false);
			OnPause_ExitGame = GameObject.Find ("OnPause_QuitGame");
			OnPause_ExitGame.SetActive (false);

			Application.LoadLevel("MainMenu");
			return;
		}

		// Beende das Spiel
		if (hit.collider.gameObject == OnPause_ExitGame) 
		{
			Application.Quit();
			return;
		}

	}


	// Update is called once per frame
	void Update () {
	
		//maxSpeedVertical = Mathf.Max ( maxSpeedVertical, rigidbody2D.velocity.y);
		//Debug.Log ("Maximale Geschwindigkeit Vertikal: " + maxSpeedVertical);

		if ( gameObject.transform.parent != null ){
			Vector3 parentVector = gameObject.transform.parent.position;
			// Debug.Log ("Spieler auf Plattform");
			float checkNext = 0.0f;
			Vector3 tempVector;
			Ray checkRay;
			Camera thisCam = GameObject.FindWithTag ("MainCamera").GetComponent<Camera>();
			RaycastHit2D hitRay;
			for ( int i = 0 ; i < 15 ; i++ ){
				checkNext = i * 2.5f;
				tempVector = new Vector3(parentVector.x, parentVector.y + checkNext, parentVector.z);
				checkRay = thisCam.ScreenPointToRay (tempVector);
				hitRay = Physics2D.Raycast (tempVector, checkRay.direction, Mathf.Infinity);

				if ( hitRay.transform != null ){
					// Debug.Log ("Something was Hit!");
					if ( hitRay.collider.gameObject.tag.Equals("LightSmall") ){
						// Debug.Log ("Small Light Hit!");

						// Debug.Log("Collected Item: LightSmall");
						CurrentScore += 15;
						// Zeichne ein weiteres Kleines Licht
						// Unnoetig, aber ich wollte einen Datentyp sichern
						listSmallLights.Add(hitRay.collider.gameObject.transform.position);
						numberOfSmallLightsCollected++;
						// Leveltimer anheben
						levelTimer += TimeGainedForCollectingSmallLight;
						
						// Animator finden
						Animator tempLight = hitRay.collider.gameObject.GetComponentInParent<Animator>();
						// Wert zur Darstellung der Einsammlung aendern
						tempLight.SetBool("smallLightCollected", true);
						// Collider entfernen, 1 Sekunde warten, GameObjekt entfernen
						StartCoroutine (WaitForAnimationToFinish(hitRay.collider.gameObject, 0.8f));
						
						if (numberOfSmallLightsCollected > 0 && smallLightOffset == null) {
							smallLightOffset = new List<float>();
							smallLightOffsetStep = new List<float>();
						}
						float random = UnityEngine.Random.Range(-4.0f, 4.0f);
						smallLightOffset.Add(random);
						smallLightOffsetStep.Add(0.1f);

						// Einen zufaelligen Sound abspielen
						SoundManager.instance.RandomizeSfx(collectLight01, collectLight02, collectLight03);
						
						break;
					}

					if ( hitRay.collider.gameObject.tag.Equals("LightBig") ){
						// Debug.Log ("Big Light Hit!");

						CurrentScore += 100;
						// Zeichne ein weiteres Großes Licht
						// Unnoetig, aber ich wollte einen Datentyp sichern
						listSmallLights.Add(hitRay.collider.gameObject.transform.position);		
						numberOfBigLightsCollected++;
						// Leveltimer anheben
						levelTimer += TimeGainedForCollectingBigLight;
						
						// Animator finden
						Animator tempLight = hitRay.collider.GetComponentInParent<Animator>();
						// Wert zur Darstellung der Einsammlung aendern
						tempLight.SetBool("bigLightCollected", true);
						// Collider entfernen, 1 Sekunde warten, GameObjekt entfernen
						StartCoroutine (WaitForAnimationToFinish(hitRay.collider.gameObject, 0.8f));
						
						if (numberOfBigLightsCollected > 0 && bigLightOffset == null) {
							bigLightOffset = new List<float>();
							bigLightOffsetStep = new List<float>();
						}
						bigLightOffset.Add(UnityEngine.Random.Range(-4.0f, 4.0f));
						bigLightOffsetStep.Add(0.1f);

						// Einen zufaelligen Sound abspielen
						SoundManager.instance.RandomizeSfx(collectLight01, collectLight02, collectLight03);
						
						break;
					}

				}
			}

			
			// Debug.Log ("Shoot at: ("+tempvector.x+"|"+tempvector.y+"|"+tempvector.z+")");
			// Sofern der Strahl ein Object trifft, dies melden


		}

		if ( rigidbody2D.velocity.y > maxSpeedVertical ){
			// Debug.Log ("Warning: Vertical Character Velocity of '"+rigidbody2D.velocity.y+"' over MAX allowed.");
			rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x, maxSpeedVertical);
		}

		if ( Time.time > nextTime ){
			nextTime = Time.time + nextRate;
			//Debug.Log ("0.5 vergangen! Zeitpunkt '"+Time.time+"'! Naechstes Intervall bei '"+nextTime+"'");

			levelTimer -= 0.5f;
		}

		// Timer erreicht 0?
		if ( levelTimer <= 0.0f ){
			// Um endlosschleife zu entgehn
			levelTimer = -0.1f;
			// Spieler quasi tot
			gameObject.SetActive(false);
		}

		// Pause
		if (Input.GetKeyDown(KeyCode.P))
		{
			if (Time.timeScale == 1)
			{
				pauseModeOn = true;
				Time.timeScale = 0;
			}
			else
			{
				pauseModeOn = false;
				Time.timeScale = 1;
			}
		}

		// Während Pause, LinksKlick in das Pausenmenu
		if (pauseModeOn && enableAfterPause && Input.GetMouseButtonDown (0)) {
			CastRay();
			return;
		}

		// Lauft Definition sollte die AddForce innerhalb der FixedUpdate() auf das Rigidbody ausgefuehrt werden
		// Sofern der Charakter nicht springt und auf dem Boden ist und einen Sprung ausfuehren moechte
		if (!characterDoJump && isGrounded && Input.GetKeyDown("space")) {
			// erlaube Sprung
			characterDoJump = true;
		}

	}
}
