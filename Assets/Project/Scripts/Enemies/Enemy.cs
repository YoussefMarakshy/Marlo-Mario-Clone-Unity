using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	protected bool dead = false;
	public bool Dead {
		get {
			return dead;
		}
	}

	protected virtual void OnKill () {
		dead = true;
		GetComponent<BoxCollider> ().enabled = false;
		GameObject.Find ("Player").GetComponent<Player> ().Jump (true);
	}
}
