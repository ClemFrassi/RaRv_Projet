using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class GameConfig 
{
    public int LifeNumber;
    public float DelayShoot;
    public float DelayTeleport;
    public Color ColorShotVirus;
    public Color ColorShotKMS;
    public int NbContaminatedPlayerToVictory;
    public float RadiusExplosion;
    public float TimeToAreaContamination;


    public string FileName = "GameConfig.JSON";
    // Start is called before the first frame update
    private GameConfig() { }

    private static GameConfig Inst;
    public static GameConfig GetInstance()
    {
        if (Inst == null)
        {
            Inst = new GameConfig();
        }
        return Inst;
    }
    public void UpdateValuesFromJSON(string jsonString)
    {
        JsonUtility.FromJsonOverwrite(jsonString, Inst);
    }

    public string ToJSONString()
    {
        return JsonUtility.ToJson(Inst, true);
    }

    public void LoadConfig()
    {
        string file = TextReader.LoadResourceTextfileFromStreamingAsset(/*"../Assets/StreamingAssets/" + */FileName);
        GameConfig.GetInstance().UpdateValuesFromJSON(file);
        Debug.Log("ConfigLoaded");
        Debug.Log(file);
    }
}

public class TextReader
{
    public static string LoadResourceTextfile(string path)
    {
        // Ajouter
        /*path = Application.dataPath + "/" + path.TrimStart('/');
        string filePath = path.Replace(".json", "");*/

        // Lire dans le dossier Resources
        //TextAsset targetFile = Resources.Load<TextAsset>(filePath);

        return System.IO.File.ReadAllText(path);
    }

    public static string LoadResourceTextfileFromStreamingAsset(string path)
    {
        path = System.IO.Path.Combine(Application.streamingAssetsPath, path);
        return LoadResourceTextfile(path);
    }
}
