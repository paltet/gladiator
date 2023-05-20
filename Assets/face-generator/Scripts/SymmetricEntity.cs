using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SymmetricEntity : MonoBehaviour
{

    public GameObject Left, Right;
    [Range(-100, 100)]
    public int distance = 0;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Left.transform.localPosition = new Vector3(-distance / 16f, Left.transform.localPosition.y, 0);
        Right.transform.localPosition = new Vector3(distance / 16f, Right.transform.localPosition.y, 0);
    }
}
