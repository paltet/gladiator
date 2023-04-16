using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatSelectorLogic : MonoBehaviour
{
    private static CombatSelectorLogic instance;
    public static CombatSelectorLogic Instance { get { return instance; } }


    public Transform selectorPanel_transform;
    public GameObject GLSelectorButton_Prefab;

    public Transform selectedPanel_transform;
    public GameObject GLSelectedButton_Prefab;
    
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


    public void Generate()
    {
        Instantiate(GLSelectorButton_Prefab, selectorPanel_transform);
    }



    public bool Select(GLSelectorButton selector)
    {

        if (selectedPanel_transform.childCount < 6)
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

            GameManager.Instance.selectedGladiators = selectedGladiators.ToArray();
            SceneManager.LoadScene("Scene_Combat");
        }
    }
}
