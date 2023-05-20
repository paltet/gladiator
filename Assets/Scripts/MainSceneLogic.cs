using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class MainSceneLogic : MonoBehaviour
{
    public TMP_Text dateText;
    public TMP_Text reputationText;
    public TMP_Text moneyText;

    public Transform battlesPanel_transform;
    public GameObject GLBattleButton_Prefab;


    private void Start()
    {
        DataManager.Instance.LoadData();

        if (GameManager.Instance.nextFestival != null)
        {
            foreach (GLBattle battle in GameManager.Instance.nextFestival.getBattles())
            {
                GameObject button = Instantiate(GLBattleButton_Prefab, battlesPanel_transform);
                button.GetComponent<GLBattleButton>().battle = battle;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dateText != null)
            dateText.text = DataManager.Instance.Date(true);

        if (reputationText != null)
            reputationText.text = DataManager.Instance.names_getTitle();

        if (moneyText != null)
            moneyText.text = DataManager.Instance.getMoney().ToString();

        if (Input.GetKeyDown(KeyCode.Q)) DataManager.Instance.AddRepPoints(50);
        if (Input.GetKeyDown(KeyCode.W)) DataManager.Instance.AddRepPoints(-50);

    }

    public void Continue()
    {
        Debug.Log(GameManager.Instance.nextFestival == null);
        Debug.Log(GameManager.Instance.nextFestival.battles.Count);
        if (GameManager.Instance.nextFestival != null && GameManager.Instance.nextFestival.battles.Count > 0)
            GameManager.Instance.LoadScene("Scene_PreCombat");
        else DataManager.Instance.Continue();
    }
}
