using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartCatalog : MonoBehaviour
{

    public Sprite[] BodyParts;

    public SpriteRenderer[] DestinationBodyParts;
	// Use this for initialization
	void Start ()
	{
	    Randomize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Randomize()
    {
        Sprite randomSprite = GetRandomType();
        for (int i = 0; i < DestinationBodyParts.Length; i++)
        {
            DestinationBodyParts[i].sprite = randomSprite;
        }
    }

    private Sprite GetRandomType()
    {
        return BodyParts[Random.Range(0, BodyParts.Length)];
    }

    
}
