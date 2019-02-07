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

    private TyrantAI.EnemyActionType enemyActionType;

    private bool attacking = false;
    private bool makingNoise = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetAnimationFromType(TyrantAI.EnemyActionType actionType) {
        this.enemyActionType = actionType;

        switch (this.enemyActionType) {
            case TyrantAI.EnemyActionType.IDLE:
            this.animator.SetFloat(SPEED_KEY, 0.0f);
            break;
            case TyrantAI.EnemyActionType.PATROLLING:
            this.animator.SetFloat(SPEED_KEY, 0.1f);
            break;
            case TyrantAI.EnemyActionType.CHASING:
            this.animator.SetFloat(SPEED_KEY, 1.0f);
            break;
            case TyrantAI.EnemyActionType.ATTACKING:
            break;
        }
    }
}
