using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterScript : GameControl {

	enum Sprites { Up, Down, Side, UpDiagonal, DownDiagonal, Blink, Hide, Sweat, Rotate };
	const int MAX_SCORE=9;
	const int PICKUP_SECONDS_LIMIT = 10;

	public float Speed;
	public GameObject Score;
	public GameObject Timer;
	public GameObject Fader;

	private Vector3 lastPosition;
	private Animator bunnyAnimator;
	private float sideRotation;
	private int currentScore;
	private bool wonPickupGame = false;
	private bool fadeOut = false;
	private bool fadeIn = false;

	void Start () {
		bunnyAnimator = GetComponent <Animator>();

		if (this.tag == "Scene1") {
			resetPickupGame();
		}

		if (this.tag == "Scene2") {
			resetDream();
		}
	}

	#region PICKUP_GAME

	IEnumerator waitThenResetPickupGame(){
		yield return new WaitForSeconds (3);
		resetPickupGame ();
	}
	
	void resetPickupGame(){
		Fader.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0f);
		currentScore = 0;
		UpdateScore ();
		GetComponent<Transform> ().position = new Vector3 (0f, 0f, -0.1f);	
		wonPickupGame = false;
		StartPickupTimer ();
		ShowTextForSeconds("Your family is hungry");
		
		var pickups = GameObject.FindGameObjectsWithTag("Pickup");
		foreach (var pickup in pickups) {
			pickup.gameObject.GetComponent<Renderer>().enabled = true;
		}
	}


	void StartPickupTimer(){
		StartCoroutine (PickupTimer ());
	}

	void LosePickupGame ()
	{
		ShowTextForSeconds ("Your family died of hunger");
		Score.GetComponent<TextMesh>().text = "YOU LOSE";
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
				Application.LoadLevel(Application.loadedLevel+1);
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
			UpdateScore();
			if(currentScore == MAX_SCORE){
				winPickupGame();
			}
		}  
	}

	void UpdateScore(){
		Score.GetComponent<TextMesh>().text = String.Format("{0}/{1}", currentScore, MAX_SCORE);
	}

	void winPickupGame(){
		wonPickupGame = true;
		ShowTextForSeconds(new string[]{"You collected enough carrots to feed your family.", "Now you are sleepy"}, 3f, false, new Vector3(0,0,-0.1f), "FadeOut");
		Score.GetComponent<TextMesh>().text = "";
		Timer.GetComponent<TextMesh>().text = "";
	}

	void FadeOut() {
		fadeOut = true;
	}

	#endregion //PICKUP_GAME


	#region DREAM

	void resetDream() {
		Fader.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
		fadeIn = true;
		ShowTextForSeconds("You are hungry", 3f);
	}


	#endregion //DREAM



	#region MOVEMENT_ANIMATION

	void UpdatePosition() {
		
		float moveHorizontal, moveVertical;
		Vector3 movement;
		
		moveHorizontal = Input.GetAxis ("Horizontal");
		moveVertical = Input.GetAxis ("Vertical");
		
		movement = new Vector3 (moveHorizontal, moveVertical);
		movement = movement * Speed;
		
		lastPosition = transform.position;
		transform.position = transform.position + movement;
		
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
				bunnyAnimator.SetInteger ("CurrentAnimation", (int) Sprites.Blink);
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
