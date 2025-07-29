using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class CheckPoint : MonoBehaviour
    {
        public Color activatedColor = Color.green;
        private bool isActivated = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isActivated) // <-- Now using it
            {
                Debug.Log("Checkpoint activated!");

                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.UpdateCheckpoint(transform.position);
                }

                Renderer rend = GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.material.color = activatedColor;
                }

                isActivated = true;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
