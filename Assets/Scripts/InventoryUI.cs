using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Platformer
{
    public class InventoryUI : MonoBehaviour
    {
        private TextMeshProUGUI coinText;

        // Start is called before the first frame update
        void Awake()
        {
            coinText = GetComponent<TextMeshProUGUI>();
        }

        public void UpdateCoinText(PlayerInventory player)
        {
            if (coinText == null)
            {
                coinText = GetComponent<TextMeshProUGUI>();
            }
            coinText.text = player.numberOfCoins.ToString();
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
