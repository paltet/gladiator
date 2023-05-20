using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinColorManager : MonoBehaviour
{
    public Color skinColor{ get; private set; }
    public Color shadowColor{ get; private set; }
    public Color highlightColor{ get; private set; }

    public Color[] possibleColors;
    
    void Awake()
    {
        Init();
    }

    public void Init()
    {
        skinColor = possibleColors[Random.Range(0, possibleColors.Length)];
        shadowColor = new Color(skinColor.r*0.8f,skinColor.g*0.8f,skinColor.b*0.8f,skinColor.a);
        highlightColor = new Color(skinColor.r*1.2f,skinColor.g*1.2f,skinColor.b*1.2f,skinColor.a);
    }
}