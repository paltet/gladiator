using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketSceneLogic : MonoBehaviour
{

    private static MarketSceneLogic instance;
    public static MarketSceneLogic Instance { get { return instance; } }

    public Transform selectorPanel_transform;
    public GameObject GLSelectorButton_Prefab;

    public GameObject profilePanel;
    public Button buyButton;
    public GameObject limitText;
    public TMP_Text moneyText;
    
    private ProfilePanel panel;
    private int currentGl;
    private int limitGl;


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
        //DataManager.Instance.LoadData();
        panel = profilePanel.GetComponent<ProfilePanel>();

    }

    private void Start()
    {
        Debug.Log(DataManager.Instance.marketList.Count);
        foreach (Gladiator gladiator in DataManager.Instance.marketList)
        {
            GameObject button = Instantiate(GLSelectorButton_Prefab, selectorPanel_transform);
            button.GetComponent<GLSelectorButton>().Set(gladiator);
        }

        currentGl = DataManager.Instance.getNGladiators();
        limitGl = DataManager.Instance.names_GlAllowed();
    }

    public bool Select(GLSelectorButton selector)
    {
        if (panel != null)
        {
            panel.Set(selector.gladiator);
            return true;
        }
        return false;
    }

    private void Update()
    {
        int money = DataManager.Instance.getMoney();

        //Debug.Log(currentGl + " " + limitGl);

        moneyText.text = money.ToString();

        if (currentGl < limitGl)
        {
            if (panel && panel.gladiator != null && panel.gladiator.MarketValue() > money && currentGl >= limitGl) buyButton.interactable = false;
            else buyButton.interactable = true;


        }
        else
        {
            buyButton.interactable = false;
            limitText.SetActive(true);
            buyButton.GetComponentInChildren<TMP_Text>().text = "MAX";
        }

        /*  
        if (panel && panel.gladiator != null && panel.gladiator.MarketValue() > money && currentGl >= limitGl) buyButton.interactable = false;
        else buyButton.interactable = true;

        moneyText.text = money.ToString();


        if (Input.GetKeyDown(KeyCode.M)) DataManager.Instance.AddMoney(100);

        if (currentGl >= limitGl)
        {
            limitText.SetActive(true);
            buyButton.GetComponentInChildren<TMP_Text>().text = "MAX";
        }
        else
        {
            limitText.SetActive(false);
            buyButton.GetComponentInChildren<TMP_Text>().text = "Buy";

        }
        */
    }

    public void Buy()
    {
        DataManager.Instance.AddGladiator(panel.gladiator, panel.gladiator.MarketValue());
        GameManager.Instance.LoadScene("Scene_Gladiators");
    }

}