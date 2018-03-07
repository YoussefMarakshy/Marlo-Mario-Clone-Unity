using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnemy : Enemy {

	public float visibleHeight = 0f;
	public float hiddenHeight = -2f;
	public float movementSpeed = 2f;
	public float waitingDuration = 5f;

	private bool hiding = true;
	private float waitingTimer = 0f;

	// Use this for initialization
	void Start () {
		waitingTimer = waitingDuration;
	}
	
	// Update is called once per frame
	void Update () {
		if (hiding) {
			if (transform.localPosition.y > hiddenHeight) {
				transform.localPosition = new Vector3 (
					transform.localPosition.x,
					transform.localPosition.y - movementSpeed * Time.deltaTime,
					transform.localPosition.z
				);
			} else {
				waitingTimer -= Time.deltaTime;
				if (waitingTimer <= 0f) {
					waitingTimer = waitingDuration;
					hiding = false;
				}
			}
		} else {
			if (transform.localPosition.y < visibleHeight) {
				transform.localPosition = new Vector3 (
					transform.localPosition.x,
					transform.localPosition.y + movementSpeed * Time.deltaTime,
					transform.localPosition.z
				);
			} else {
				waitingTimer -= Time.deltaTime;
				if (waitingTimer <= 0f) {
					waitingTimer = waitingDuration;
					hiding = true;
				}
			}
		}
	}
}
