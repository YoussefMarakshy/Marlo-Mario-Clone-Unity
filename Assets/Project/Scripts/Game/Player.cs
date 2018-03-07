using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour {
	[Header("Visuals")]
	public GameObject model;
	public Animator animator;
	public float powerUpSize = 1.3f;

	[Header("Acceleration")]
	public float acceleration = 2.5f;
	public float deacceleration = 5.0f;

	[Header("Movement fields")]
	[Range(4f, 6f)]
	public float movementSpeed = 4f;
	public float movementSpeedRight = 8f;
	public float movementSpeedLeft = 2f;

	[Header("Jumping fields")]
	public float normalJumpingSpeed = 6f;
	public float longJumpingSpeed = 10f;
	public float destroyEnemyJumpingSpeed = 9f;
	public float jumpDuration = 0.75f;
	public float verticalWallJumpingSpeed = 5f;
	public float horizontalWallJumpingSpeed = 3.5f;

	[Header("Power ups")]
	public float invincibilityDuration = 5f;

	public Action onCollectCoin;

	private float speed = 0f;
	private float jumpingSpeed = 0f;
	private float jumpingTimer = 0f;

	private bool dead = false;
	private bool paused = false;
	private bool canJump = false;
	private bool jumping = false;
	private bool canWallJump = false;
	private bool wallJumpLeft = false;
	private bool onSpeedAreaLeft = false;
	private bool onSpeedAreaRight = false;
	private bool onLongJumpBlock = false;
	private bool finished = false;

	private bool hasPowerUp = false;
	private bool hasInvincibility = false;

	private Vector3 previousPosition;

	private bool facingRight = true;

	public bool Dead {
		get {
			return dead;
		}
	}

	public bool Finished {
		get {
			return finished;
		}
	}

	// Use this for initialization
	void Start () {
		jumpingSpeed = normalJumpingSpeed;
	}

	void FixedUpdate () {
		animator.transform.localPosition = Vector3.zero;

		// Running animation.
		if (transform.position.x > previousPosition.x + 0.01f) {
			animator.SetFloat ("Forward", 1f);
		} else {
			animator.SetFloat ("Forward", 0f);
		}

		// Set the player's direction.
		facingRight = previousPosition.x <= transform.position.x;

		previousPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {

		if (dead) {
			return;
		}

		// Accelerate the player.
		speed += acceleration * Time.deltaTime;

		float targetMovementSpeed = movementSpeed;
		if (onSpeedAreaLeft) {
			targetMovementSpeed = movementSpeedLeft;
		} else if (onSpeedAreaRight) {
			targetMovementSpeed = movementSpeedRight;
		}

		if (speed > targetMovementSpeed) {
			speed -= deacceleration * Time.deltaTime;
		}

		// Move horizontally.
		GetComponent<Rigidbody> ().velocity = new Vector3(
			paused ? 0 : speed, 
			GetComponent<Rigidbody> ().velocity.y, 
			GetComponent<Rigidbody> ().velocity.z
		);

		// Check for input.
		bool pressingJumpButton = Input.GetMouseButton (0) || Input.GetKey ("space");
		if (pressingJumpButton) {
			if (canJump) {
				Jump ();
			}
		}

		// Check for unpause.
		if (paused && pressingJumpButton) {
			paused = false;
		}

		// Make the player jump.
		if (jumping) {
			jumpingTimer += Time.deltaTime;

			if (pressingJumpButton && jumpingTimer < jumpDuration) {
				if (onLongJumpBlock) {
					jumpingSpeed = longJumpingSpeed;
				}

				GetComponent<Rigidbody> ().velocity = new Vector3 (
					GetComponent<Rigidbody> ().velocity.x, 
					jumpingSpeed, 
					GetComponent<Rigidbody> ().velocity.z
				);
			}
		}

		// Make the player wall jump.
		if (canWallJump) {
			speed = 0;

			if (pressingJumpButton) {
				canWallJump = false;

				speed = wallJumpLeft ? -horizontalWallJumpingSpeed : horizontalWallJumpingSpeed;

				GetComponent<Rigidbody> ().velocity = new Vector3 (
					GetComponent<Rigidbody> ().velocity.x, 
					verticalWallJumpingSpeed, 
					GetComponent<Rigidbody> ().velocity.z
				);
			}
		}

		// Rotating the players model.
		transform.rotation = Quaternion.Lerp(transform.rotation, facingRight ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0), Time.deltaTime * 3);
	}

	public void Pause () {
		paused = true;
	}

	void OnTriggerEnter (Collider otherCollider) {
		// Collect coins.
		if (otherCollider.transform.GetComponent<Coin> () != null) {
			Destroy (otherCollider.gameObject);
			onCollectCoin ();
		}

		// Speed up or down.
		if (otherCollider.GetComponent<SpeedArea> () != null) {
			SpeedArea speedArea = otherCollider.GetComponent<SpeedArea> ();
			if (speedArea.direction == Direction.Left) {
				onSpeedAreaLeft = true;
			} else if (speedArea.direction == Direction.Right) {
				onSpeedAreaRight = true;
			}
		}

		// Perform long jump.
		if (otherCollider.GetComponent<LongJumpBlock> () != null) {
			onLongJumpBlock = true;
		}

		// Kill the player when they touch the enemy.
		if (otherCollider.GetComponent<Enemy> () != null) {
			Enemy enemy = otherCollider.GetComponent<Enemy> ();
			if (hasInvincibility == false && enemy.Dead == false) {
				if (hasPowerUp == false) {
					Kill ();
				} else {
					hasPowerUp = false;
					model.transform.localScale = Vector3.one;
					ApplyInvincibility ();
				}
			}
		}

		// Collect the PowerUp.
		if (otherCollider.GetComponent<PowerUp> () != null) {
			PowerUp powerUp = otherCollider.GetComponent<PowerUp> ();
			powerUp.Collect ();
			ApplyPowerUp ();
		}

		// Reach the finish line.
		if (otherCollider.GetComponent<FinishLine> () != null) {
			hasInvincibility = true;
			finished = true;
		}
	}

	void OnTriggerStay (Collider otherCollider) {
		if (otherCollider.tag == "JumpingArea") {
			canJump = true;
			jumping = false;
			jumpingSpeed = normalJumpingSpeed;
			jumpingTimer = 0f;
			animator.SetBool ("OnGround", true);
		} else if (otherCollider.tag == "WallJumpingArea") {
			canWallJump = true;
			wallJumpLeft = transform.position.x < otherCollider.transform.position.x;
		}
	}

	void OnTriggerExit (Collider otherCollider) {
		if (otherCollider.tag == "JumpingArea") {
			animator.SetBool ("OnGround", false);
		} else if (otherCollider.tag == "WallJumpingArea") {
			canWallJump = false;
		}

		if (otherCollider.GetComponent<SpeedArea> () != null) {
			SpeedArea speedArea = otherCollider.GetComponent<SpeedArea> ();
			if (speedArea.direction == Direction.Left) {
				onSpeedAreaLeft = false;
			} else if (speedArea.direction == Direction.Right) {
				onSpeedAreaRight = false;
			}
		}

		if (otherCollider.GetComponent<LongJumpBlock> () != null) {
			onLongJumpBlock = false;
		}
	}

	void Kill () {
		dead = true;
		GetComponent<Collider> ().enabled = false;
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		GetComponent<Rigidbody> ().AddForce (new Vector3(0f, 500f, -800f));
	}

	public void Jump (bool hitEnemy = false) {
		jumping = true;

		if (hitEnemy) {
			GetComponent<Rigidbody> ().velocity = new Vector3 (
				GetComponent<Rigidbody> ().velocity.x, 
				destroyEnemyJumpingSpeed, 
				GetComponent<Rigidbody> ().velocity.z
			);
		}
	}

	void ApplyPowerUp () {
		hasPowerUp = true;
		model.transform.localScale = Vector3.one * powerUpSize;
	}

	void ApplyInvincibility () {
		hasInvincibility = true;
		StartCoroutine (InvincibilityRoutine());
	}

	private IEnumerator InvincibilityRoutine () {
		// Slow blinks.
		float initialWaitingTime = invincibilityDuration * 0.75f;
		int initialBlinks = 20;

		for (int i = 0; i < initialBlinks; i++) {
			model.SetActive (!model.activeSelf);
			yield return new WaitForSeconds (initialWaitingTime / initialBlinks);
		}

		// Fast blinks.
		float finalWaitingTime = invincibilityDuration * 0.25f;
		int finalBlinks = 30;

		for (int i = 0; i < finalBlinks; i++) {
			model.SetActive (!model.activeSelf);
			yield return new WaitForSeconds (finalWaitingTime / finalBlinks);
		}

		model.SetActive (true);

		hasInvincibility = false;
	}

	public void OnDestroyBrick () {
		GetComponent<Rigidbody> ().velocity = new Vector3(
			GetComponent<Rigidbody> ().velocity.x,
			0,
			GetComponent<Rigidbody> ().velocity.z
		);
		canJump = false;
		jumping = false;
	}
}
