using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBlock : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider otherCollider) {
		if (otherCollider.GetComponent<Player> () != null) {
			otherCollider.GetComponent<Player> ().Pause ();
			GetComponent<BoxCollider> ().enabled = false;
		}
	}
}
