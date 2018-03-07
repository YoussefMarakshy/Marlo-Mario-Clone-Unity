using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Player player;
	public Vector3 offset;
	public float speed = 5f;
	
	void FixedUpdate () {
		if (player.Dead == false && player.Finished == false) {
			Vector3 targetPosition = new Vector3 (player.transform.position.x + offset.x, player.transform.position.y + offset.y, transform.position.z);
			transform.position = Vector3.Lerp (transform.position, targetPosition, speed * Time.deltaTime);
		}
	}
}
