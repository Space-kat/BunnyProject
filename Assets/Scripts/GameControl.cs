using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// all things inherit from game control
// allows us to give common methods and properties to all things
public class GameControl : MonoBehaviour {

	private Vector3 otherTextPosition = new Vector3(0, -1f, -0.5f);
	private Vector3 youTextPosition = new Vector3(0, 1f, -0.5f);


	GameObject objectText {
		get {
			if(theObjectText == null){
				theObjectText = new GameObject();
				theObjectText.AddComponent<TextMesh> ();
			}
			return theObjectText;
		}
	}

	GameObject theObjectText = null;

	TextMesh message {
		get {
			if(theMesh == null){ 
				theMesh = objectText.GetComponent<TextMesh> ();
				theMesh.characterSize = 0.1f;
				theMesh.fontSize = 20;
				theMesh.alignment = TextAlignment.Center;
				theMesh.anchor = TextAnchor.UpperCenter;
			}
			return theMesh;
		}
	}

	TextMesh theMesh = null;

	// allows character to register collision 
	public virtual void CharacterCollision () {
	}

	// ------ SHOW TEXT

	public void ShowTextForSeconds(string text, float seconds = 2f, Vector3 position = default(Vector3), string andThen = null) {
		ShowTextForSeconds( new string[]{text}, seconds, position, andThen);
	}

	public void ShowTextForSeconds(string[] texts, float seconds = 2f, Vector3 position = default(Vector3),  string andThen = null) {
		ObjectText (position);
		StartCoroutine(showTextForSeconds(texts, seconds, andThen));
	}

	private IEnumerator showTextForSeconds(string[] texts, float seconds, string andThen) {
		
		message.color = Color.white;
		message.fontStyle = FontStyle.Normal;
		string tempText = "";
		foreach (string text in texts) {
			if(text.Contains("YOU: ")){
			
				message.text = text.Replace("YOU: ","");
				message.color = Color.white;
				objectText.transform.position = youTextPosition;
			
			}else if(text.Contains("OTHER: ")){
			
				message.text = text.Replace("OTHER: ","");
				message.color = Color.yellow;
				objectText.transform.position = otherTextPosition;

			}else if(text.Contains("Chapter") || text.Contains("The End") || text.Contains("TITLE: ")){

				message.fontStyle = FontStyle.Bold;
				message.fontSize = message.fontSize+5;
				message.text = text.Replace("TITLE: ","");

				objectText.transform.position = new Vector3(0f,0f,-0.5f);
			}else if(text.Contains("KING: ")){
				message.color = Color.yellow;
				message.text = text.Replace("KING: ","");
			}else{
				message.text = text;
			}

			tempText = message.text;
			yield return new WaitForSeconds (seconds); // waits seconds
		}

		if (message.text == tempText) {
			message.text = "";
		}

		if (andThen != null) {
			this.SendMessage(andThen);
		}
	}

	private void ObjectText(Vector3 position){

		if (position == default(Vector3)) {
			position = GetComponent<Transform>().position;
			position.y = position.y + 0.5f;
		}

		position.z = -0.5f;
		objectText.transform.position = position;

	}
	// ------ END SHOW TEXT
}