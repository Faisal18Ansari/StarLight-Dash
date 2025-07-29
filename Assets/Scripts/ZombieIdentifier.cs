using UnityEngine;

namespace Platformer
{
    public class ZombieIdentifier : MonoBehaviour
    {
        public string zombieID; // Assign unique ID in Inspector

        private void Awake()
        {
            if (string.IsNullOrEmpty(zombieID))
            {
                zombieID = System.Guid.NewGuid().ToString(); // Auto-generate if not assigned
            }
        }
    }
}
