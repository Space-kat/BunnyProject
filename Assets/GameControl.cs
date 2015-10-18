using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

	const int UI_Z_AXIS = 2;

	public Level CurrentLevel {
		get {
			return levels[currentLevelIndex];
		}
	}

	public  GameObject UICarrots;
	public  GameObject UISeconds;
	public  Camera 	  GameCamera;
	private TextMesh  UICarrotText, UISecondsText;

	private int currentLevelIndex = -1;
	private List<Level> levels;
	private Boundaries screenBounds;


	// Use this for initialization
	void Start () {
		screenBounds = new Boundaries (GameCamera, GetComponent<Transform>());
		LevelsInit ();
		UISetup ();
		NextLevel ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

	public Level NextLevel(){
		currentLevelIndex++;
		return levels[currentLevelIndex];
	}

	public bool LastLevel(){
		return currentLevelIndex == levels.Count;
	}

	private void UpdateUI(){
		UICarrotText.text = CurrentLevel.Carrots.ToString () + "/" + CurrentLevel.Carrots.ToString ();
		UISecondsText.text = CurrentLevel.Seconds.ToString ();
	}

	private void UISetup(){
		Vector3 topLeft = new Vector3  (GameCamera. .Left + 1, screenBounds.Top - 1, UI_Z_AXIS);
		Vector3 topRight = new Vector3 (screenBounds.Left - 1, screenBounds.Top - 1, UI_Z_AXIS);

		this.UICarrots.transform.position = topLeft;
		this.UISeconds.transform.position = topRight;
		Debug.Log(topLeft);
		Debug.Log (topRight);

		UICarrotText = this.UICarrots.GetComponent<TextMesh> ();
		UISecondsText = this.UISeconds.GetComponent<TextMesh> ();
	}

	private void LevelsInit(){
		levels = new List<Level>()
		{
			new Level (10, 20),
			new Level (14, 20),
			new Level (3, 20)
		};
	}
}

public class Boundaries {

	// Uses Pythagorean and so relies on the game plane and camera being perpendicular to one another

	public int Top, Right, Bottom, Left;

	public Boundaries(Camera camera, Transform planeTransform){

		var cameraZ = camera.transform.position.z;
		var planeY = planeTransform.position.y;


		this.Top = Math.Pow (cameraZ, 2) * Math.Pow (planeY, 2);
		this.Right  =  	width  / 2;
		this.Bottom = 	height / 2;
		this.Left 	=	width  / 2 * -1;
	}

}

public class Level {

	public int Carrots;
	public int Seconds;
	
	public Level(int carrots, int seconds){
		this.Carrots = carrots;
		this.Seconds = seconds;
	}
}