using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

	public GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider otherCollider) {
		if (otherCollider.transform.GetComponent<Player> () != null) {
			target.SendMessage ("OnKill");
			Destroy (target.gameObject);
		}
	}
}
