using System.Collections.Generic;
using UnityEngine;

public class ZombieSaveTracker : MonoBehaviour
{
    public static ZombieSaveTracker Instance { get; private set; }

    public List<string> killedZombieIDsThisSession = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject); // Persist across scenes
    }

    public void MarkZombieAsKilled(string zombieID)
    {
        if (!killedZombieIDsThisSession.Contains(zombieID))
        {
            killedZombieIDsThisSession.Add(zombieID);
            Debug.Log("Zombie marked as killed: " + zombieID);
        }
    }
    public void AddKilledZombie(string zombieID)
    {
        if (!killedZombieIDsThisSession.Contains(zombieID))
        {
            killedZombieIDsThisSession.Add(zombieID);
        }
    }

}
