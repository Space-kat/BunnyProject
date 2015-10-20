using UnityEngine;
using System.Collections;

public class NoteScript : GameControl {

	const float zoomInLevel = 0.2f;
	const float normalZoomLevel = 2.3f;
	
	public bool shouldZoomIn  = false;
	public bool shouldZoomOut = false;

	public GameObject character;

	private bool hasShown = false;


	// Use this for initialization
	public override void CharacterCollision() {
		if (GetComponent< SpriteRenderer > ().enabled) {
			if (!hasShown) {
				shouldZoomIn = true;
			}
		}
	}
	
	void FixedUpdate(){
		if (shouldZoomIn) {
			if(Camera.main.orthographicSize > zoomInLevel){
				Camera.main.orthographicSize = Camera.main.orthographicSize-0.01f;
			}else{
				StartCoroutine(ShouldZoomOut());
			}
		} else if (shouldZoomOut) {
			if(Camera.main.orthographicSize < normalZoomLevel){
				Camera.main.orthographicSize = Camera.main.orthographicSize+0.01f;
			}else{
				GetComponent< SpriteRenderer > ().enabled = false;
				shouldZoomOut = false;
				character.GetComponent<CharacterScript>().ShowTextForSeconds(new string[]{"You are frightened", "You black out"}, 3f, new Vector3(0,0,-0.1f), "FadeOut");
	
			}
		}
	}


	private IEnumerator ShouldZoomOut(){
		shouldZoomIn = false;
		yield return new WaitForSeconds (3.4f);
		shouldZoomOut = true;
	}

}
