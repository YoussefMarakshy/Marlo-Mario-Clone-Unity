using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Player player;
	public Text scoreText;
	public Text endLevelText;

	public float gravity = 9.81f;

	private int score;
	private float restartTimer = 3f;
	private float finishTimer = 5f;
	private bool finished;

	// Use this for initialization
	void Start () {
		player.onCollectCoin = OnCollectCoin;
		endLevelText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		Physics.gravity = new Vector3 (0, -gravity, 0);

		if (player.Dead) {
			restartTimer -= Time.deltaTime;
			if (restartTimer <= 0f) {
				LevelManager.Instance.ReloadLevel ();
			}
		}

		if (player.Finished) {
			if (finished == false) {
				finished = true;
				OnFinish ();
			}

			finishTimer -= Time.deltaTime;
			if (finishTimer <= 0f) {
				LevelManager.Instance.LoadNextLevel ();
			}
		}
	}

	void OnCollectCoin () {
		score++;
		scoreText.text = "Score: " + score;
	}

	void OnFinish () {
		endLevelText.enabled = true;
		endLevelText.text = "You beat " + LevelManager.Instance.LevelName + "!";
		endLevelText.text += "\nYour score: " + score;
	}
}
