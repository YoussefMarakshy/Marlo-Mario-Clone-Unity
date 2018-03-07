using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour {

	public GameObject referenceObject;
	public float elementSize;
	public float elementOffset;
	public float speed;

	private List<GameObject> backgroundElements;

	// Use this for initialization
	void Start () {
		backgroundElements = new List<GameObject> ();

		for (int i = 0; i < transform.childCount; i++) {
			backgroundElements.Add(transform.GetChild (i).gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Applying infinite illusion.
		if (referenceObject != null) {
			foreach (GameObject backgroundElement in backgroundElements) {
				if (referenceObject.transform.position.x - backgroundElement.transform.position.x > elementOffset) {
					backgroundElement.transform.position = new Vector3 (
						backgroundElement.transform.position.x + backgroundElements.Count * elementSize,
						backgroundElement.transform.position.y,
						backgroundElement.transform.position.z
					);
				}
			}
		}

		// Making background elements move.
		foreach (GameObject backgroundElement in backgroundElements) {
			backgroundElement.transform.position += Vector3.left * speed * Time.deltaTime;
		}
	}
}
