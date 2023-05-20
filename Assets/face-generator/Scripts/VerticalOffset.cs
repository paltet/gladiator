using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VerticalOffset : MonoBehaviour
{

	public int minOffset = -100,maxOffset = 100;
    [Range(-100, 100)]
    public int verticalOffset = 0;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Clamp(verticalOffset, minOffset, maxOffset) / 16f, 0);
    }
}
