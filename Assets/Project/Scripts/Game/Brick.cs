using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

	public GameObject coinPrefab;
	public bool hasCoin;

	void OnKill () {
		Player player = GameObject.Find ("Player").GetComponent<Player> ();

		if (hasCoin) {
			GameObject coinObject = GameObject.Instantiate (coinPrefab);
			coinObject.transform.position = transform.position + new Vector3 (0, 0.7f, 0);

			Coin coin = coinObject.GetComponent<Coin> ();
			coin.Vanish ();

			player.onCollectCoin ();
		}

		player.OnDestroyBrick ();
	}
}
