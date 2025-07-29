using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Spring : MonoBehaviour
    {
        public float launchForce = 15f;
        private Vector3 launchDirection;
        private Animator animator;
        private bool isCooldown = false;
        private float cooldownTime = 1f;

        void Start()
        {
            animator = GetComponent<Animator>();

           
            launchDirection = transform.up + transform.forward;
            launchDirection.Normalize();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isCooldown)
            {
                Debug.Log("Player stepped on the spring!");
                animator.SetTrigger("Activate");

                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.LaunchPlayer(launchDirection * launchForce);
                }

                isCooldown = true;
                Invoke(nameof(ResetCooldown), cooldownTime);
            }
        }

        private void ResetCooldown()
        {
            isCooldown = false;
        }

        void Update()
        {
            // No updates needed for now
        }
    }
}
