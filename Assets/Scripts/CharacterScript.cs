using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; //prevents unknown entity String error there probably a better solution but it works ¯\_(ツ)_/¯ 

public class CharacterScript : MonoBehaviour {

	public float speed;
	public Sprite upSprite, downSprite, sideSprite, diagonalUpSprite, diagonalDownSprite, blinkingSprite, hidingSprite, sweatSprite, rotationSprite;
	public bool Character;

	void OnTriggerEnter2D(Collider2D other) {
		Destroy (other.gameObject);
	}

	private Vector3 lastPosition;

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

		var spriteRenderer = GetComponent <SpriteRenderer>();

		if (MovingUp ()) {
			// UPLEFTWARDS
			if (MovingLeft ()) {

				transform.rotation = new Quaternion(0f,0f,0,0);
				spriteRenderer.sprite = diagonalUpSprite;

			// UPRIGHTWARDS
			} else if (MovingRight ()) {

				transform.rotation = new Quaternion(0f,180f,0,0);
				spriteRenderer.sprite = diagonalUpSprite;

			// JUST UP
			} else {

				transform.rotation = new Quaternion(0f,0f,0,0);
				spriteRenderer.sprite = upSprite;
			}
		} else if (MovingDown ()) {
			// DOWN LEFTWARDS
			if (MovingLeft ()) {

				transform.rotation = new Quaternion(0f,0f,0,0);
				spriteRenderer.sprite = diagonalDownSprite;

			// DOWN RIGHTWARDS
			} else if (MovingRight ()) {
				
				transform.rotation = new Quaternion(0f,180f,0,0);
				spriteRenderer.sprite = diagonalDownSprite;


			// JUST DOWN
			} else {

				transform.rotation = new Quaternion(0f,0f,0,0);
				spriteRenderer.sprite = downSprite;
			
			}
		} else {
			// JUST LEFTWARDS
			if (MovingLeft ()) {
								
				transform.rotation = new Quaternion(0f,0f,0,0);
				spriteRenderer.sprite = sideSprite;
				
			// JUST RIGHTWARDS
			} else if (MovingRight ()) {
				
				transform.rotation = new Quaternion(0f,180f,0,0);
				spriteRenderer.sprite = sideSprite;
			}
		}


	}

	Boolean MovingUp(){
		return (transform.position.y > lastPosition.y);
	} 

	Boolean MovingDown(){
		return (transform.position.y < lastPosition.y);
	}

	Boolean MovingLeft(){
		return (transform.position.x < lastPosition.x);
	}

	Boolean MovingRight (){
		return (transform.position.x > lastPosition.x);
	}


}
