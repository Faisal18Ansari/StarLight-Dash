using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class AutoDisableAfter : MonoBehaviour
    {
        public float disable = 11f;

        void onEnable()
        {
            Invoke(nameof(DisableSelf), disable);
        }

        void DisableSelf()
        {
            gameObject.SetActive(false);
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
