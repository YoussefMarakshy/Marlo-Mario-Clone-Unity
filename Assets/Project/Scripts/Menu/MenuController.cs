using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

	public GameObject helpPanel;

	public void OnPressPlay () {
		LevelManager.Instance.LoadLevel (1, 1);
	}

	public void OnPressHelp () {
		helpPanel.SetActive (true);
	}
}
