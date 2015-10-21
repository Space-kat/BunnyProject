using UnityEngine;
using System.Collections;

public class KingScript : GameControl {

	
	enum Sprites { Up, Down, Side, UpDiagonal, DownDiagonal, Blink, Hide, Sweat, Rotate };

	public Sprite kingSprite;
	public Sprite bunnySprite;
	public float speed;

	public GameObject character;
	
	private Animator kingAnimator;

	private bool isKing;
	private bool bunnyIsDead;

	void Start (){
		reset ();
	}

	public void reset(){
		kingAnimator = GetComponent <Animator>();
		bunnyIsDead = false;
		isKing = false;
		GetComponent<Transform> ().localScale = new Vector3 (1f, 1f, 1f);
		GetComponent<SpriteRenderer> ().sprite = bunnySprite;
		GetComponent<Animator> ().StartPlayback();
		GetComponent<SpriteRenderer> ().color = new Color(0.588f, 0.588f, 0.588f);
		character.GetComponent<SpriteRenderer> ().enabled = true;
	}

	void CharacterCollision(){
		bunnyIsDead = true;
		character.GetComponent<SpriteRenderer> ().enabled = false;
		character.GetComponent<CharacterScript>().ShowTextForSeconds(new string[]{"You were eaten", "The carrots rest in peace"}, 3f, new Vector3 (0, 0, -0.5f), "loseOnRevenge");
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (isKing) {
			//face the bunny
			bool turnLeft;
			if (!bunnyIsDead){
				turnLeft = bunnyToMyLeft ();
			} else {
				turnLeft = GetComponent<Transform>().position.x > 0;
			}

			if (turnLeft) {
				GetComponent<Transform> ().rotation = new Quaternion (0, 180f, 0, 0);
			} else {
				GetComponent<Transform> ().rotation = new Quaternion (0, 0, 0, 0);
			}

			float newX = GetComponent<Transform> ().position.x;
			float newY = GetComponent<Transform> ().position.y;
			if(!bunnyIsDead){

				//chase the bunny

				newX = newX + speed * (bunnyToMyLeft() ? -1f : 1f);
				newY = newY + speed * (bunnyBelowMe()  ? -1f : 1f);

			}else{

				newX = newX + speed * (newX > 0 ? -1f : 1f);
				newY = newY + speed * (newY > 0  ? -1f : 1f);

			}
			GetComponent<Transform> ().position = new Vector3(newX, newY, -0.1f); 

			if(bunnyIsDead){
				GetComponent<Transform> ().localScale = GetComponent<Transform> ().localScale * 1.01f;
			}
		}
	}

	bool bunnyToMyLeft(){
		return character.GetComponent<Transform>().position.x < GetComponent<Transform>().position.x;
	}

	bool bunnyBelowMe(){
		return character.GetComponent<Transform>().position.y < GetComponent<Transform>().position.y;
	}

	public void CausePain(int index) {
		switch (index) {
		case 1:
			ShowTextForSeconds("KING: Argh!", 1f);
			break;
		case 2:
			ShowTextForSeconds("KING: Stop it!", 1f);
			break;
		case 3:
			ShowTextForSeconds("KING: NO!", 1f);
			break;
		case 4:
			ShowTextForSeconds("KING: Those are my children!", 1f);
			break;
		case 5:
			ShowTextForSeconds("KING: How could you be so cold?!", 1f);
			break;
		case 7:
			ShowTextForSeconds("KING: PLEASE STOP IT", 1f);
			break;
		case 9:
			ShowTextForSeconds(new string[]{"KING: All of my children...", "They're all dead."}, 1.5f, default(Vector3), "WinGame");
			character.GetComponent<CharacterScript>().FadeOut();
			isKing = false;
			break;
		default:
			break;
		}
	}

	void WinGame(){
		character.GetComponent<CharacterScript>().WinGame();
	}

	public void Transformation(){
		GetComponent<Animator> ().Stop();
		GetComponent<SpriteRenderer> ().sprite = kingSprite;
		GetComponent<SpriteRenderer> ().color = Color.white;
		isKing = true;
	}

}
