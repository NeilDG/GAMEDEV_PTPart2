using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the tyrant animation and sounds
/// </summary>
public class TyrantAnimation : MonoBehaviour
{
    private const string SPEED_KEY = "Speed";
    private const string VARIANCE_KEY = "Variance";

    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip[] idleSoundList;
    [SerializeField] private AudioClip[] chaseSoundList;

    [SerializeField] private AudioSource monsterSoundSource;

    private TyrantAI.EnemyActionType enemyActionType = TyrantAI.EnemyActionType.IDLE;

    private string[] attackKeys;
    private bool attacking = false;
    private bool makingNoise = false;

    private float factor = 0.0f;
    private float currentSpeed = 0.0f;
    private float speedB = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.attackKeys = new string[2];
        this.attackKeys[0] = "Attack01"; this.attackKeys[1] = "Attack02";
    }

    // Update is called once per frame
    void Update()
    {
        this.currentSpeed = Mathf.Lerp(this.currentSpeed, this.speedB, this.factor);
        this.currentSpeed = Mathf.Clamp(this.currentSpeed, 0.0f, 1.0f);

        this.animator.SetFloat(SPEED_KEY, this.currentSpeed);
        this.factor += 10.0f * Time.deltaTime;
    }

    public void SetAnimationFromType(TyrantAI.EnemyActionType actionType) {
        this.enemyActionType = actionType;
        this.factor = 0.0f;

        if(this.enemyActionType == TyrantAI.EnemyActionType.IDLE) {
            this.speedB = 0.0f;
        }
        else if(this.enemyActionType == TyrantAI.EnemyActionType.PATROLLING) {
            this.speedB = 0.1f;
        }
        else if(this.enemyActionType == TyrantAI.EnemyActionType.CHASING) {
            this.speedB = 1.0f;
        }
        else if(this.enemyActionType == TyrantAI.EnemyActionType.ATTACKING) {
            this.speedB = 0.0f;
        }

    }

    /// <summary>
	/// Plays the attack animation. Can be called on an update function
	/// </summary>
	public void PlayAttackAnim() {
        if (this.attacking == false) {
            this.attacking = true;
            this.enemyActionType = TyrantAI.EnemyActionType.ATTACKING;

            int attackIndex = Random.Range(0, 2);
            this.animator.SetTrigger(this.attackKeys[attackIndex]);
            
            this.StartCoroutine(this.HandleAttackAnim());
        }
    }

    private IEnumerator HandleAttackAnim() {
        yield return new WaitForSeconds(0.7f);
        PlayerHP.Instance.AttackHit();
        this.attacking = false;
    }

    public bool IsAttacking() {
        return this.attacking;
    }
}
