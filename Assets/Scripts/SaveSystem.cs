using UnityEngine;
using System.IO;

public static class SaveSystem
{
    // Generates the profile path based on the index number (1, 2, or 3)
    private static string GetSavePath(int profileIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"profile_{profileIndex}.json");
    }

    // Serializes the PlayerData object and saves it to disk
    public static void SaveProfile(int profileIndex, PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSavePath(profileIndex), json);
        Debug.Log($"Profile {profileIndex} saved successfully to disk!");
    }

    // Reads the file from disk and returns a filled PlayerData object (or null if it doesn't exist)
    public static PlayerData LoadProfile(int profileIndex)
    {
        string path = GetSavePath(profileIndex);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log($"Profile {profileIndex} file read successfully!");
            return loadedData;
        }
        else
        {
            Debug.LogWarning($"No existing save data file discovered for Profile {profileIndex}!");
            return null;
        }
    }
}