using Platformer;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public float detectionRange = 15f;
    public float attackRange = 2f;
    public int damageAmount = 1;
    public float attackCooldown = 2f;
    public AudioSource zombieGrowl;
    private float growlTimer = 0f;
    public float growlInterval = 5f;
    private Transform player;
    private PlayerController playerController;
    private Animator animator;
    private NavMeshAgent agent;
    private float attackTimer = 0f;
    private EnemyHealth enemyHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();

        agent.stoppingDistance = attackRange * 0.8f; // stop slightly before attacking
    }

    void Update()
    {
        growlTimer += Time.deltaTime;
        if (growlTimer >= growlInterval)
        {
            if (zombieGrowl != null && !zombieGrowl.isPlaying)
            {
                zombieGrowl.Play();
            }
            growlTimer = 0;
        }
        if (enemyHealth.health <= 0) return; // skip if zombie dead
        if (playerController != null && playerController.isDead)
        {
            agent.isStopped = true;
            animator.SetFloat("Speed", 0);
            return; // stop all actions if player dead
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            agent.SetDestination(player.position);

            // Smooth walking animation
            float speedPercent = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("Speed", speedPercent);

            if (distance <= attackRange)
            {
                agent.isStopped = true;

                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0f)
                {
                    animator.SetTrigger("Attack");
                    attackTimer = attackCooldown;
                }
            }
            else
            {
                agent.isStopped = false;
            }
        }
        else
        {
            agent.isStopped = true;
            animator.SetFloat("Speed", 0);
        }
    }

    // Called via Animation Event during Attack animation
    public void DealDamage()
    {
        if (playerController != null && !playerController.isDead)
        {
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                playerController.TakeDamage(damageAmount);
            }
        }
    }
}
