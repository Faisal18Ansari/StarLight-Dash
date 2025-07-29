using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Breakable : MonoBehaviour
    {
        public Color warningColor = Color.blue;
        public float blinkDuration = 0.2f;
        public int blinkCount = 4;
        public float delayBeforeBreak = 0.5f;

        private Material[] platformMaterials;
        private Color[] originalColors;
        private bool isBreaking = false;

        private GameObject parentPlatform;

        void Start()
        {
            // Get the parent GameObject
            parentPlatform = transform.parent.gameObject;

            if (parentPlatform != null)
            {
                Renderer renderer = parentPlatform.GetComponent<Renderer>();
                if (renderer != null)
                {
                    platformMaterials = renderer.materials;
                    originalColors = new Color[platformMaterials.Length];

                    for (int i = 0; i < platformMaterials.Length; i++)
                    {
                        if (platformMaterials[i].HasProperty("_BaseColor"))
                        {
                            originalColors[i] = platformMaterials[i].GetColor("_BaseColor");
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isBreaking)
            {
                Debug.Log("Player stepped on breakable platform (trigger).");
                StartCoroutine(BreakPlatform());
            }
        }

        IEnumerator BreakPlatform()
        {
            isBreaking = true;

            for (int i = 0; i < blinkCount; i++)
            {
                for (int j = 0; j < platformMaterials.Length; j++)
                {
                    if (platformMaterials[j].HasProperty("_BaseColor"))
                    {
                        platformMaterials[j].SetColor("_BaseColor", warningColor);
                    }
                }
                yield return new WaitForSeconds(blinkDuration);

                for (int j = 0; j < platformMaterials.Length; j++)
                {
                    if (platformMaterials[j].HasProperty("_BaseColor"))
                    {
                        platformMaterials[j].SetColor("_BaseColor", originalColors[j]);
                    }
                }
                yield return new WaitForSeconds(blinkDuration);
            }

            yield return new WaitForSeconds(delayBeforeBreak);

            // Deactivate the parent platform
            parentPlatform.SetActive(false);
        }
    }
}
