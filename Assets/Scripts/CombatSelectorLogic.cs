using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatSelectorLogic : MonoBehaviour
{
    private static CombatSelectorLogic instance;
    public static CombatSelectorLogic Instance { get { return instance; } }


    public Transform selectorPanel_transform;
    public GameObject GLSelectorButton_Prefab;

    public Transform selectedPanel_transform;
    public GameObject GLSelectedButton_Prefab;

    public Button saveButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (Gladiator gl in DataManager.Instance.gladiatorList)
        {
            GameObject button = Instantiate(GLSelectorButton_Prefab, selectorPanel_transform);
            button.GetComponent<GLSelectorButton>().Set(gl);
            button.GetComponent<Button>().interactable = !GameManager.Instance.GladiatorInAnotherBattle(gl);
        }
    }

    private void Update()
    {
        if (selectedPanel_transform.childCount < 2) saveButton.interactable = false;
        else saveButton.interactable = true;
    }

    public bool Select(GLSelectorButton selector)
    {

        if (selectedPanel_transform.childCount < 2 && !GladiatorAlreadySelected(selector))
        {
            GameObject newSelected = Instantiate(GLSelectedButton_Prefab, selectedPanel_transform);
            if (newSelected.GetComponent<GLSelectedButton>() != null)
            {
                newSelected.GetComponent<GLSelectedButton>().Set(selector);
                return true;
            }
            else return false;
        }
        else return false;
    }

    public bool GladiatorAlreadySelected(GLSelectorButton selector)
    {
        foreach(Transform child in selectedPanel_transform)
        {
            GLSelectedButton selected = child.gameObject.GetComponent<GLSelectedButton>();
            if (selected != null && selected.gladiator == selector.gladiator) return true;
        }
        return false;
    }

    public void Save()
    {

        if (GameManager.Instance.nextFestival == null)
        {
            GameManager.Instance.NewFestival();
        }

        List<Gladiator> gladiators = new List<Gladiator>();
        foreach(Transform child in selectedPanel_transform)
        {
            gladiators.Add(child.GetComponent<GLSelectedButton>().gladiator);
        }

        GLBattle battle = new GLBattle(gladiators);

        GameManager.Instance.AddBattle(battle);
        GameManager.Instance.LoadScene("Scene_Main");
    }

    void Test(GLBattle battle)
    {
        string path = Application.persistentDataPath + "/" + "test.json";
        string content = JsonUtility.ToJson(battle, true);
        FileStream fileStream = new FileStream(path, FileMode.Create);
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
        }
    }

    public int WinPercent(Gladiator gladiator)
    {
        float total = 0;
        foreach (Transform child in selectedPanel_transform)
        {
            Gladiator glsum = child.gameObject.GetComponent<GLSelectedButton>().gladiator;
            total += (float)glsum.MarketValue();
        }

        float fret = (float)gladiator.MarketValue() / total * 100f;
        int ret = (int)fret;

        return ret;
    }


    /*
    public void StartCombat()
    {

        if (selectedPanel_transform.childCount >= 2 && selectorPanel_transform.childCount <= 6)
        {

            List<Gladiator> selectedGladiators = new List<Gladiator>();

            foreach(Transform child in selectedPanel_transform)
            {
                if(child.gameObject.GetComponent<GLSelectedButton>() != null)
                {
                    selectedGladiators.Add(child.gameObject.GetComponent<GLSelectedButton>().gladiator);
                }
            }

            //GameManager.Instance.selectedGladiators = selectedGladiators.ToArray();
            //DataManager.Instance.SaveToJson(selectedGladiators);
            SceneManager.LoadScene("Scene_Combat");
        }
    }*/
}
