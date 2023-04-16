using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;
using UnityEngine;

[System.Serializable]
public class NamesData
{
    public List<string> regions;
    public List<string> names;
}


public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance { get { return instance; } }

    private string namesDataFile;
    private NamesData namesData = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            namesDataFile = Application.persistentDataPath + "/names.json";

            if (File.Exists(namesDataFile)) namesData = JsonUtility.FromJson<NamesData>(File.ReadAllText(namesDataFile));

            DontDestroyOnLoad(gameObject);
        }
    }

    public string names_getName()
    {
        System.Random rnd = new System.Random();
        return namesData.names[rnd.Next(namesData.names.Count)];
    }

    public string names_getRegion()
    {
        System.Random rnd = new System.Random();
        return namesData.regions[rnd.Next(namesData.regions.Count)];
    }

}
