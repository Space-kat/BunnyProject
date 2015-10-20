using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// all things inherit from game control
// allows us to give common methods and properties to all things
public class GameControl : MonoBehaviour {
	
	GameObject objectText = null;
	TextMesh message; 

	// allows character to register collision 
	public void CharacterCollision(){}

	// ------ SHOW TEXT

	public void ShowTextForSeconds(string text, float seconds = 2f, bool destroyAfter = false, Vector3 position = default(Vector3), string andThen = null) {
		ShowTextForSeconds( new string[]{text}, seconds, destroyAfter, position, andThen);
	}

	public void ShowTextForSeconds(string[] texts, float seconds = 2f, bool destroyAfter = false, Vector3 position = default(Vector3),  string andThen = null) {
		ObjectText (position);
		StartCoroutine(showTextForSeconds(texts, seconds, destroyAfter, andThen));
	}

	private IEnumerator showTextForSeconds(string[] texts, float seconds, bool destroyAfter, string andThen) {
		foreach (string text in texts) {
			message.text = text;
			yield return new WaitForSeconds (seconds); // waits seconds
		}
		Destroy (objectText);
		if (destroyAfter) {
			Destroy(this.gameObject);
		}
		if (andThen != null) {
			this.SendMessage(andThen);
		}
	}

	private void ObjectText(Vector3 position){

		if (!objectText) {
			objectText = new GameObject ();
			message = objectText.AddComponent<TextMesh> ();
			message.characterSize = 0.1f;
			message.fontSize = 20;
			message.alignment = TextAlignment.Center;
			message.anchor = TextAnchor.UpperCenter;
		}

		if (position == default(Vector3)) {
			position = GetComponent<Transform>().position;
			position.y = position.y + 0.3f;
		}


		objectText.transform.position = position;

	}
	// ------ END SHOW TEXT
}