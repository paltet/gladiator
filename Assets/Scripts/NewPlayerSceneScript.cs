using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewPlayerSceneScript : MonoBehaviour
{

    public Button button;
    public TMP_InputField nameInput, cityInput;

    public void Save()
    {
        if (nameInput.text != "" & cityInput.text != "")
        {
            DataManager.Instance.NewSave(nameInput.text, cityInput.text);
            GameManager.Instance.LoadScene("Scene_Main");
        }
    }

    private void Update()
    {
        if (nameInput.text != "" & cityInput.text != "")
        {
            button.interactable = true;
        }
        else button.interactable = false;
    }
}
