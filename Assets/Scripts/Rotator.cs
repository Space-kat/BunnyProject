using UnityEngine;
using System.Collections;

public class Rotator : GameControl {
	
	void Update () {
		transform.Rotate (new Vector3 (90, 90, 0) *Time.deltaTime);
	}

}
