using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Coin : MonoBehaviour
    {
        public string coinID; // Set a unique ID in the inspector or via script

        private void Start()
        {
            if (SaveSystem.IsCoinCollected(coinID))
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerInventory player = other.GetComponent<PlayerInventory>();
            if (player != null)
            {
                player.CoinsCollected();
                SaveSystem.MarkCoinAsCollected(coinID);
                gameObject.SetActive(false);
            }
        }
    }
}
