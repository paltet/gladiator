using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GLBattleButton : MonoBehaviour
{
    public GLBattle battle;
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = battle.GetTitle();
    }

    public void Delete()
    {
        GameManager.Instance.RemoveBattle(battle.id);
        GameManager.Instance.LoadScene("Scene_Main");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
