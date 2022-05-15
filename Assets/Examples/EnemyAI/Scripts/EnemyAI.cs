using System.Collections;
using System.Collections.Generic;
using BehaviourTrees;
using UnityEngine;

/// <summary>
/// Example enemy AI using a Behaviour tree.
/// </summary>
public class EnemyAI : MonoBehaviour
{

    // Configurable parameters of this AI.
    public Transform playerTransform;
    public float behaviourTreeTickTime = 0.5f;
    public float attackDistance = 1.5f;
    public float movementSpeed = 0.5f;
    public float attackCooldown = 0.8f;

    // Enemy components.
    private new Rigidbody rigidbody;
    private Animator animator;
    private string attackAnimationTrigger = "attack";

    // Behaviour Tree delegates declaration
    private BehaviourTreeConditional closeToPlayer;
    private BehaviourTreeAction seekPlayer;
    private BehaviourTreeAction attackPlayer;

    // Behaviour tree states to return on the delegates.
    private BehaviourTreeState seekPlayerState;
    private BehaviourTreeState attackPlayerState;


    private BehaviourTree enemyAI;






    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // Assign the different delegates.
        closeToPlayer = IsPlayerClose;
        seekPlayer = SeekPlayer;
        attackPlayer = AttackPlayer;

        // Create the behaviour tree.
        enemyAI = new BehaviourTree.Builder()
        .OpenSequence()
            .OpenFallback()
                .Conditional(closeToPlayer)
                .Action(seekPlayer)
            .Close()
            .Action(attackPlayer)
        .Close()
        .Build();

        // Start the AI.
        StartCoroutine(EnemyBehaviour());
    }

    private IEnumerator EnemyBehaviour()
    {
        while (true)
        {
            enemyAI.Tick();
            yield return new WaitForSeconds(behaviourTreeTickTime);
        }
    }

    private bool IsPlayerClose()
    {
        return (playerTransform.position - transform.position).magnitude <= attackDistance;
    }

    private BehaviourTreeState SeekPlayer()
    {
        if (!seekPlayerState.Equals(BehaviourTreeState.RUNNING))
        {
            StartCoroutine(MoveTowardsPlayer());
        }
        return seekPlayerState;
    }

    private IEnumerator MoveTowardsPlayer()
    {
        seekPlayerState = BehaviourTreeState.RUNNING;

        Vector3 directionToPlayer;
        while ((directionToPlayer = playerTransform.position - transform.position).magnitude > attackDistance)
        {
            rigidbody.AddForce(-rigidbody.velocity, ForceMode.VelocityChange);
            rigidbody.MoveRotation(Quaternion.LookRotation(directionToPlayer, Vector3.up));
            rigidbody.AddForce(directionToPlayer.normalized * movementSpeed, ForceMode.VelocityChange);

            yield return new WaitForFixedUpdate();

        }

        rigidbody.AddForce(-rigidbody.velocity, ForceMode.VelocityChange);
        seekPlayerState = BehaviourTreeState.SUCCESS;
        yield return null;
    }

    private BehaviourTreeState AttackPlayer()
    {
        if (!attackPlayerState.Equals(BehaviourTreeState.RUNNING))
        {
            StartCoroutine(Attack());
        }
        return attackPlayerState;
    }

    private IEnumerator Attack()
    {
        attackPlayerState = BehaviourTreeState.RUNNING;
        animator.SetTrigger(attackAnimationTrigger);
        yield return new WaitForSeconds(1f + attackCooldown);
        attackPlayerState = BehaviourTreeState.SUCCESS;
        yield return null;
    }

}
