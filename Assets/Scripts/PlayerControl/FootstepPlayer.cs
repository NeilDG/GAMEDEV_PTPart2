using UnityEngine;
using System.Collections;

/// <summary>
/// Handles playing of footsteps
/// </summary>
public class FootstepPlayer : MonoBehaviour {
	[SerializeField] private CharacterMotor motor;
	[SerializeField] private AudioSource[] audioSteps;

	private float footstepPlayDelay = 0.5f;
	private float nextPlayTime = 0.0f;
	private float currentPlayTime = 0.0f;

	// Use this for initialization
	void Start () {
		this.nextPlayTime += footstepPlayDelay;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		
		if (directionVector != Vector3.zero) {
			this.PlayFootstep();
		}
		else {
			this.currentPlayTime = 0.0f;
		}

		
		if(Input.GetKeyDown(KeyCode.LeftShift)) {
			this.footstepPlayDelay = 0.25f;
			this.motor.movement.maxForwardSpeed = PlayerConstants.PLAYER_RUN_SPEED;
		}
		else if(Input.GetKeyUp(KeyCode.LeftShift)) {
			this.footstepPlayDelay = 0.5f;
			this.motor.movement.maxForwardSpeed = PlayerConstants.PLAYER_WALK_SPEED;
		}
}

	private void PlayFootstep() {
		this.currentPlayTime += Time.deltaTime;

		if(this.currentPlayTime >= this.footstepPlayDelay) {
			this.currentPlayTime = 0.0f;

			this.audioSteps[Random.Range(0, this.audioSteps.Length)].Play();

		}
	}
}
