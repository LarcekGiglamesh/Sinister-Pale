#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct levelData{
	public float timer;
	public int score;
	public int smalLightCounter;
	public int bigLightCounter;
}

public struct smallLightObjects{
	public int id;
	public GameObject orbObject;
	public bool isActive;
	public Vector3 objectPosition;
}

public struct bigLightObjects{
	public int id;
	public GameObject orbObject;
	public bool isActive;
	public Vector3 objectPosition;
}

public struct sinisterObjects{
	public int id;
	public GameObject sinisterObj;
	public string sinisterName;
	public Vector3 objectPosition;
	public Vector3 objectScale;
	public float movinginx;
	public float movinginnegativex;
	public float movinginy;
	public float movinginnegativey;
	public float maxmoveinx;
	public float maxnegativemoveinx;
	public float maxmoveiny;
	public float maxnegativemoveiny;

	public float minPositionForPatrouilInX;
	public float maxPositionForPatrouilInX;
	public float minPositionForPatrouilInY;
	public float maxPositionForPatrouilInY;

	public float gravityScale;

	public bool face;

	public int objectEnumValue;
}

public struct platformObjects{
	public GameObject obj;
	public Vector3 objPosition;
	public Vector3 objScale;

	public float movinginx;
	public float movinginnegativex;
	public float movinginy;
	public float movinginnegativey;

	public float maxmoveinx;
	public float maxnegativemoveinx;
	public float maxmoveiny;
	public float maxnegativemoveiny;
	
	public float minPositionForPatrouilInX;
	public float maxPositionForPatrouilInX;
	public float minPositionForPatrouilInY;
	public float maxPositionForPatrouilInY;

	public int objectEnumValue;
}


public class Monitor : MonoBehaviour {

	public GameObject SmallLight_Group;
	public GameObject BigLight_group;
	public GameObject SinisterOne_Group;
	public GameObject SinisterTwo_Group;
	public GameObject Plattform_Group;

	// Globale temporäre Listen
	private List<smallLightObjects> listOfSmallLightObjects = null;
	private List<bigLightObjects> listOfBigLightObjects = null;
	private List<sinisterObjects> listOfSinisterObjects = null;
	private List<platformObjects> listOfPlattformObjects = null;

	// Listen für den ersten SavePoint
	private List<smallLightObjects> listOfSmallLightObjects_FirstSaveLocation = null;
	private List<bigLightObjects> listOfBigLightObjects_FirstSaveLocation = null;
	private List<sinisterObjects> listOfSinisterObjects_FirstSaveLocation = null;
	private List<platformObjects> listOfPlattformObjects_FirstSaveLocation = null;
	private levelData firstSavedLevelData;

	// Listen für den zweiten SavePoint
	private List<smallLightObjects> listOfSmallLightObjects_SecondSaveLocation = null;
	private List<bigLightObjects> listOfBigLightObjects_SecondSaveLocation = null;
	private List<sinisterObjects> listOfSinisterObjects_SecondSaveLocation = null;
	private List<platformObjects> listOfPlattformObjects_SecondSaveLocation = null;
	private levelData secondSavedLevelData;

	// Listen aller Objekte zum SpielStart
	private List<smallLightObjects> startListOfSmallLightObjects = null;
	private List<bigLightObjects> startListOfBigLightObjects = null;
	private List<sinisterObjects> startListOfSinisterObjects = null;

	private int index;
	private const int SMALL_LIGHT_INDEX_START = 0;
	private const int BIG_LIGHT_INDEX_START = 500;
	private const int SINISTERS_INDEX_START = 1000;
	private const int PLATTFORM_INDEX_START = 5000;

	/***************************************************************
	 * 				Gruppe "Kleine Lichter"
	 * *************************************************************/

