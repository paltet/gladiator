using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSave
{

    public string name;
    public string cityName;
    public int year;
    public int month;
    public int repPoints;
    public int money;
    public List<GL_Data> gladiators;
    public List<GL_Data> marketGladiators;

    public List<GLFestival> festivals;
    public GLFestival nextFestival;

    public int perk_standsLevel;
    public int perk_chambersLevel;
    public int perk_logesLevel;


    public GameSave (string name, string cityname)
    {
        this.name = name;
        this.cityName = cityname;
        this.year = 0;
        this.month = 1;
        this.repPoints = 0;
        this.money = 0;

        this.perk_chambersLevel = 1;
        this.perk_logesLevel = 1;
        this.perk_standsLevel = 1;

        gladiators = new List<GL_Data> ();
        marketGladiators = new List<GL_Data>();
        festivals = new List<GLFestival>();

        for (int i = 0; i < 2; i++)
        {
            Gladiator gl = new Gladiator();
            gladiators.Add(gl.getData());
        }
        NewSoldGladiators();
    }

    public void Continue()
    {
        month++;
        if (month == 13)
        {
            month = 1;
            year++;
        }

        if (nextFestival.totalMoney != 0 && nextFestival.totalXp != 0)
        {
            foreach(GLBattle battle in nextFestival.battles)
            {
                for (int i = 0; i<battle.gladiators.Count; i++)
                {
                    if (battle.results[i] == Result.Dead)
                    {
                        gladiators.Remove(gladiators[i]);
                    }
                }
            }

            festivals.Add (nextFestival);
            this.repPoints += nextFestival.totalXp;
            this.money += nextFestival.totalMoney;
            nextFestival = null;
        }

        int renewMarketEachXMonths = 4;

        if ((year*12 + month) % renewMarketEachXMonths == 0) NewSoldGladiators();
    }

    void NewSoldGladiators()
    {
        int n = (int)UnityEngine.Random.Range(2, 5);

        marketGladiators = new List<GL_Data>();
        for (int i = 0; i < n; i++)
        {
            Gladiator gladiator = new Gladiator();
            marketGladiators.Add(gladiator.getData());
        }
    }

    public void AddGladiator(Gladiator gladiator, int cost)
    {
        marketGladiators.Remove(gladiator.getData());
        gladiators.Add(gladiator.getData());
        money -= cost;
    }
}