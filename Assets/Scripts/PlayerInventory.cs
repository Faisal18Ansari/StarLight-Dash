using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Platformer
{
    public class PlayerInventory : MonoBehaviour
    {
        public AudioSource coinCollectAudio;

        public int numberOfCoins { get; private set; }
        public UnityEvent<PlayerInventory> onCoinsCollected;
        public void CoinsCollected()
        {
            numberOfCoins++;
            onCoinsCollected.Invoke(this);
            if (coinCollectAudio != null)
            {
                coinCollectAudio.Play();
            }
        }
        public void LoadCoins(int coins)
        {
            numberOfCoins = coins;
            onCoinsCollected.Invoke(this);
        }

    }
}
