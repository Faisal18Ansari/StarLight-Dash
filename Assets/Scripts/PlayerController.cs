using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float walkSpeed = 5f;
        public float sprintSpeed = 8f;
        public float gravity = -20f;
        public float jumpForce = 8f;

        [Header("Dash")]
        public float dashSpeed = 20f;
        public float dashDuration = 0.2f;
        public float dashCoolDown = 0.3f;
        private float dashCoolDownTimer = 0f;
        private bool isDashing = false;

        [Header("Ground Pound")]
        public float groundPoundSpeed = -40f;
        private bool isGroundPounding = false;

        [Header("Reference")]
        public Animator animator;
        public float waitAfterJump = 0.5f;
        public float radius = 3f;
        public int damage = 1;

        [Header("Health")]
        public HealthUI healthUI;
        public Vector3 respawnPoint;
        public int maxHealth = 5;
        private int currentHealth;
        public bool isDead = false;

        private CharacterController controller;
        private Vector3 velocity;
        private bool canDoubleJump = false;
        private bool isSprinting = false;
        public ParticleSystem groundPoundEffect;
        public PlayerInventory playerInventory;
        public AudioSource dashAudio;
        public GameObject dashParticle;
        public AudioSource groundPoundAudio;

        public bool hasCheckpoint = false;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            respawnPoint = transform.position; // default spawn point
            currentHealth = maxHealth;
            healthUI.UpdateHealth(currentHealth);

            LoadPlayerData();
        }

        public void TakeDamage(int damage)
        {
            if (isDead) return;

            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (animator != null) animator.SetTrigger("Hit");
            if (healthUI != null) healthUI.UpdateHealth(currentHealth);

            if (currentHealth <= 0)
            {
                StartCoroutine(Die());
            }
        }

        private IEnumerator Die()
        {
            Debug.Log("Player Died");
            isDead = true;

            if (animator != null) animator.SetTrigger("Die");

            yield return new WaitForSeconds(1f);

            currentHealth = maxHealth;
            if (healthUI != null) healthUI.UpdateHealth(currentHealth);

            Respawn();
            if (animator != null) animator.SetFloat("Speed", 0f);

            isDead = false;
        }

        public void Respawn()
        {
            controller.enabled = false;
            transform.position = respawnPoint; // uses last checkpoint if available
            controller.enabled = true;
            velocity = Vector3.zero;
            isDead = false;
        }

        public void UpdateCheckpoint(Vector3 newRespawnPoint)
        {
            respawnPoint = newRespawnPoint;
            hasCheckpoint = true;
        }

        public void LaunchPlayer(Vector3 force)
        {
            StartCoroutine(ApplyLaunch(force));
        }

        private IEnumerator ApplyLaunch(Vector3 force)
        {
            velocity = force;
            yield return new WaitForSeconds(waitAfterJump);
            velocity = Vector3.zero;
        }

        void Update()
        {
            if (isDashing) return;

            if (controller.isGrounded)
            {
                if (isGroundPounding)
                {
                    groundPoundEffect.Play();
                    GroundPoundDamage();
                }
                isGroundPounding = false;
                if (velocity.y < 0) velocity.y = -2f;
                canDoubleJump = true;
            }

            Move();
            HandleJump();
            HandleDash();
            HandleGroundPound();
            ApplyGravity();

            dashCoolDownTimer -= Time.deltaTime;
        }

        private void GroundPoundDamage()
        {
            Vector3 groundPosition = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
            Collider[] hitColliders = Physics.OverlapSphere(groundPosition, radius);

            foreach (var hitCollider in hitColliders)
            {
                EnemyHealth enemy = hitCollider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
            Debug.Log("Ground Pound Damage Applied");
        }

        void Move()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * vertical + cameraRight * horizontal;
            moveDirection.Normalize();

            isSprinting = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

            if (moveDirection != Vector3.zero)
            {
                transform.forward = Vector3.Slerp(transform.forward, moveDirection, 0.15f);
            }

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);

            if (animator != null)
            {
                float normalizedSpeed = moveDirection.magnitude * (currentSpeed / sprintSpeed);
                animator.SetFloat("Speed", normalizedSpeed);
            }
        }

        void HandleJump()
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (controller.isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                    canDoubleJump = true;
                    if (animator != null) animator.SetTrigger("Jump");
                }
                else if (canDoubleJump && !isGroundPounding)
                {
                    velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                    canDoubleJump = false;
                    if (animator != null) animator.SetTrigger("Jump");
                }
            }
        }

        void HandleDash()
        {
            if (Input.GetKey(KeyCode.E) && dashCoolDownTimer <= 0 && !controller.isGrounded)
            {
                StartCoroutine(Dash());
                dashCoolDownTimer = dashCoolDown;
            }
        }

        IEnumerator Dash()
        {
            isDashing = true;
            if (dashAudio != null) dashAudio.Play();
            if (dashParticle != null) dashParticle.SetActive(true);

            float startTime = Time.time;
            Vector3 dashDirection = transform.forward;
            while (Time.time < startTime + dashDuration)
            {
                controller.Move(dashDirection * dashSpeed * Time.deltaTime);
                yield return null;
            }
            if (dashParticle != null) dashParticle.SetActive(false);
            isDashing = false;
        }

        void HandleGroundPound()
        {
            if (!controller.isGrounded && !isGroundPounding && Input.GetKeyDown(KeyCode.Q))
            {
                isGroundPounding = true;
                velocity.y = groundPoundSpeed;
                if (animator != null) animator.SetTrigger("GroundPound");
                if (groundPoundAudio != null) groundPoundAudio.Play();
            }
        }

        void ApplyGravity()
        {
            if (!isGroundPounding)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            controller.Move(velocity * Time.deltaTime);
        }

        public void SavePlayerData()
        {
            SaveSystem.SaveGame(
                transform.position,
                currentHealth,
                playerInventory.numberOfCoins,
                hasCheckpoint,
                ZombieSaveTracker.Instance.killedZombieIDsThisSession // ✅ Correct field for saving zombies
            );
        }

        public void LoadPlayerData()
        {
            if (SaveSystem.HasSave())
            {
                transform.position = SaveSystem.LoadPosition();
                currentHealth = SaveSystem.LoadHealth();
                hasCheckpoint = SaveSystem.LoadCheckpointStatus();
                healthUI.UpdateHealth(currentHealth);

                if (playerInventory != null)
                {
                    playerInventory.LoadCoins(SaveSystem.LoadCoins());
                }

                List<string> killedZombieIDs = SaveSystem.LoadKilledZombies();
                ZombieSaveTracker.Instance.killedZombieIDsThisSession = killedZombieIDs;

                ZombieIdentifier[] zombies = FindObjectsOfType<ZombieIdentifier>();
                foreach (ZombieIdentifier zombie in zombies)
                {
                    if (killedZombieIDs.Contains(zombie.zombieID))
                    {
                        zombie.gameObject.SetActive(false);
                    }
                }
            }
            Debug.Log("Loaded Position: " + transform.position);
        }
    }
}
