using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterScript : GameControl {

	enum Sprites { Up, Down, Side, UpDiagonal, DownDiagonal, Blink, Hide, Sweat, Rotate };
	const int MAX_SCORE=9;
	const int PICKUP_SECONDS_LIMIT = 10;

	public float Speed;
	public GameObject Score, King, Timer, Fader;

	private Vector3 lastPosition;
	private Animator bunnyAnimator;
	private float sideRotation;
	private int currentScore;
	private bool wonRevengeGame = false;
	private bool wonPickupGame = false;
	private bool fadeOut = false;
	private bool fadeIn = false;
	private bool canMove = true;

	void Start () {

		canMove = true;

		bunnyAnimator = GetComponent <Animator>();
		Fader.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);

		if (this.tag == "Scene1") {
			canMove = false;
			ShowTextForSeconds ("TITLE: The narcoleptic bunny and the carrots of revenge.");
			StartCoroutine(waitThenResetPickupGame());
		}

		if (this.tag == "Scene2") {
			ShowTextForSeconds ("Chapter 2: You are asleep");
			StartCoroutine(waitThenResetDream());
		}

		if (this.tag == "Scene3") {
			canMove = false;
			ShowTextForSeconds ("Chapter 3: You are awake", 2f);
			StartCoroutine(waitThenResetRevenge());
		}
	}

	IEnumerator waitThenResetPickupGame(){
		fadeIn = true;
		yield return new WaitForSeconds (3f);
		resetPickupGame ();
	}

	IEnumerator waitThenResetDream(){
		yield return new WaitForSeconds (3f);
		fadeIn = true;
		resetDream ();
	}

	IEnumerator waitThenResetRevenge(){
		yield return new WaitForSeconds (3f);
		fadeIn = true;
		resetRevengeStage ();
	}

	void loseOnRevenge(){
		FadeOut ();
		ShowTextForSeconds("The End", 5f, default(Vector3), "reloadLevel");
	}

	void reloadLevel(){
		Application.LoadLevel (Application.loadedLevel);
	}



	#region PICKUP_GAME

	void resetPickupGame(){
		currentScore = 0;
		UpdateScore ();
		GetComponent<Transform> ().position = new Vector3 (0f, 0f, -0.1f);	
		wonPickupGame = false;
		ShowTextForSeconds(new string[]{"Your family is hungry", "Use WASD to find food"}, 2f, default(Vector3), "StartPickupTimer");
		var pickups = GameObject.FindGameObjectsWithTag("Pickup");
		foreach (var pickup in pickups) {
			pickup.gameObject.GetComponent<Renderer>().enabled = true;
		}
	}


	void StartPickupTimer(){
		ShowTextForSeconds("GO!", 0.5f);
		canMove = true;
		StartCoroutine (PickupTimer ());
	}

	void LosePickupGame ()
	{
		canMove = false;
		ShowTextForSeconds ("Your family died of hunger");
		FadeOut ();
		Score.GetComponent<TextMesh>().text = "";
		Timer.GetComponent<TextMesh>().text = "";
		var pickups = GameObject.FindGameObjectsWithTag("Pickup");
		foreach (var pickup in pickups) {
			pickup.gameObject.GetComponent<Renderer>().enabled = false;
		}
		StartCoroutine( waitThenResetPickupGame() );
	}



	IEnumerator PickupTimer(){

		for (int i = 0; i < PICKUP_SECONDS_LIMIT; i++) {
			yield return new WaitForSeconds (1);
			if(wonPickupGame) break;
			Timer.GetComponent<TextMesh>().text = (PICKUP_SECONDS_LIMIT - i).ToString();
		}
		if (!wonPickupGame) {
			LosePickupGame ();
		}
	}

	void FixedUpdate() {
		UpdatePosition();
		UpdateSprite();

		if (fadeOut) {
			var spriteRenderer = Fader.GetComponent<SpriteRenderer> ();

			if(spriteRenderer.color.a >= 1f) {
				fadeOut = false;
				if(this.tag == "Scene1"){
					int newLevel = Application.loadedLevel + (wonPickupGame ? 1 : 0);
					Application.LoadLevel(newLevel);
				}else if(this.tag == "Scene2"){
					Application.LoadLevel(Application.loadedLevel+1);
				}
			} else {
				spriteRenderer.color = new Color(1f,1f,1f, spriteRenderer.color.a + 0.01f);
			}
		}else if (fadeIn) {
			var spriteRenderer = Fader.GetComponent<SpriteRenderer> ();
			
			if(spriteRenderer.color.a <= 0f) {
				fadeIn = false;
			} else {
				spriteRenderer.color = new Color(1f,1f,1f, spriteRenderer.color.a - 0.01f);
			}
		}

	}

	void OnTriggerEnter2D(Collider2D other) {
		var gameControl = other.gameObject.GetComponent<GameControl> ();
		if(gameControl) gameControl.SendMessage ("CharacterCollision");

		if (other.tag == "Pickup" && other.gameObject.GetComponent<Renderer>().enabled) {
			// remove carrot
			other.gameObject.GetComponent<Renderer>().enabled = false;

			// update score
			currentScore++;

			if(this.tag == "Scene1"){
				UpdateScore();
				if(currentScore == MAX_SCORE){
					winPickupGame();
				}
			}else{
				King.GetComponent<KingScript>().CausePain(currentScore);
			}
		}  
	}

	void UpdateScore(){
		Score.GetComponent<TextMesh>().text = String.Format("{0}/{1}", currentScore, MAX_SCORE);
	}

	void winPickupGame(){
		wonPickupGame = true;
		ShowTextForSeconds(new string[]{"You collected enough carrots to feed your family.", "Now you are sleepy"}, 3f, new Vector3(0,0,-0.1f), "FadeOut");
		Score.GetComponent<TextMesh>().text = "";
		Timer.GetComponent<TextMesh>().text = "";
	}

	public void FadeOut() {
		fadeOut = true;
	}

	#endregion //PICKUP_GAME


	#region DREAM

	void resetDream() {
		ShowTextForSeconds("You are hungry", 3f);
	}


	#endregion //DREAM


	#region REVENGE

	public void WinGame(){
		FadeOut ();
		ShowTextForSeconds(new string[]{"TITLE: All the carrot children were dead", "TITLE: In his grief the carrot king was defeated", "TITLE: You won"}, 4f, default(Vector3), "resetEntireGame");
	}

	void resetEntireGame(){
		Application.LoadLevel (0);
	}

	void resetRevengeStage ()
	{
		currentScore = 0;
		King.GetComponent<Transform> ().position = new Vector3 (-1f, 0f, -0.1f);
		GetComponent<Transform> ().position = new Vector3 (1f, 0.03f, -0.1f);	
		wonRevengeGame = false;
		var pickups = GameObject.FindGameObjectsWithTag("Pickup");
		foreach (var pickup in pickups) {
			pickup.gameObject.GetComponent<Renderer>().enabled = true;
		}

		string[] conversation = new string[]{
			"OTHER: Hey friend, what's wrong?",
			"YOU: I had the worst dream", "YOU: Carrots were out to get me",
			"OTHER: That was no dream",
			"YOU: What.. *stammer* what do you mean?",
			"OTHER: You have done us a great wrong", "OTHER: Eaten my children without remorse",
			"OTHER: Now I will make you suffer"
		};
		ShowTextForSeconds(conversation, 1.2f, new Vector3(0f,1f,-0.5f), "kingTransform");
	}

	void kingTransform(){
		canMove = true;
		King.GetComponent<KingScript> ().Transformation ();
	}

	#endregion //REVENGE



	#region MOVEMENT_ANIMATION

	void UpdatePosition() {

		if (canMove) {
			float moveHorizontal, moveVertical;
			Vector3 movement;
		
			moveHorizontal = Input.GetAxis ("Horizontal");
			moveVertical = Input.GetAxis ("Vertical");
		
			movement = new Vector3 (moveHorizontal, moveVertical);
			movement = movement * Speed;

			lastPosition = transform.position;
			transform.position = transform.position + movement;
		} else {
			lastPosition = transform.position;
		}

		
	}
	
	void UpdateSprite() {

		if (MovingUp ()) {
			// UPLEFT OR UPRIGHT
			if (MovingLeft () || MovingRight()) {
				
				bunnyAnimator.SetInteger ("CurrentAnimation", (int) Sprites.UpDiagonal);
				transform.rotation = new Quaternion(0f,sideRotation,0,0);
				
				// JUST UP
			} else {
				bunnyAnimator.SetInteger ("CurrentAnimation", (int) Sprites.Up);
			}
			
			
		} else if (MovingDown ()) {
			
			// DOWNLEFT OR DOWNRIGHT
			if (MovingLeft () || MovingRight()) {
				
				bunnyAnimator.SetInteger ("CurrentAnimation", (int) Sprites.DownDiagonal);
				transform.rotation = new Quaternion(0f,sideRotation,0,0);
				
				// JUST DOWN
			} else {
				bunnyAnimator.SetInteger ("CurrentAnimation", (int) Sprites.Down);
			}
			
		} else {
			
			//RIGHT OR LEFT
			if (MovingLeft() || MovingRight() ){
				
				bunnyAnimator.SetInteger ("CurrentAnimation", (int) Sprites.Side);
				
				// IDLE BLINKING
			}else{
				if(this.tag == "Scene3"){
					bunnyAnimator.SetInteger ("CurrentAnimation", (int) Sprites.Hide);
				}else{
					bunnyAnimator.SetInteger ("CurrentAnimation", (int) Sprites.Blink);	
				}
			}
			
			transform.rotation = new Quaternion(0f,sideRotation,0f,0);
		}
		
		
	}
	
	
	
	Boolean MovingUp(){
		return (transform.position.y > lastPosition.y);
	} 
	
	Boolean MovingDown(){
		return (transform.position.y < lastPosition.y);
	}
	
	Boolean MovingLeft(){
		var left = (transform.position.x < lastPosition.x);
		if (left)
			sideRotation = 0f; 
		return left;
	}
	
	Boolean MovingRight (){
		var right = (transform.position.x > lastPosition.x);
		if (right)
			sideRotation = 180f; 
		return right;
	}

	#endregion //MOVEMENT_ANIMATION

}
