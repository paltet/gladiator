using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewPlayerSceneScript : MonoBehaviour
{

    public TMP_InputField nameInput, cityInput;

    public void Save()
    {
        if (nameInput.text != "" & cityInput.text != "")
        {
            DataManager.Instance.NewSave(nameInput.text, cityInput.text);
            GameManager.Instance.LoadScene("Scene_Main");
        }
    }
}
