using UnityEngine;
using System.Collections;

public class Carrot : GameControl {

	private int collisions;

	// Use this for initialization
	void Start () {
		collisions = 0;
	}
	
	public void CharacterCollision() {
		// Check for if it is visible so we don't register collisions with invisible objects
		if (GetComponent<Renderer> ().enabled) {

			// next collision
			collisions++;

			// runs the right method, i.e. first time it is called the Collision1() method will run
			this.SendMessage ("Collision" + collisions);
		}
	}

	private void Collision1(){
		ShowTextForSeconds ("You hear screaming", 1f);

		// make carrot invisible
		GetComponent< Renderer >().enabled = false;

		// coroutines allow things to happen without affecting our gameplay 
		// i.e. stopping and doing nothing for a few seconds won't stop us from playing in that time
		StartCoroutine (collision1yield() );
	}

	private IEnumerator collision1yield(){
		//hide carrot
		yield return new WaitForSeconds(2f); // waits seconds

		//reposition carrot on horizonal opposite
		Vector3 position = GetComponent<Transform>().position;
		position.x = position.x * -1;
		GetComponent<Transform> ().position = position;

		// make carrot visible again
		GetComponent<Renderer> ().enabled = true;
	}

	private void Collision2(){
		ShowTextForSeconds ("Definately screaming", 2f);
		GetComponent< Renderer >().enabled = false;

		// SHOW NOTE HERE
	}

}