	public void setSmallLightObjectList(){

		// Maximum der Zahlvariable
		int iNumberOfElements;
		// Alle GameObjekte im Zwischenspeicher
		GameObject[] tempObjects;
		// Index Wert auf die jeweilige Konstante anpassen
		index = SMALL_LIGHT_INDEX_START;
		// Liste neu aufbauen
		listOfSmallLightObjects = new List<smallLightObjects>();
		// Finde alle GameObjekte mit dem gleichem TAG
		tempObjects = GameObject.FindGameObjectsWithTag("LightSmall");
		// Lese die Anzahl an Elementen aus
		iNumberOfElements = tempObjects.Length;
		// Temporaeres Element
		smallLightObjects tempSmall;
		// Iteriere ueber die Anzahl gefundener Elemente
		for ( int i = 0 ; i < iNumberOfElements ; i++ ){
			// Neues Element erstellen
			tempSmall = new smallLightObjects();
			// Index zur eindeutigen Zuweisung
			tempSmall.id = index;
			// Game Objekt sichern
			tempSmall.orbObject = tempObjects[i];
			// (In)Aktives Element
			tempSmall.isActive = tempObjects[i].activeSelf;
			// Aktuelle Position
			tempSmall.objectPosition = tempObjects[i].transform.position;
			// Collider 
			// tempSmall.objectCircleCollider2d = tempObjects[i].collider2D as CircleCollider2D;
			// Animator
			// tempSmall.objectAnimator = tempObjects[i].GetComponent<Animator>();
			// Sprite Renderer
			// tempSmall.objectRenderer = tempObjects[i].GetComponent<SpriteRenderer>();
			// Fuege das Element der Liste kleiner Lichter hinzu
			listOfSmallLightObjects.Add(tempSmall);

			// Index erhoehen
			index++;
		}

	}

	public List<smallLightObjects> getSmallLightObjectList(){
		return this.listOfSmallLightObjects;
	}

	/***************************************************************
	 * 				Gruppe "Große Lichter"
	 * *************************************************************/

	public void setBigLightObjectList(){
		// Maximum der Zahlvariable
		int iNumberOfElements;
		// Alle GameObjekte im Zwischenspeicher
		GameObject[] tempObjects;
		// Index Wert auf die jeweilige Konstante anpassen
		index = BIG_LIGHT_INDEX_START;
		// Liste neu aufbauen
		listOfBigLightObjects = new List<bigLightObjects>();
		// Finde alle GameObjekte mit dem gleichem TAG
		tempObjects = GameObject.FindGameObjectsWithTag("LightBig");
		// Lese die Anzahl an Elementen aus
		iNumberOfElements = tempObjects.Length;
		// Temporaeres Element
		bigLightObjects tempBig;
		// Iteriere ueber die Anzahl gefundener Elemente
		for ( int i = 0 ; i < iNumberOfElements ; i++ ){
			// Neues Element erstellen
			tempBig = new bigLightObjects();
			// Index zur eindeutigen Zuweisung
			tempBig.id = index;
			// (In)Aktives Element
			tempBig.isActive = tempObjects[i].activeSelf;
			// Game Objekt sichern
			tempBig.orbObject = tempObjects[i];
			// Aktuelle Position
			tempBig.objectPosition = tempObjects[i].transform.position;
			// Collider 
			// tempBig.objectCircleCollider2d = (CircleCollider2D) tempObjects[i].collider2D;
			// Animator
			// tempBig.objectAnimator = tempObjects[i].GetComponent<Animator>();
			// Sprite Renderer
			// tempBig.objectRenderer = tempObjects[i].GetComponent<SpriteRenderer>();
			// Fuege das Element der Liste kleiner Lichter hinzu
			listOfBigLightObjects.Add(tempBig);

			// Index erhoehen
			index++;
		}
	}

	public List<bigLightObjects> getBigLightObjectList(){
		return this.listOfBigLightObjects;
	}


	/***************************************************************
	 * 				Gruppe "Plattform"
	 * *************************************************************/
	
