using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public GLFestival nextFestival;
    public GLBattle nextBattle;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            nextFestival = null;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(string name)
    {   
        SceneManager.LoadScene(name);
    }

    public void NewFestival()
    {
        nextFestival = new GLFestival(DataManager.Instance.getYear(), DataManager.Instance.getMonth());
    }

    public void LoadFestival(GLFestival newFestival)
    {
        this.nextFestival = newFestival;
    }

    public void AddBattle(GLBattle battle)
    {
        nextFestival.battles.Add(battle);
        DataManager.Instance.SaveNextFestivalState();
    }

    public void RemoveBattle(string id)
    {
        foreach(var battle in nextFestival.battles)
        {
            if (battle.id == id)
            {
                nextFestival.battles.Remove(battle);
                DataManager.Instance.SaveNextFestivalState();
                return;
            }
        }
    }

    public void SetNextBattle(GLBattle battle)
    {
        nextBattle = battle;
    }

    public void SaveBattle()
    {
        nextFestival.SaveBattle(nextBattle);
    }

    public bool GladiatorInAnotherBattle(Gladiator gladiator)
    {
        if (GameManager.Instance.nextFestival == null) return false;
        else return GameManager.Instance.nextFestival.GladiatorInFestival(gladiator);
    }
}
