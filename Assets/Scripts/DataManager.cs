using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class NamesData
{
    public List<string> regions;
    public List<string> names;
    public List<string> repTitles;
    public List<int> repPoints;
}


public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance { get { return instance; } }

    private string namesDataFile;
    private NamesData namesData = null;

    private GameSave save;
    public List<Gladiator> gladiatorList = new List<Gladiator>();
    public List<Gladiator> marketList = new List<Gladiator>();

    private string gameDataFile = "data.json";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            namesDataFile = Application.persistentDataPath + "/names.json";

            if (File.Exists(namesDataFile)) namesData = JsonUtility.FromJson<NamesData>(File.ReadAllText(namesDataFile));

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
        for(int level = 0; level < namesData.repPoints.Count; level++)
        {
            if (save.repPoints < namesData.repPoints[level]) return namesData.repTitles[level];
        }
        return namesData.repTitles[namesData.repTitles.Count - 1];
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
        FileStream fileStream = new FileStream(path, FileMode.Create);
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

    public void AddMoney(int n) { save.money += 100; SaveData(); }

    public void AddGladiator(Gladiator gladiator, int cost)
    {
        save.AddGladiator(gladiator, cost);
        SaveData();
        LoadGladiators();
    }

    public List<String> getRepTitles()
    {
        return namesData.repTitles;
    }

}