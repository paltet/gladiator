using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Person : MonoBehaviour
{
    public int seed;
    public bool useRandomSeed;
    public int randomSeedMin, randomSeedMax;
    public SymmetricEntity eyes, eyebrows;
    public VerticalOffset nose, ears;
 
    private SkinColorManager _skinColorManager;
    private HairColorManager _hairColorManager;


    void Start()
    {
        _skinColorManager = GetComponent<SkinColorManager>();
        _hairColorManager = GetComponent<HairColorManager>();
        RandomizeFace();
    }

    public void RandomizeFace()
    {
        Random.InitState(new System.Random().Next());
        if (useRandomSeed)
        {
        seed = Random.Range(randomSeedMin, randomSeedMax);
        }
        else
        {
            Random.InitState(seed);
        }
        
        if(_skinColorManager)_skinColorManager.Init();
        if(_hairColorManager)_hairColorManager.Init();

        eyes.distance = Random.Range(7, 10);
        eyebrows.distance = Random.Range(7, 10);
        nose.verticalOffset = Random.Range(-1, 1);
        ears.verticalOffset = Random.Range(-3, 3);
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            foreach (var catalog in child.GetComponentsInChildren<BodyPartCatalog>())
            {
                catalog.Randomize();
            }
        }

        foreach (var colorizable in transform.GetComponentsInChildren<Colorizable>())
        {
            colorizable.Init();
        }
    }
}