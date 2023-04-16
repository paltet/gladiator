using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GLSelectorButton : MonoBehaviour
{
    public  Gladiator gladiator;
    public TMP_Text basicInfo_text, st_text, sp_text, ag_text, br_text, sk_text, lu_text;

    void Start()
    {
        gladiator = new Gladiator();

        GL_Data data = gladiator.getData();
        basicInfo_text.text = data.gl_name + ", " + data.gl_origin;

        st_text.text = "Strength: " + data.gl_strength.ToString();
        sp_text.text = "Speed: " + data.gl_speed.ToString();
        ag_text.text = "Agility: " + data.gl_agility.ToString();
        br_text.text = "Bravery: " + data.gl_bravery.ToString();
        sk_text.text = "Skill: " + data.gl_skill.ToString();
        lu_text.text = "Luck: " + data.gl_luck.ToString();
    }

    public void Select()
    {
        if (CombatSelectorLogic.Instance != null)
        {
            if (CombatSelectorLogic.Instance.Select(this))
                if (GetComponent<Button>() != null)
                    GetComponent<Button>().interactable = false;
        }
    }

    public void Delete()
    {
        Destroy(this.gameObject);
    }
}
