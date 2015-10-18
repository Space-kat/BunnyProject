using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; //prevents unknown entity String error there probably a better solution but it works ¯\_(ツ)_/¯ 

public class CharacterScript : MonoBehaviour {

	enum Sprites { Up, Down, Side, UpDiagonal, DownDiagonal, Blink, Hide, Sweat, Rotate };

	public float speed;
	public bool Character;

	private Vector3 lastPosition;
	private Animator bunnyAnimator;
	private float sideRotation;

	void Start () {
		bunnyAnimator = GetComponent <Animator>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		Destroy (other.gameObject);
	}

	void FixedUpdate() {
		UpdatePosition();
		UpdateSprite();
	}

	void UpdatePosition() {

		float moveHorizontal, moveVertical;
		Vector3 movement;
		
		moveHorizontal = Input.GetAxis ("Horizontal");
		moveVertical = Input.GetAxis ("Vertical");
		
		movement = new Vector3 (moveHorizontal, moveVertical);
		movement = movement * speed;

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


}
