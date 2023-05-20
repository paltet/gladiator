using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GLDetailLogic : MonoBehaviour
{

    private static GLDetailLogic instance;
    public static GLDetailLogic Instance { get { return instance; } }

    public Transform selectorPanel_transform;
    public GameObject GLSelectorButton_Prefab;

    public GameObject profilePanel;

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
        foreach (Gladiator gladiator in DataManager.Instance.gladiatorList)
        {
            GameObject button = Instantiate(GLSelectorButton_Prefab, selectorPanel_transform);
            button.GetComponent<GLSelectorButton>().Set(gladiator);
        }
    }

    public bool Select(GLSelectorButton selector)
    {
        if (profilePanel.GetComponent<ProfilePanel>() != null)
        {
            profilePanel.GetComponent<ProfilePanel>().Set(selector.gladiator);
            return true;
        }
        return false;
    }

}