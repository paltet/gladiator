using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GL_TYPE
{
    Thraex,
    Murmillo,
    Retiarius,
    Secutor
}

[System.Serializable]
public struct GL_Data 
{
    public string gl_name;
    public GL_TYPE gl_type;
    public int gl_startage;
    public int gl_birthmonth;
    public int gl_height;
    public int gl_weight;
    public string gl_origin;

    public int gl_bravery;
    public int gl_luck;
    public int gl_strength;
    public int gl_agility;
    public int gl_skill;
    public int gl_speed;

    public int gl_randomSeed;

    public override int GetHashCode() => (gl_name, gl_origin).GetHashCode();

    public override bool Equals(object? obj) => obj is GL_Data data && Equals(data);
    public bool Equals(GL_Data data) => gl_name == data.gl_name && gl_origin == data.gl_origin;
    public static bool operator ==(GL_Data data1, GL_Data data2) => data1.Equals(data2);
    public static bool operator !=(GL_Data data1, GL_Data data2) => !(data1 == data2);
}


public class Gladiator
{
    int GL_AGEMIN = 18;
    int GL_AGEMAX = 60;

    int GL_HEIMIN = 150;
    int GL_HEIMAX = 230;

    int GL_WEIMIN = 50;
    int GL_WEIMAX = 140;

    int GL_SEEDMAX = 10000;

    private GL_Data data;

    public Gladiator()
    {
        System.Random rnd = new System.Random();
        data.gl_type = GL_TYPE.Thraex;

        data.gl_name = DataManager.Instance.names_getName();
        data.gl_startage = rnd.Next(GL_AGEMAX - GL_AGEMIN + 1) + GL_AGEMIN;
        data.gl_birthmonth = rnd.Next(12) + 1;
        data.gl_height = rnd.Next(GL_HEIMAX - GL_HEIMIN + 1) + GL_HEIMIN;
        data.gl_weight = rnd.Next(GL_WEIMAX - GL_WEIMIN + 1) + GL_WEIMIN;
        data.gl_origin = DataManager.Instance.names_getRegion();

        data.gl_bravery = rnd.Next(10) + 1;
        data.gl_luck = rnd.Next(10) + 1;
        data.gl_strength = rnd.Next(10) + 1;
        data.gl_agility = rnd.Next(10) + 1;
        data.gl_skill = rnd.Next(10) + 1;
        data.gl_speed = rnd.Next(10) + 1;

        data.gl_randomSeed = rnd.Next(GL_SEEDMAX + 1);
    }

    public Gladiator(int weightLevel)
    {
        System.Random rnd = new System.Random();
        data.gl_type = GL_TYPE.Thraex;

        data.gl_name = DataManager.Instance.names_getName();
        data.gl_startage = rnd.Next(GL_AGEMAX - GL_AGEMIN + 1) + GL_AGEMIN;
        data.gl_birthmonth = rnd.Next(12) + 1;
        data.gl_height = rnd.Next(GL_HEIMAX - GL_HEIMIN + 1) + GL_HEIMIN;
        data.gl_weight = rnd.Next(GL_WEIMAX - GL_WEIMIN + 1) + GL_WEIMIN;
        data.gl_origin = DataManager.Instance.names_getRegion();

        data.gl_bravery = rnd.Next(10) + 1;
        data.gl_luck = rnd.Next(10) + 1;
        data.gl_strength = rnd.Next(10) + 1;
        data.gl_agility = rnd.Next(10) + 1;
        data.gl_skill = rnd.Next(10) + 1;
        data.gl_speed = rnd.Next(10) + 1;
    }

    public Gladiator(GL_Data data)
    {
        this.data = data;
    }

    public GL_Data getData()
    {
        return data;
    }

    public string getHeiText()
    {
        return (data.gl_height / 100).ToString() + "," + (data.gl_height % 100).ToString("00") + "m";
    }

    public string getWeiText()
    {
        return (data.gl_weight).ToString() + " Kg";
    }

    public bool Equals(Gladiator gladiator)
    {
        return Gladiator.ReferenceEquals(this, gladiator);
    }

    public int MarketValue()
    {
        int value = data.gl_bravery + data.gl_luck + data.gl_strength + data.gl_agility + data.gl_skill + data.gl_speed;
        return value;
    }
}
