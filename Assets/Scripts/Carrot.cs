using UnityEngine;
using System.Collections;

public class Carrot : GameControl {

	private int collisions;

	// Use this for initialization
	void Start () {
		collisions = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void CharacterCollision() {
		if (GetComponent<Renderer> ().enabled) {
			collisions++;
			this.SendMessage ("Collision" + collisions);
		}
	}

	private void Collision1(){
		ShowTextForSeconds ("You hear screaming", 1f);
		GetComponent< Renderer >().enabled = false;
		StartCoroutine (collision1yield() );
	}

	private IEnumerator collision1yield(){
		//hide carrot
		yield return new WaitForSeconds(2f); // waits seconds

		//reposition carrot
		Vector3 position = GetComponent<Transform>().position;
		position.x = position.x * -1;
		GetComponent<Transform> ().position = position;

		GetComponent<Renderer> ().enabled = true;
	}

	private void Collision2(){
		ShowTextForSeconds ("Definately screaming", 2f);
		
		GetComponent< Renderer >().enabled = false;
		StartCoroutine (collision1yield() );
	}

}
