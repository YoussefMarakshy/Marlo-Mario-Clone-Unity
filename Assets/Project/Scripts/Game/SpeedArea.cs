using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
	Left, 
	Right
}

public class SpeedArea : MonoBehaviour {

	public Direction direction;

	// Use this for initialization
	void Start () {
		if (direction == Direction.Left) {
			for (int i = 0; i < transform.childCount; i++) {
				Transform child = transform.GetChild (i);
				child.RotateAround (child.position, Vector3.forward, 180);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