	public void setPlattformObjectList(){
		
		// Maximum der Zahlvariable
		int iNumberOfElements;
		// Alle GameObjekte im Zwischenspeicher
		GameObject[] tempObjects;
		// Liste neu aufbauen
		listOfPlattformObjects = new List<platformObjects>();
		// Finde alle GameObjekte mit dem gleichem TAG
		tempObjects = GameObject.FindGameObjectsWithTag("Plattform");
		// Lese die Anzahl an Elementen aus
		iNumberOfElements = tempObjects.Length;
		// Temporaeres Element
		platformObjects tempPlattform;
		// Iteriere ueber die Anzahl gefundener Elemente
		for ( int i = 0 ; i < iNumberOfElements ; i++ ){
			// Neues Element erstellen
			tempPlattform = new platformObjects();
			// Aktuelle Position
			tempPlattform.objPosition = tempObjects[i].transform.position;
			// GameObjekt sichern
			tempPlattform.obj = tempObjects[i];
			// Skalierung sichern
			tempPlattform.objScale = tempObjects[i].transform.localScale;
			// Werte fuer das Skript
			PlattformMovement script = tempObjects[i].GetComponent<PlattformMovement>();
			tempPlattform.movinginx = script.MovingInX;
			tempPlattform.movinginnegativex = script.MovingInNegativeX;
			tempPlattform.movinginy = script.MovingInY;
			tempPlattform.movinginnegativey = script.MovingInNegativeY;
			tempPlattform.maxmoveinx = script.maxMovementInX;
			tempPlattform.maxnegativemoveinx = script.maxNegativeMovementInX;
			tempPlattform.maxmoveiny = script.maxMovementInY;
			tempPlattform.maxnegativemoveiny = script.maxNegativeMovementInY;
			// Wurde zuvor nicht korrekt geschrieben, daher auslesen
			tempPlattform.minPositionForPatrouilInX = script.minPositionForX;
			tempPlattform.maxPositionForPatrouilInX = script.maxPositionForX;
			tempPlattform.minPositionForPatrouilInY = script.minPositionForY;
			tempPlattform.maxPositionForPatrouilInY = script.maxPositionForY;

			// Enumerator
			tempPlattform.objectEnumValue = (int) tempObjects[i].GetComponent<PlattformMovement>().platMoveBehave;
			// Fuege das Element der Liste kleiner Lichter hinzu
			listOfPlattformObjects.Add(tempPlattform);
		}	
	}
	
	public List<platformObjects> getPlattformObjectList(){
		return this.listOfPlattformObjects;
	}



	/***************************************************************
	 * 				Gruppe "Sinister One"
	 * *************************************************************/

