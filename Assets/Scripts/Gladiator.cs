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


[Serializable]
public struct GL_Data 
{
    public string gl_name;
    public GL_TYPE gl_type;
    public int gl_age;
    public int gl_height;
    public int gl_weight;
    public string gl_origin;

    public int gl_bravery;
    public int gl_luck;
    public int gl_strength;
    public int gl_agility;
    public int gl_skill;
    public int gl_speed;
}
public class Gladiator
{
    [SerializeField]
    private GL_Data data;

    public Gladiator()
    {
        int GL_AGEMIN = 18;
        int GL_AGEMAX = 60;

        int GL_HEIMIN = 150;
        int GL_HEIMAX = 230;

        int GL_WEIMIN = 50;
        int GL_WEIMAX = 140;

        System.Random rnd = new System.Random();
        data.gl_type = GL_TYPE.Thraex;

        data.gl_name = DataManager.Instance.names_getName();
        data.gl_age = rnd.Next(GL_AGEMAX - GL_AGEMIN + 1) + GL_AGEMIN;
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

    public GL_Data getData()
    {
        return data;
    }

    public bool Equals(Gladiator gladiator)
    {
        return Gladiator.ReferenceEquals(this, gladiator);
    }
}
