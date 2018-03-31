using UnityEngine;
using System.Collections;

/// <summary>
/// Handles enemy animation as well as sounds.
/// </summary>
public class EnemyAnimation : MonoBehaviour {

	//[SerializeField] private Animation animComponent;
	[SerializeField] private Animator animator;

	[SerializeField] private AudioSource[] footstepAudioList;
	[SerializeField] private AudioClip[] idleSoundList;
	[SerializeField] private AudioClip[] chaseSoundList;

	[SerializeField] private AudioSource monsterSoundSource;

	private const float FOOTSTEP_PATROL_DELAY = 1.5f;
	private const float FOOTSTEP_CHASE_DELAY = 0.4f;

	private const float CHANCE_TO_MAKE_IDLE_NOISE = 100.0f; //out of 100;
	private const float IDLE_TIMEOUT_SOUND_PLAY = 2.0f;
	private const float CHASE_TIMEOUT_SOUND_PLAY = 1.0f;

	private float footstepPlayTime = 0.0f;
	private float soundTimeoutTime = 0.0f;
	private EnemyAI.EnemyActionType enemyActionType;

	private bool attacking = false;
	private bool makingNoise = false;


	// Use this for initialization
	void Start () {
	
	}

	void Update() {
		if (this.enemyActionType == EnemyAI.EnemyActionType.PATROLLING) {
			this.PlayFootstep(FOOTSTEP_PATROL_DELAY);
		}
		else if(this.enemyActionType == EnemyAI.EnemyActionType.CHASING) {
			this.PlayFootstep(FOOTSTEP_CHASE_DELAY);
			this.PlayRandomChaseSound();
		}
		else if(this.enemyActionType == EnemyAI.EnemyActionType.IDLE) {
			this.footstepPlayTime = 0.0f;
			this.PlayRandomIdleSound();
		}
	}

	private void PlayFootstep(float delay) {
		this.footstepPlayTime += Time.deltaTime;
		
		if(this.footstepPlayTime >= delay) {
			this.footstepPlayTime = 0.0f;
			
			this.footstepAudioList[Random.Range(0, this.footstepAudioList.Length)].Play();
			
		}
	}

	private void PlayRandomIdleSound() {
		this.soundTimeoutTime += Time.deltaTime;
		if (this.makingNoise == false && this.soundTimeoutTime >= IDLE_TIMEOUT_SOUND_PLAY) {
			float chance = Random.Range(0.0f, 100.0f);

			if(chance <= CHANCE_TO_MAKE_IDLE_NOISE) {
				this.makingNoise = true;
				this.monsterSoundSource.clip = this.idleSoundList[Random.Range(0,this.idleSoundList.Length)];
				this.monsterSoundSource.Play();
				this.StartCoroutine(this.ObserveSound(0.0f));
			}
			else {
				this.soundTimeoutTime = 0.0f;
			}
		}
	}

	private void PlayRandomChaseSound() {
		if(this.makingNoise == false) {
			this.makingNoise = true;
			this.monsterSoundSource.clip = this.chaseSoundList[Random.Range(0,this.chaseSoundList.Length)];
			this.monsterSoundSource.Play();
			this.StartCoroutine(this.ObserveSound(CHASE_TIMEOUT_SOUND_PLAY));
		}
	}

	private IEnumerator ObserveSound(float additionalDelay) {
		yield return new WaitForSeconds (this.monsterSoundSource.clip.length + additionalDelay);
		this.makingNoise = false;
	}

	public float GetCurrentDuration() {
		//Debug.Log ("Anim length: " + this.animator.GetCurrentAnimatorStateInfo (0).length);
		return this.animator.GetCurrentAnimatorStateInfo (0).length;
	}


	public void SetAnimationFromType(EnemyAI.EnemyActionType actionType) {
		if (this.enemyActionType == actionType || this.attacking == true) {
			return;
		}

		Debug.Log ("New action type: " + actionType);
		this.enemyActionType = actionType;

		switch (this.enemyActionType) {
		case EnemyAI.EnemyActionType.IDLE:
			this.animator.SetFloat ("RunSpeed", 0.0f);
			break;
		case EnemyAI.EnemyActionType.PATROLLING:
			this.animator.SetFloat ("RunSpeed", 20.0f);
			break;
		case EnemyAI.EnemyActionType.CHASING:
			this.animator.SetFloat ("RunSpeed", 40.0f);
			break;
		case EnemyAI.EnemyActionType.ATTACKING:
			break;
		}
	}

	/// <summary>
	/// Plays the attack animation. Can be called on an update function
	/// </summary>
	public void PlayAttackAnim() {
		if (this.attacking == false) {
			this.attacking = true;
			this.enemyActionType = EnemyAI.EnemyActionType.ATTACKING;
			this.animator.SetTrigger("Attack");
			this.StartCoroutine(this.HandleAttackAnim());
		}
	}

	private IEnumerator HandleAttackAnim() {
		yield return new WaitForSeconds(0.7f);
		PlayerHP.Instance.AttackHit ();
		this.attacking = false;
	}

	public bool IsAttacking() {
		return this.attacking;
	}

}
