using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int Health;
    public int MaxHealth;
    public int Damage;
    public int Resistance;
    public int Defense;
    public int Level;
    public int Gold;
    public int Essence;
    public int Step;
    public int Stage;
    public int Exp;
}

[Serializable]
public class SaveLoad
{
    public PlayerData playerData;

    private readonly string SaveFile = "/save.sav";

    public SaveLoad() { }

    public void Save()
    {
        string json = JsonUtility.ToJson(GameManager.Instance.saveLoad);
        BinaryFormatter binaryFormatter = new();
        using FileStream fileStream = new(Application.persistentDataPath + SaveFile, FileMode.Create);
        binaryFormatter.Serialize(fileStream, json);
        Debug.Log("Saved Data");
    }

    public SaveLoad Load()
    {
        if (!File.Exists(Application.persistentDataPath + SaveFile))
            return null;

        BinaryFormatter binaryFormatter = new();
        using FileStream fileStream = new(Application.persistentDataPath + SaveFile, FileMode.Open);
        SaveLoad saveLoad = JsonUtility.FromJson<SaveLoad>(binaryFormatter.Deserialize(fileStream).ToString());
        Debug.Log("Loaded Game Data");
        return saveLoad;
    }
}
