using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLBattle
{
    List<GLController> gladiators;

    public GLBattle(GLController gladiator)
    {
        gladiators.Add(gladiator);
    }


    // Start is called before the first frame update
    void Start()
    {
        gladiators.Capacity = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
