using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	public GameObject model;
	public float rotatingSpeed = 30f;
	public float startingAngleOffset = 35f;

	// Use this for initialization
	void Start () {
		model.transform.RotateAround(model.transform.position, Vector3.up, transform.position.x * startingAngleOffset);
	}
	
	// Update is called once per frame
	void Update () {
		model.transform.RotateAround (model.transform.position, Vector3.up, rotatingSpeed * Time.deltaTime);
	}

	public void Vanish () {
		StartCoroutine (VanishRoutine ());
	}

	private IEnumerator VanishRoutine () {
		yield return new WaitForSeconds (0.5f);
		Destroy (this.gameObject);
	}
}
