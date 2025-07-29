using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class SaveSystem
{
    private const string CollectedCoinsKey = "CollectedCoins";
    private const string KilledZombiesKey = "KilledZombies";

    public static void SaveGame(Vector3 position, int health, int coins, bool hasCheckpoint, List<string> killedZombieIDs)
    {
        PlayerPrefs.SetFloat("PlayerX", position.x);
        PlayerPrefs.SetFloat("PlayerY", position.y);
        PlayerPrefs.SetFloat("PlayerZ", position.z);
        PlayerPrefs.SetInt("PlayerHealth", health);
        PlayerPrefs.SetInt("PlayerCoins", coins);
        PlayerPrefs.SetInt("HasCheckpoint", hasCheckpoint ? 1 : 0);

        // Save killed zombies list
        SaveKilledZombies(killedZombieIDs);

        PlayerPrefs.Save();
    }

    public static bool HasSave()
    {
        return PlayerPrefs.HasKey("PlayerX");
    }

    public static Vector3 LoadPosition()
    {
        float x = PlayerPrefs.GetFloat("PlayerX", 0);
        float y = PlayerPrefs.GetFloat("PlayerY", 0);
        float z = PlayerPrefs.GetFloat("PlayerZ", 0);
        return new Vector3(x, y, z);
    }

    public static int LoadHealth()
    {
        return PlayerPrefs.GetInt("PlayerHealth", 5);
    }

    public static int LoadCoins()
    {
        return PlayerPrefs.GetInt("PlayerCoins", 0);
    }

    public static bool LoadCheckpointStatus()
    {
        return PlayerPrefs.GetInt("HasCheckpoint", 0) == 1;
    }

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey("PlayerX");
        PlayerPrefs.DeleteKey("PlayerY");
        PlayerPrefs.DeleteKey("PlayerZ");
        PlayerPrefs.DeleteKey("PlayerHealth");
        PlayerPrefs.DeleteKey("PlayerCoins");
        PlayerPrefs.DeleteKey("HasCheckpoint");
        PlayerPrefs.DeleteKey(CollectedCoinsKey);
        PlayerPrefs.DeleteKey(KilledZombiesKey);
    }

    // =======================
    // Coin Collection Tracking
    // =======================

    public static void MarkCoinAsCollected(string coinID)
    {
        string savedData = PlayerPrefs.GetString(CollectedCoinsKey, "");
        List<string> collectedCoins = savedData.Split(',').Where(id => !string.IsNullOrEmpty(id)).ToList();

        if (!collectedCoins.Contains(coinID))
        {
            collectedCoins.Add(coinID);
            string newData = string.Join(",", collectedCoins);
            PlayerPrefs.SetString(CollectedCoinsKey, newData);
            PlayerPrefs.Save();
        }
    }

    public static bool IsCoinCollected(string coinID)
    {
        string savedData = PlayerPrefs.GetString(CollectedCoinsKey, "");
        List<string> collectedCoins = savedData.Split(',').Where(id => !string.IsNullOrEmpty(id)).ToList();
        return collectedCoins.Contains(coinID);
    }

    public static void ClearCollectedCoins()
    {
        PlayerPrefs.DeleteKey(CollectedCoinsKey);
    }

    // =======================
    // Zombie Kill Tracking
    // =======================

    public static void SaveKilledZombies(List<string> killedZombieIDs)
    {
        string newData = string.Join(",", killedZombieIDs);
        PlayerPrefs.SetString(KilledZombiesKey, newData);
    }

    public static List<string> LoadKilledZombies()
    {
        string savedData = PlayerPrefs.GetString(KilledZombiesKey, "");
        return savedData.Split(',').Where(id => !string.IsNullOrEmpty(id)).ToList();
    }

    public static bool IsZombieKilled(string zombieID)
    {
        List<string> killedZombies = LoadKilledZombies();
        return killedZombies.Contains(zombieID);
    }

    public static void ClearKilledZombies()
    {
        PlayerPrefs.DeleteKey(KilledZombiesKey);
    }
}
