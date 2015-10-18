using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameControl : MonoBehaviour {
	
	GameObject objectText = null;
	TextMesh message; 

	// Use this for initialization
	void Start () {

	}

	public void CharacterCollision(){}

	public void ShowTextForSeconds(string text, float seconds = 2f, bool destroyAfter = false) {
		ObjectText ();
		StartCoroutine(showTextForSeconds(text, seconds, destroyAfter));
	}

	private IEnumerator showTextForSeconds(string text, float seconds, bool destroyAfter) {
		message.text = text;
		yield return new WaitForSeconds(seconds); // waits seconds
		Destroy (objectText);
		if (destroyAfter) {
			Destroy(this.gameObject);
		}
	}

	private void ObjectText(){

		if (!objectText) {
			objectText = new GameObject ();
			message = objectText.AddComponent<TextMesh> ();
			message.characterSize = 0.1f;
			message.fontSize = 27;
			message.alignment = TextAlignment.Center;
			message.anchor = TextAnchor.UpperCenter;
		}

		Vector3 position = GetComponent<Transform>().position;
		position.y = position.y + 0.5f;


		objectText.transform.position = position;

	}
}