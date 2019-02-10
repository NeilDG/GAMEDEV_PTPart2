using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyrantAI : MonoBehaviour, IPauseCommand, IResumeCommand {

    [SerializeField] private UnityEngine.AI.NavMeshAgent navMeshAgent;
    [SerializeField] private Transform target;
    [SerializeField] private EnemyTriggerRadius enemyTriggerRadius;
    [SerializeField] private TyrantAnimation enemyAnim;

    private Transform playerLocation;

    public enum EnemyState {
        ACTIVE,
        RESTRICTED
    }

    public enum EnemyActionType {
        IDLE,
        PATROLLING,
        CHASING,
        ATTACKING,
        FORCE_CHASE,
    }

    private EnemyState currentEnemyState = EnemyState.ACTIVE;
    private EnemyActionType currentActionType = EnemyActionType.IDLE;

    private bool playerInSight = false;
    private bool hasRecentlyAttacked = false;
    private Vector3 lastPlayerSighting = Vector3.zero;

    private bool shouldWait = false;



    void Awake() {
    }

    // Use this for initialization
    void Start() {
        this.playerLocation = GameObject.FindObjectOfType<CharacterController>().transform;
        this.StartCoroutine(this.DelaySwitchAction(0.5f, EnemyActionType.PATROLLING, this.TransitionToPatrolling));

        this.enemyTriggerRadius.SetOnTriggerDelegate(this.HandleTriggerEnter, this.HandleTriggerStay, this.HandleTriggerExit);
        GamePauseHandler.Instance.AttachClassToVisit(this, this);

        EventBroadcaster.Instance.AddObserver(EventNames.ON_ESCAPE_EVENT_STARTED, this.ForceChasePlayer);

        if (GameStateMachine.HasProperState() && GameStateMachine.Instance.GetGameState() == GameStateMachine.StateType.GAME_ESCAPE_EVENT) {
            this.ForceChasePlayer();
        }

    }

    void Destroy() {
        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_ESCAPE_EVENT_STARTED, this.ForceChasePlayer);
    }

    // Update is called once per frame
    void Update() {
        switch (this.currentEnemyState) {
            case EnemyState.ACTIVE:
            this.HandleEnemyAction();
            break;
            case EnemyState.RESTRICTED:
            //do nothing
            break;
        }
    }

    private void HandleEnemyAction() {
        switch (this.currentActionType) {
            case EnemyActionType.IDLE:
            //do nothing
            break;
            case EnemyActionType.PATROLLING:
            if (this.navMeshAgent.hasPath && this.navMeshAgent.remainingDistance <= EnemyConstants.PATROL_STOPPING_DISTANCE) {
                this.TransitionToIdle(2.5f);
            }
            break;
            case EnemyActionType.CHASING:
            break;
            case EnemyActionType.ATTACKING:
            break;
            case EnemyActionType.FORCE_CHASE:
            this.enemyAnim.SetAnimationFromType(EnemyActionType.CHASING);
            this.navMeshAgent.SetDestination(this.playerLocation.position);

            if (Vector3.Distance(this.playerLocation.position, this.transform.position) <= EnemyConstants.CHASE_STOPPING_DISTANCE) {
                this.enemyAnim.PlayAttackAnim();
                this.HaltAgent();

            }
            else {
                this.navMeshAgent.isStopped = false;
            }
            break;
        }
    }

    /// <summary>
    /// Transitions to patrolling state.
    /// </summary>
    private void TransitionToPatrolling() {
        this.target = EnemyPatrolPointDirectory.Instance.GetRandomPatrolPoint();
        this.navMeshAgent.SetDestination(this.target.position);
        this.navMeshAgent.speed = EnemyConstants.PATROL_SPEED;
        this.enemyAnim.SetAnimationFromType(EnemyActionType.PATROLLING);
    }

    /// <summary>
    /// Transitions to idle state. Automatically transitions to patrol state if no further action is triggered.
    /// </summary>
    private void TransitionToIdle(float idleTime) {
        this.currentActionType = EnemyActionType.IDLE;
        this.navMeshAgent.ResetPath();
        this.enemyAnim.SetAnimationFromType(EnemyActionType.IDLE);
        this.StartCoroutine(this.DelaySwitchAction(idleTime, EnemyActionType.PATROLLING, this.TransitionToPatrolling));
    }

    private void TransitionToChasing() {
        this.currentActionType = EnemyActionType.CHASING;
        this.navMeshAgent.speed = EnemyConstants.CHASE_SPEED;
        this.navMeshAgent.acceleration = EnemyConstants.CHASE_ACCELERATION;
        this.enemyAnim.SetAnimationFromType(this.currentActionType);
    }

    private void ForceChasePlayer() {
        this.TransitionToChasing();
        this.currentActionType = EnemyActionType.FORCE_CHASE;
    }

    private void HandleTriggerEnter(Collider other) {

    }

    /// <summary>
    /// Handles the trigger event. This is used to check if the trigger event was caused by the player. This means that the player may be in sight for the enemy
    /// </summary>
    /// <param name="other">Other.</param>
    private void HandleTriggerStay(Collider other) {

        if (this.currentEnemyState == EnemyState.RESTRICTED) {
            this.navMeshAgent.ResetPath();
            return;
        }

        if (this.currentActionType == EnemyActionType.FORCE_CHASE) {
            return;
        }

        CharacterController playerControl = other.gameObject.GetComponent<CharacterController>();

        if (playerControl != null) {
            this.playerInSight = false;

            //Debug.Log("Player is within trigger!");
            // Create a vector from the enemy to the player and store the angle between it and forward.
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            // If the angle between forward and where the player is, is less than half the angle of view...
            if (angle < EnemyConstants.FIELD_OF_VIEW_ANGLE * 0.5f && this.hasRecentlyAttacked == false) {
                this.playerInSight = true;
                this.lastPlayerSighting = playerControl.transform.position;

                this.TransitionToChasing();
                this.navMeshAgent.SetDestination(this.lastPlayerSighting);

                if (Vector3.Distance(this.lastPlayerSighting, this.transform.position) <= EnemyConstants.CHASE_STOPPING_DISTANCE) {
                    this.enemyAnim.PlayAttackAnim();
                    this.hasRecentlyAttacked = true;
                    this.HaltAgent();

                }
                else {
                    this.TransitionToChasing();
                    this.navMeshAgent.isStopped = false;
                }
            }
        }
    }

    private void HaltAgent() {
        this.navMeshAgent.acceleration = 160.0f;
        this.navMeshAgent.angularSpeed = 300.0f;
        this.navMeshAgent.isStopped = true;
        this.navMeshAgent.velocity = Vector3.zero;
        this.TransitionToIdle(1.25f);
    }

    private void HandleTriggerExit(Collider other) {
        CharacterController playerControl = other.gameObject.GetComponent<CharacterController>();

        if (playerControl != null && this.currentActionType != EnemyActionType.FORCE_CHASE) {
            this.playerInSight = false;
            this.TransitionToIdle(1.0f);
        }
    }

    private IEnumerator DelaySwitchAction(float seconds, EnemyActionType actionType, System.Action transitionFunction) {
        yield return new WaitForSeconds(seconds);
        transitionFunction();
        this.hasRecentlyAttacked = false;
        this.currentActionType = actionType;
        this.enemyAnim.SetAnimationFromType(this.currentActionType);
    }

    public void Pause() {
        this.currentEnemyState = EnemyState.RESTRICTED;
    }

    public void Resume() {
        this.currentEnemyState = EnemyState.ACTIVE;
    }

    public EnemyActionType GetEnemyActionType() {
        return this.currentActionType;
    }

    private void Attack() {

    }
}
