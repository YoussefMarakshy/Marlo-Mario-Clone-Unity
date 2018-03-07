using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellEnemy : PatrollingEnemy {

	public GameObject shellPrefab;

	// Use this for initialization
	override protected void Start () {
		base.Start ();
	}

	// Update is called once per frame
	override protected void Update () {
		base.Update ();
	}

	override protected void OnKill () {
		base.OnKill ();

		GameObject shellObject = Instantiate (shellPrefab);
		shellObject.transform.position = this.transform.position;
		shellObject.transform.parent = this.transform.parent;
	}
}
