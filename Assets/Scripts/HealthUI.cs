using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Platformer
{
    public class HealthUI : MonoBehaviour
    {
        public List<Image> hearts;//list of all the hearts
        public Sprite fullHeart;//sprite of a full heart
        public Sprite emptyHeart;//sprite of an empty heart

        public void UpdateHealth(int currentHealth)
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                if (i < currentHealth)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite=emptyHeart;
                }
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
