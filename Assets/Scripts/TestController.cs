using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    [SerializeField]
    public GL_Data gladiator;

    // Start is called before the first frame update
    void Start()
    {
        Gladiator test = new Gladiator();
        gladiator = test.getData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewData()
    {
        Gladiator test = new Gladiator();
        gladiator = test.getData();
    }
}
