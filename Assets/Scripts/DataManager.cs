using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using JetBrains.Annotations;

[System.Serializable]
public class NamesData
{
    public List<string> regions;
    public List<string> names;
    public List<string> repTitles;
    public List<int> repPoints;
    public List<float> perkStandsMoneyMult;
    public List<float> perkLogesRepMult;
    public List<int> perkChambersGlAllowed;
}


public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance { get { return instance; } }

    //private string namesDataFile;
    private NamesData namesData = null;

    private GameSave save;
    public List<Gladiator> gladiatorList = new List<Gladiator>();
    public List<Gladiator> marketList = new List<Gladiator>();

    public TextAsset names;

    private string gameDataFile = "data.json";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //namesDataFile = "Assets/Data/" + "names.json";

            //if (File.Exists(namesDataFile)) namesData = JsonUtility.FromJson<NamesData>(File.ReadAllText(namesDataFile));
            if (names != null) namesData = JsonUtility.FromJson<NamesData>(names.ToString());
            else Debug.LogWarning("Namesdata not found.");

            Debug.Log(namesData.regions.Count);

            DontDestroyOnLoad(gameObject);


            #if UNITY_EDITOR

            LoadData();
            //LoadGladiators();
            //LoadNextFestival();

            #endif
        }
    }

    private void Update()
    {
        //  if (Input.GetKeyDown(KeyCode.A)) Continue();
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

    public string names_getTitle()
    {
        for(int level = 1; level < namesData.repPoints.Count; level++)
        {
            if (save.repPoints < namesData.repPoints[level]) return namesData.repTitles[level-1];
        }
        return namesData.repTitles[namesData.repTitles.Count - 1];
    }

    public string names_getNextTitle()
    {
        for (int level = 1; level < namesData.repPoints.Count; level++)
        {
            if (save.repPoints < namesData.repPoints[level]) return namesData.repTitles[level];
        }
        return namesData.repTitles[namesData.repTitles.Count - 1];
    }

    private int names_getTitlePoints(string title)
    {
        for (int level = 0; level < namesData.repPoints.Count; level++)
        {
            if (title == namesData.repTitles[level]) return namesData.repPoints[level];
        }
        return namesData.repPoints[namesData.repTitles.Count - 1];
    }

    public int names_getCurrentTitlePoints()
    {
        return names_getTitlePoints(names_getTitle());
    }

    public int names_getNextTitlePoints()
    {
        return names_getTitlePoints(names_getNextTitle());
    }

    public int getRepPoints()
    {
        return save.repPoints;
    }

    public void AddRepPoints(int points)
    {
        save.repPoints += points;
    }

    public void NewSave(string name, string cityname)
    {
        save = new GameSave(name, cityname);
        SaveData();
        LoadGladiators();
    }

    public bool LoadData()
    {
        string data =ReadFile(GetPath(gameDataFile));

        if (string.IsNullOrEmpty(data) || data == "[]") return false;
        else
        {
            save = JsonUtility.FromJson<GameSave>(data);
            LoadGladiators();
            LoadNextFestival();
            return true;
        }
    }

    private void SaveData() { 

        string content = JsonUtility.ToJson(save, true);
        WriteFile(GetPath(gameDataFile), content);
    }

    string GetPath(string filename)
    {
        return Application.persistentDataPath + "/" + filename;
    }

    void WriteFile(string path, string content)
    {
        FileStream fileStream = new FileStream(path, System.IO.FileMode.Create);
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
        }
    }

    string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        else return "";
    }

    public string Date(bool roman)
    {
        if (save == null) return "";
        else
        {
            if (roman) return ("Year " + IntToRoman(save.year) + " Month " + IntToRoman(save.month));
            else return ("Year " + save.year.ToString() + " Month " + save.month.ToString());
        }
    }

    string IntToRoman(int n)
    {
        switch (n)
        {
            case 0: return "0";
            case 1: return "I";
            case 2: return "II";
            case 3: return "III";
            case 4: return "IV";
            case 5: return "V";
            case 6: return "VI";
            case 7: return "VII";
            case 8: return "VIII";
            case 9: return "IX";
            case 10: return "X";
            case 11: return "XI";
            case 12: return "XII";
            case 13: return "XIII";
            case 14: return "XIV";
            case 15: return "XV";
            case 16: return "XVI";
            case 17: return "XVII";
            case 18: return "XVIII";
            case 19: return "XIX";
            case 20: return "XX";
            default: return "-";
        }
    }

    public void Continue()
    {
        Debug.Log("Continue");
        //if (save == null) LoadData();
        save.Continue();
        SaveData();
    }

    private void LoadGladiators()
    {
        Debug.Log("gl loaded");

        gladiatorList = new List<Gladiator>();
        foreach(GL_Data data in save.gladiators)
        {
            Gladiator newgl = new Gladiator(data);
            gladiatorList.Add(newgl);
        }
        marketList = new List<Gladiator>();
        foreach(GL_Data data in save.marketGladiators)
        {
            Gladiator newgl = new Gladiator(data);
            marketList.Add(newgl);
        }
    }

    private void LoadNextFestival()
    {
        if(save.nextFestival != null && GameManager.Instance != null)
        {
            GameManager.Instance.LoadFestival(save.nextFestival); 
            //GameManager.Instance.nextFestival = save.nextFestival;
        }
    }

    public void SaveNextFestivalState()
    {
        save.nextFestival = GameManager.Instance.nextFestival;
        SaveData();
    }

    public int ActualAge(GL_Data data)
    {
        int starting = data.gl_startage;
        int birthday = data.gl_birthmonth;

        int age = starting + save.year;
        if (save.month >= birthday) age++;
        return age;
    }

    public int getYear() { if (save != null) return save.year; else return 0; }
    public int getMonth() { if (save != null) return save.month; else return 0; }

    public int getMoney() { return save.money; }

    public string getCity() { return save.cityName; }

    public int getPerkLevel(string perk)
    {
        switch (perk)
        {
            case "stands":
                return save.perk_standsLevel;
            case "loges":
                return save.perk_logesLevel;
            case "chambers":
                return save.perk_chambersLevel;
            default:
                Debug.LogError("ERROR: Perk " + perk + " level not found");
                return -1;
        }
    }

    public float names_MoneyMult()
    {
        if (save.perk_standsLevel < 1 || save.perk_standsLevel > 4) return 1f;
        else return namesData.perkStandsMoneyMult[save.perk_standsLevel];
    }

    public float names_RepMult()
    {
        if (save.perk_logesLevel < 1 || save.perk_logesLevel > 4) return 1f;
        else return namesData.perkLogesRepMult[save.perk_logesLevel];
    }

    public int names_GlAllowed()
    {
        Debug.Log(save.perk_chambersLevel);
        if (save.perk_chambersLevel < 1 || save.perk_chambersLevel > 4) return 2;
        else return namesData.perkChambersGlAllowed[save.perk_chambersLevel-1];
    }

    public void AddMoney(int n) { save.money += n; SaveData(); }

    public void AddGladiator(Gladiator gladiator, int cost)
    {
        save.AddGladiator(gladiator, cost);
        SaveData();
        LoadGladiators();
    }

    public void UpgradePerk(string perk, int cost)
    {
        switch (perk)
        {
            case "stands":
                if (save.perk_standsLevel < 4) save.perk_standsLevel++;
                break;
            case "loges":
                if (save.perk_logesLevel < 4) save.perk_logesLevel++;
                break;
            case "chambers":
                if (save.perk_chambersLevel < 4) save.perk_chambersLevel++;
                break;
            default:
                Debug.LogError("ERROR: Perk " + perk + " not found");
                return;
        }
        save.money -= cost;
        SaveData();
    }

    public int getNGladiators()
    {
        return save.gladiators.Count;
    }

}