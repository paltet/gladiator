using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PreCombatSceneLogic : MonoBehaviour
{
    public TMP_Text dateText;
    public TMP_Text cityText;

    public Transform battlesPanel_transform;
    public GameObject GLNextBattlePanel_Prefab;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GLBattle battle in GameManager.Instance.nextFestival.getBattles())
        {
            GameObject panel = Instantiate(GLNextBattlePanel_Prefab, battlesPanel_transform);
            panel.GetComponentInChildren<TMP_Text>().text = battle.GetTitle();
        }

        if (cityText)
            cityText.text = DataManager.Instance.getCity();
    }

    // Update is called once per frame
    void Update()
    {
        if (dateText != null)
            dateText.text = DataManager.Instance.Date(true);
    }

    public void NextBattle()
    {
        foreach(GLBattle battle in GameManager.Instance.nextFestival.getBattles())
        {
            if (battle.results.Count == 0)
            {
                GameManager.Instance.SetNextBattle(battle);
                Debug.Log(GameManager.Instance.nextBattle.gladiators.Count);
                GameManager.Instance.LoadScene("Scene_Combat");
                return;
            }
        }

        DataManager.Instance.SaveNextFestivalState();
        DataManager.Instance.Continue();
        GameManager.Instance.LoadScene("Scene_Main");
    }
}
 