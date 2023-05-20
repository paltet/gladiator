using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GLSelectedButton : MonoBehaviour
{
    public Gladiator gladiator;
    public TMP_Text basicInfo_text;
    //public TMP_Text st_text, sp_text, ag_text, br_text, sk_text, lu_text;
    public Person face;

    GLSelectorButton gl_selector;

    private void Update()
    {
        GL_Data data = gladiator.getData();
        basicInfo_text.text = data.gl_name + ", " + data.gl_origin + "\n" + CombatSelectorLogic.Instance.WinPercent(gladiator) + "%";
    }

    public void Set(GLSelectorButton selector)
    {
        this.gladiator = selector.gladiator;
        GL_Data data = gladiator.getData();
        gl_selector = selector;

        basicInfo_text.text = data.gl_name + ", " + data.gl_origin + "\n" + CombatSelectorLogic.Instance.WinPercent(gladiator) + "%";
        face.seed = data.gl_randomSeed;
        face.RandomizeFace();

        /*
        st_text.text = "Strength: " + data.gl_strength.ToString();
        sp_text.text = "Speed: " + data.gl_speed.ToString();
        ag_text.text = "Agility: " + data.gl_agility.ToString();
        br_text.text = "Bravery: " + data.gl_bravery.ToString();
        sk_text.text = "Skill: " + data.gl_skill.ToString();
        lu_text.text = "Luck: " + data.gl_luck.ToString();
        */
    }

    public void Select()
    {
        if (gl_selector != null)
        {
            if (gl_selector.gameObject.GetComponent<Button>() != null)
            {
                gl_selector.gameObject.GetComponent<Button>().interactable = true;
            }
        }
        Destroy(gameObject);
    }
}
