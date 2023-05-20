using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairColorManager : MonoBehaviour
{
    public Color[] PossibleHairColors;

    public Color hairColor { get; private set; }

    // Use this for initialization
    void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Init()
    {
        hairColor = PossibleHairColors[Random.Range(0, PossibleHairColors.Length)];
    }
}