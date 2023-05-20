using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LogoSceneLogic : MonoBehaviour
{
    public TMP_Text loadingText;
    public float secondsChange = 0.5f;
    public float minimumSeconds = 3f;

    private int nDots = 0;
    private bool loadingEnded = false;
    private bool minimumEnded = false;
    private bool hasSave;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loadingTextChange());
        StartCoroutine(minimumTime());
        hasSave = DataManager.Instance.LoadData();
        loadingEnded = true;
        ChangeScene();
    }


    IEnumerator loadingTextChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsChange);
            nDots = (nDots + 1) % 4;
            string dots = new String('.', nDots);
            loadingText.text = "Loading" + dots;
        }
    }

    IEnumerator minimumTime()
    {
        yield return new WaitForSeconds(minimumSeconds);
        minimumEnded = true;
        ChangeScene();
    }

    void ChangeScene()
    {
        if (minimumEnded && loadingEnded)
        {
            if (hasSave) GameManager.Instance.LoadScene("Scene_Main");
            else GameManager.Instance.LoadScene("Scene_NewPlayer");
        }
    }
}
