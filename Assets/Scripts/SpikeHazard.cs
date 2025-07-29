using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class SpikeHazard : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player Hit");
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.Respawn();
                }
            }
        }
    }
}