	public void setSinisterOneObjectList(){
		
		// Maximum der Zahlvariable
		int iNumberOfElements;
		// Alle GameObjekte im Zwischenspeicher
		GameObject[] tempObjects;
		// Index Wert auf die jeweilige Konstante anpassen
		index = SINISTERS_INDEX_START;
		// Liste neu aufbauen
		listOfSinisterObjects = new List<sinisterObjects>();
		// Finde alle GameObjekte mit dem gleichem TAG
		tempObjects = GameObject.FindGameObjectsWithTag("Enemy");
		// Lese die Anzahl an Elementen aus
		iNumberOfElements = tempObjects.Length;
		// Temporaeres Element
		sinisterObjects tempSinister;
		// Iteriere ueber die Anzahl gefundener Elemente
		for ( int i = 0 ; i < iNumberOfElements ; i++ ){
			// Neues Element erstellen
			tempSinister = new sinisterObjects();
			// Index zur eindeutigen Zuweisung
			tempSinister.id = index;
			// Aktuelle Position
			tempSinister.objectPosition = tempObjects[i].transform.position;
			// Debug.Log("Sichere Sinister an Position ("+tempObjects[i].transform.position.x+"|"+tempObjects[i].transform.position.y+"|"+tempObjects[i].transform.position.z+")");
			// Name merken
			tempSinister.sinisterName = tempObjects[i].name;
			// Debug.Log("Sichere den Namen '"+tempObjects[i].name+"' als Element '"+i+"' in der aktuellen Liste.");
			// GameObjekt sichern
			tempSinister.sinisterObj = tempObjects[i];
			// Skalierung sichern
			tempSinister.objectScale = tempObjects[i].transform.localScale;
			// Werte fuer das Skript
			EnemyMovement script = tempObjects[i].GetComponent<EnemyMovement>();
			tempSinister.movinginx = script.MovingInX;
			tempSinister.movinginnegativex = script.MovingInNegativeX;
			tempSinister.movinginy = script.MovingInY;
			tempSinister.movinginnegativey = script.MovingInNegativeY;
			tempSinister.maxmoveinx = script.maxMovementInX;
			tempSinister.maxnegativemoveinx = script.maxNegativeMovementInX;
			tempSinister.maxmoveiny = script.maxMovementInY;
			tempSinister.maxnegativemoveiny = script.maxNegativeMovementInY;
			// Wurde zuvor nicht korrekt geschrieben, daher auslesen
			tempSinister.minPositionForPatrouilInX = script.minPositionForX;
			tempSinister.maxPositionForPatrouilInX = script.maxPositionForX;
			tempSinister.minPositionForPatrouilInY = script.minPositionForY;
			tempSinister.maxPositionForPatrouilInY = script.maxPositionForY;
			// Blickrichtung
			tempSinister.face = script.isFacingRight;
			// Gravitation
			if ( tempObjects[i].rigidbody2D != null ){
				tempSinister.gravityScale = tempObjects[i].rigidbody2D.gravityScale;
			}
			// Enumerator
			tempSinister.objectEnumValue = (int) tempObjects[i].GetComponent<EnemyMovement>().enemyMovePattern;
			// Fuege das Element der Liste kleiner Lichter hinzu
			listOfSinisterObjects.Add(tempSinister);

			// Index erhoehen
			index++;
		}	
	}

	public List<sinisterObjects> getSinisterObjectList(){
		return this.listOfSinisterObjects;
	}

	/**************************************************************************
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * ************************************************************************/

	// Zu Beginn sichere alle Objekte
	void Start(){
		setSmallLightObjectList();
		setBigLightObjectList();
		setSinisterOneObjectList();

		startListOfSmallLightObjects = getSmallLightObjectList();
		startListOfBigLightObjects = getBigLightObjectList();
		startListOfSinisterObjects = getSinisterObjectList();
	}

	public void SaveListsForFirstSaveLocation(){
		if (listOfSmallLightObjects_FirstSaveLocation != null)	return;

		setSmallLightObjectList();
		setBigLightObjectList();
		setSinisterOneObjectList();
		setPlattformObjectList ();
		
		listOfSmallLightObjects_FirstSaveLocation = getSmallLightObjectList();
		listOfBigLightObjects_FirstSaveLocation = getBigLightObjectList();
		listOfSinisterObjects_FirstSaveLocation = getSinisterObjectList();
		listOfPlattformObjects_FirstSaveLocation = getPlattformObjectList ();

		GameObject player = GameObject.Find("Player");
		PlayerLevelController plcScript = player.GetComponent<PlayerLevelController>();

		firstSavedLevelData = new levelData ();
		firstSavedLevelData.score = plcScript.getCurrentScore();
		firstSavedLevelData.timer = plcScript.getCurrentLevelTimer();
		firstSavedLevelData.smalLightCounter = plcScript.returnNumberOfCollectedSmallLights ();
		firstSavedLevelData.bigLightCounter = plcScript.returnNumberOfCollectedBigLights ();
	}

