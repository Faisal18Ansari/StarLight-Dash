using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class EnemyHealth : MonoBehaviour
    {
        private Animator animator;
        private AudioSource zombieAudioSource;
        private ZombieIdentifier zombieIdentifier; // NEW

        public int health = 2;
        private bool isDead = false;

        void Start()
        {
            animator = GetComponent<Animator>();
            zombieAudioSource = GetComponent<AudioSource>();
            zombieIdentifier = GetComponent<ZombieIdentifier>();

            // If this zombie was already killed in saved data, disable immediately
            if (zombieIdentifier != null && SaveSystem.IsZombieKilled(zombieIdentifier.zombieID))
            {
                gameObject.SetActive(false);
            }
        }

        public void TakeDamage(int damage)
        {
            if (isDead) return; // prevent over-triggering

            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;

            // Save as killed
            if (zombieIdentifier != null)
            {
                ZombieSaveTracker.Instance.AddKilledZombie(zombieIdentifier.zombieID);
            }

            // Stop zombie audio if playing
            if (zombieAudioSource != null && zombieAudioSource.isPlaying)
            {
                zombieAudioSource.Stop();
            }

            // Play death animation if available
            if (animator != null)
            {
                animator.SetTrigger("Die");
            }

            // Destroy the zombie after 2 seconds (adjust to match death animation length)
            Destroy(gameObject, 2f);
        }

        void Update()
        {
            // No per-frame logic needed
        }
    }
}
