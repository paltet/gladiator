using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Result
{
    Winner,
    Fainted,
    Dead
}

[System.Serializable]
public class GLBattle
{
    public string id;
    public List<GL_Data> gladiators;
    public List<Result> results;
    public int xpEarned = 0;
    public int moneyEarned = 0;

    public GLBattle(List<Gladiator> gladiators)
    {
        this.gladiators = new List<GL_Data>();
        this.results = new List<Result>();
        string id = "";

        foreach (Gladiator gladiator in gladiators)
        {
            this.gladiators.Add(gladiator.getData());
            id = id + gladiator.getData().gl_name.Substring(0, 3) + "_";
        }
        this.id = id+DataManager.Instance.getYear().ToString() + "_" + DataManager.Instance.getMonth().ToString();
    }

    public bool GladiatorInBattle(Gladiator gladiator)
    {
        foreach(GL_Data data in gladiators)
        {
            if (data == gladiator.getData()) return true;
        }
        return false;
    }

    public string GetTitle()
    {
        string title = this.gladiators[0].gl_name;
        for (int i = 1; i < this.gladiators.Count; i++)
        {
            title += " vs " + this.gladiators[i].gl_name;
        }
        return title;
    }

    public void ApplyResults(List<GLController> controllers)
    {
        gladiators = new List<GL_Data>();
        results = new List<Result>();
        Result result;

        foreach (GLController controller in controllers)
        {
            switch (controller.state)
            {
                case GL_State.Alive:
                    result = Result.Winner;
                    break;
                case GL_State.Dead:
                    result = Result.Dead;
                    break;
                case GL_State.Fainted:
                    result = Result.Fainted;
                    break;
                default:
                    result = Result.Dead;
                    break;
            }

            gladiators.Add(controller.gladiator.getData());
            results.Add(result);
        }

        moneyEarned = GetMoney();
        xpEarned = GetXP();
        GameManager.Instance.SaveBattle();
    }

    private int GetMoney()
    {
        return (int)(100 * DataManager.Instance.names_MoneyMult());
    }

    private int GetXP()
    {
        return (int)(50 * DataManager.Instance.names_RepMult());
    }
}

[System.Serializable]
public class GLFestival
{
    public List<GLBattle> battles;
    public int year;
    public int month;
    public int totalMoney = 0;
    public int totalXp = 0;

    public GLFestival(int year, int month)
    {
        this.year = year;
        this.month = month;
        battles = new List<GLBattle>();
    }

    public void AddBattle(GLBattle battle)
    {
        battles.Add(battle);
    }

    public bool GladiatorInFestival(Gladiator gladiator)
    {
        bool ret = false;
        foreach (GLBattle battle in battles)
        {
            ret = ret || battle.GladiatorInBattle(gladiator);
        }
        return ret;
    }

    public List<GLBattle> getBattles()
    {
        return battles;
    }

    public void SaveBattle(GLBattle newbattle)
    {
        int index = battles.FindIndex(s => s.id == newbattle.id);
        if (index != -1) battles[index] = newbattle;

        int xp = 0;
        int money = 0;

        foreach(GLBattle battle in battles)
        {
            if (battle.results.Count == 0) return;
            xp += battle.xpEarned;
            money += battle.moneyEarned;
        }

        this.totalXp = xp;
        this.totalMoney = money;
    }
}