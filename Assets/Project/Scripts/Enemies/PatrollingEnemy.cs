using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : Enemy {

	public GameObject model;
	public float speed = 3f;
	public float movementAmplitude = 4f;

	private Vector3 initialPosition;
	private bool movingLeft = true;

	// Use this for initialization
	protected virtual void Start () {
		initialPosition = transform.position;
	}

	// Update is called once per frame
	protected virtual void Update () {
		transform.position = new Vector3 (
			transform.position.x + speed * Time.deltaTime * (movingLeft ? -1 : 1),
			transform.position.y,
			transform.position.z
		);

		if (movingLeft == true && transform.position.x < initialPosition.x - movementAmplitude / 2) {
			movingLeft = false;
		} else if (movingLeft == false && transform.position.x > initialPosition.x + movementAmplitude / 2) {
			movingLeft = true;
		}

		// Perform model rotation.
		if (movingLeft) {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler (new Vector3 (0, 90, 0)), Time.deltaTime * 6f);
		} else {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler (new Vector3 (0, -90, 0)), Time.deltaTime * 6f);
		}
	}
}