	public void SaveListsForSecondSaveLocation(){
		if (listOfSmallLightObjects_SecondSaveLocation != null)	return;

		setSmallLightObjectList();
		setBigLightObjectList();
		setSinisterOneObjectList();
		setPlattformObjectList ();
		
		listOfSmallLightObjects_SecondSaveLocation = getSmallLightObjectList();
		listOfBigLightObjects_SecondSaveLocation = getBigLightObjectList();
		listOfSinisterObjects_SecondSaveLocation = getSinisterObjectList();
		listOfPlattformObjects_SecondSaveLocation = getPlattformObjectList ();

		GameObject player = GameObject.Find("Player");
		PlayerLevelController plcScript = player.GetComponent<PlayerLevelController>();

		secondSavedLevelData = new levelData ();
		secondSavedLevelData.score = plcScript.getCurrentScore();
		secondSavedLevelData.timer = plcScript.getCurrentLevelTimer();
		secondSavedLevelData.smalLightCounter = plcScript.returnNumberOfCollectedSmallLights ();
		secondSavedLevelData.bigLightCounter = plcScript.returnNumberOfCollectedBigLights ();
	}


	// Lade die Objekte neu. Liefert TRUE wenn erfolgreich, sonst FALSE 
	public bool restartToSaveLocation( int saveLocation ){
		// Lade Speicherpunkt 1 oder 2
		if ( saveLocation < 1 || saveLocation > 2 ){
			return false;
		}



		if ( saveLocation == 1 ){

			/* *******************************************************************
			 * 
			 * 	Lade die Kleinen Lichter neu
			 * 	zerstoere dafuer zuerst alle vorhandenen kleinen Lichter
			 * 
			 * ******************************************************************* */

			// Neues Licht Objekt
			GameObject newLightOrb;

			// Sichere eine Neue Liste an kleinen Lichtern
			setSmallLightObjectList();
			// Maximale Anzahl feststellen
			int iMaxNumber = listOfSmallLightObjects.Count;
			// Alle Elemente zerstören
			for ( int i = 0 ; i < iMaxNumber ; i++ ){
				Destroy(listOfSmallLightObjects[i].orbObject);
			}
			// zaehle die zahl an die -zu dem damaligen zeitpunkt existierten- kleinen Lichter aus
			int numberOfElements = listOfSmallLightObjects_FirstSaveLocation.Count;
			// iteriere durch die Objekte
			for ( int i = 0 ; i < numberOfElements ; i++ ){
				newLightOrb = (GameObject) Instantiate(Resources.Load("SmallLight"));
				newLightOrb.transform.position = listOfSmallLightObjects_FirstSaveLocation[i].objectPosition;

				newLightOrb.transform.parent = SmallLight_Group.transform;
			}

			/* *******************************************************************
			 * 
			 * 	Lade die Großen Lichter neu
			 * 	zerstoere dafuer zuerst alle vorhandenen großen Lichter
			 * 
			 * ******************************************************************* */


			// Sichere eine Neue Liste an kleinen Lichtern
			setBigLightObjectList();
			// Maximale Anzahl feststellen
			iMaxNumber = listOfBigLightObjects.Count;
			// Alle Elemente zerstören
			for ( int i = 0 ; i < iMaxNumber ; i++ ){
				Destroy(listOfBigLightObjects[i].orbObject);
			}
			// zaehle die zahl an die -zu dem damaligen zeitpunkt existierten- großen Lichter aus
			numberOfElements = listOfBigLightObjects_FirstSaveLocation.Count;
			// iteriere durch die Objekte
			for ( int i = 0 ; i < numberOfElements ; i++ ){
				newLightOrb = (GameObject) Instantiate(Resources.Load("BigLight"));
				newLightOrb.transform.position = listOfBigLightObjects_FirstSaveLocation[i].objectPosition;

				newLightOrb.transform.parent = BigLight_group.transform;
			}

			/* *******************************************************************
			 * 
			 * 	Lade die Sinisters (beide Typen) neu
			 * 	zerstoere dafuer zuerst alle vorhandenen Sinisters beider Typen
			 * 
			 * ******************************************************************* */


			GameObject newSinister;
			// Sichere eine Neue Liste an Sinistern
			setSinisterOneObjectList();
			// Maximale Anzahl feststellen
			iMaxNumber = listOfSinisterObjects.Count;
			// Alle Elemente zerstoeren
			for ( int i = 0 ; i < iMaxNumber ; i++ ){
				Destroy(listOfSinisterObjects[i].sinisterObj);
			}
			// zaehle die zahl an die -zu dem damaligen zeitpunkt existierten- großen Lichter aus
			numberOfElements = listOfSinisterObjects_FirstSaveLocation.Count;

			// iteriere durch die Objekte
			for ( int i = 0 ; i < numberOfElements ; i++ ){
				newSinister  = (GameObject) Instantiate(Resources.Load(listOfSinisterObjects_FirstSaveLocation[i].sinisterName));
				newSinister.name = listOfSinisterObjects_FirstSaveLocation[i].sinisterName;
				newSinister.transform.position = listOfSinisterObjects_FirstSaveLocation[i].objectPosition;
				newSinister.transform.localScale = listOfSinisterObjects_FirstSaveLocation[i].objectScale;
				if ( newSinister.rigidbody2D != null ){
					newSinister.rigidbody2D.gravityScale = listOfSinisterObjects_FirstSaveLocation[i].gravityScale;
				}
				EnemyMovement tempScript = newSinister.GetComponent<EnemyMovement>();
				tempScript.enabled = false;

				tempScript.MovingInX = listOfSinisterObjects_FirstSaveLocation[i].movinginx;
				tempScript.MovingInNegativeX = listOfSinisterObjects_FirstSaveLocation[i].movinginnegativex;
				tempScript.MovingInY = listOfSinisterObjects_FirstSaveLocation[i].movinginy;
				tempScript.MovingInNegativeY = listOfSinisterObjects_FirstSaveLocation[i].movinginnegativey;

				tempScript.maxMovementInX = listOfSinisterObjects_FirstSaveLocation[i].maxmoveinx;
				tempScript.maxNegativeMovementInX = listOfSinisterObjects_FirstSaveLocation[i].maxnegativemoveinx;
				tempScript.maxMovementInY = listOfSinisterObjects_FirstSaveLocation[i].maxmoveiny;
				tempScript.maxNegativeMovementInY = listOfSinisterObjects_FirstSaveLocation[i].maxnegativemoveiny;

				tempScript.minPositionForX = listOfSinisterObjects_FirstSaveLocation[i].minPositionForPatrouilInX;
				tempScript.maxPositionForX = listOfSinisterObjects_FirstSaveLocation[i].maxPositionForPatrouilInX;
				tempScript.minPositionForY = listOfSinisterObjects_FirstSaveLocation[i].minPositionForPatrouilInY;
				tempScript.maxPositionForY = listOfSinisterObjects_FirstSaveLocation[i].maxPositionForPatrouilInY;

				tempScript.isFacingRight = listOfSinisterObjects_FirstSaveLocation[i].face;

				tempScript.enemyMovePattern = (EnemyMovement.enemyMovementPattern) listOfSinisterObjects_FirstSaveLocation[i].objectEnumValue;

				tempScript.enabled = true;

				newSinister.transform.parent = ( listOfSinisterObjects_FirstSaveLocation[i].sinisterName.Equals("SinisterOne") ? SinisterOne_Group.transform : SinisterTwo_Group.transform );
			}

			/* *******************************************************************
			 * 
			 * 	Setze die Plattformen zurueck
			 * 	! Keine zerstoerung notwendig, da Plattformen unzerstörbar sind !
			 * 
			 * ******************************************************************* */

			// Plattformen
			numberOfElements = listOfPlattformObjects_FirstSaveLocation.Count;

			// aktuelles GameObjekt
			GameObject curPlatform;
			// iteriere durch alle Objekte
			for ( int i = 0 ; i < numberOfElements ; i++ ){
				curPlatform = listOfPlattformObjects_FirstSaveLocation[i].obj;

				curPlatform.transform.position = listOfPlattformObjects_FirstSaveLocation[i].objPosition;
			}


			GameObject player = GameObject.Find("Player");
			PlayerLevelController plcScript = player.GetComponent<PlayerLevelController>();
			plcScript.setCurrentLevelTimer( firstSavedLevelData.timer );
			plcScript.setCurrentScore( firstSavedLevelData.score );
			plcScript.resetLightCountTo( firstSavedLevelData.smalLightCounter, firstSavedLevelData.bigLightCounter );

			return true;
		}


		if ( saveLocation == 2 ){
			
			/* *******************************************************************
			 * 
			 * 	Lade die Kleinen Lichter neu
			 * 	zerstoere dafuer zuerst alle vorhandenen kleinen Lichter
			 * 
			 * ******************************************************************* */
			
			// Neues Licht Objekt
			GameObject newLightOrb;
			
			// Sichere eine Neue Liste an kleinen Lichtern
			setSmallLightObjectList();
			// Maximale Anzahl feststellen
			int iMaxNumber = listOfSmallLightObjects.Count;
			// Alle Elemente zerstören
			for ( int i = 0 ; i < iMaxNumber ; i++ ){
				Destroy(listOfSmallLightObjects[i].orbObject);
			}
			// zaehle die zahl an die -zu dem damaligen zeitpunkt existierten- kleinen Lichter aus
			int numberOfElements = listOfSmallLightObjects_SecondSaveLocation.Count;
			// iteriere durch die Objekte
			for ( int i = 0 ; i < numberOfElements ; i++ ){
				newLightOrb = (GameObject) Instantiate(Resources.Load("SmallLight"));
				newLightOrb.transform.position = listOfSmallLightObjects_SecondSaveLocation[i].objectPosition;
				
				newLightOrb.transform.parent = SmallLight_Group.transform;
			}
			
			/* *******************************************************************
			 * 
			 * 	Lade die Großen Lichter neu
			 * 	zerstoere dafuer zuerst alle vorhandenen großen Lichter
			 * 
			 * ******************************************************************* */
			
			
			// Sichere eine Neue Liste an kleinen Lichtern
			setBigLightObjectList();
			// Maximale Anzahl feststellen
			iMaxNumber = listOfBigLightObjects.Count;
			// Alle Elemente zerstören
			for ( int i = 0 ; i < iMaxNumber ; i++ ){
				Destroy(listOfBigLightObjects[i].orbObject);
			}
			// zaehle die zahl an die -zu dem damaligen zeitpunkt existierten- großen Lichter aus
			numberOfElements = listOfBigLightObjects_SecondSaveLocation.Count;
			// iteriere durch die Objekte
			for ( int i = 0 ; i < numberOfElements ; i++ ){
				newLightOrb = (GameObject) Instantiate(Resources.Load("BigLight"));
				newLightOrb.transform.position = listOfBigLightObjects_SecondSaveLocation[i].objectPosition;
				
				newLightOrb.transform.parent = BigLight_group.transform;
			}
			
			/* *******************************************************************
			 * 
			 * 	Lade die Sinisters (beide Typen) neu
			 * 	zerstoere dafuer zuerst alle vorhandenen Sinisters beider Typen
			 * 
			 * ******************************************************************* */
			
			
			GameObject newSinister;
			// Sichere eine Neue Liste an Sinistern
			setSinisterOneObjectList();
			// Maximale Anzahl feststellen
			iMaxNumber = listOfSinisterObjects.Count;
			// Alle Elemente zerstoeren
			for ( int i = 0 ; i < iMaxNumber ; i++ ){
				Destroy(listOfSinisterObjects[i].sinisterObj);
			}
			// zaehle die zahl an die -zu dem damaligen zeitpunkt existierten- großen Lichter aus
			numberOfElements = listOfSinisterObjects_SecondSaveLocation.Count;
			
			// iteriere durch die Objekte
			for ( int i = 0 ; i < numberOfElements ; i++ ){
				//Debug.Log ("Name to Load: '"+listOfSinisterObjects_SecondSaveLocation[i].sinisterName+"'");
				newSinister  = (GameObject) Instantiate(Resources.Load(listOfSinisterObjects_SecondSaveLocation[i].sinisterName));
				newSinister.name = listOfSinisterObjects_SecondSaveLocation[i].sinisterName;
				newSinister.transform.position = listOfSinisterObjects_SecondSaveLocation[i].objectPosition;
				newSinister.transform.localScale = listOfSinisterObjects_SecondSaveLocation[i].objectScale;

				if ( newSinister.rigidbody2D != null ){
					newSinister.rigidbody2D.gravityScale = listOfSinisterObjects_SecondSaveLocation[i].gravityScale;
				}

				EnemyMovement tempScript = newSinister.GetComponent<EnemyMovement>();
				tempScript.enabled = false;
				
				tempScript.MovingInX = listOfSinisterObjects_SecondSaveLocation[i].movinginx;
				tempScript.MovingInNegativeX = listOfSinisterObjects_SecondSaveLocation[i].movinginnegativex;
				tempScript.MovingInY = listOfSinisterObjects_SecondSaveLocation[i].movinginy;
				tempScript.MovingInNegativeY = listOfSinisterObjects_SecondSaveLocation[i].movinginnegativey;
				
				tempScript.maxMovementInX = listOfSinisterObjects_SecondSaveLocation[i].maxmoveinx;
				tempScript.maxNegativeMovementInX = listOfSinisterObjects_SecondSaveLocation[i].maxnegativemoveinx;
				tempScript.maxMovementInY = listOfSinisterObjects_SecondSaveLocation[i].maxmoveiny;
				tempScript.maxNegativeMovementInY = listOfSinisterObjects_SecondSaveLocation[i].maxnegativemoveiny;
				
				tempScript.minPositionForX = listOfSinisterObjects_SecondSaveLocation[i].minPositionForPatrouilInX;
				tempScript.maxPositionForX = listOfSinisterObjects_SecondSaveLocation[i].maxPositionForPatrouilInX;
				tempScript.minPositionForY = listOfSinisterObjects_SecondSaveLocation[i].minPositionForPatrouilInY;
				tempScript.maxPositionForY = listOfSinisterObjects_SecondSaveLocation[i].maxPositionForPatrouilInY;
				
				tempScript.isFacingRight = listOfSinisterObjects_SecondSaveLocation[i].face;
				
				tempScript.enemyMovePattern = (EnemyMovement.enemyMovementPattern) listOfSinisterObjects_SecondSaveLocation[i].objectEnumValue;
				
				tempScript.enabled = true;
				
				newSinister.transform.parent = ( listOfSinisterObjects_SecondSaveLocation[i].sinisterName.Equals("SinisterOne") ? SinisterOne_Group.transform : SinisterTwo_Group.transform );
			}
			
			/* *******************************************************************
			 * 
			 * 	Setze die Plattformen zurueck
			 * 	! Keine zerstoerung notwendig, da Plattformen unzerstörbar sind !
			 * 
			 * ******************************************************************* */
			
			// Plattformen
			numberOfElements = listOfPlattformObjects_SecondSaveLocation.Count;
			
			// aktuelles GameObjekt
			GameObject curPlatform;
			// iteriere durch alle Objekte
			for ( int i = 0 ; i < numberOfElements ; i++ ){
				curPlatform = listOfPlattformObjects_SecondSaveLocation[i].obj;
				
				curPlatform.transform.position = listOfPlattformObjects_SecondSaveLocation[i].objPosition;
			}
			
			GameObject player = GameObject.Find("Player");
			PlayerLevelController plcScript = player.GetComponent<PlayerLevelController>();
			plcScript.setCurrentLevelTimer( secondSavedLevelData.timer ); 
			plcScript.setCurrentScore( secondSavedLevelData.score );
			plcScript.resetLightCountTo( secondSavedLevelData.smalLightCounter, secondSavedLevelData.bigLightCounter );

			return true;
		}

		/* Vorangehensweise:
		 * 	Wenn Objekt existiert, editiere es
		 * 	Wenn Objekt nicht existiert, erstelle es vom Prefab und setze Werte
		 */

		return false;
	}
	
}
