using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    private GameObject panel;
    private TMP_Text titleLabel;
    private TMP_Text mainLabel;

    [TextAreaAttribute]
    public string titleText;

    [TextAreaAttribute]
    public string mainText;

    // Start is called before the first frame update
    void Start()
    {
        panel = this.transform.Find("Panel").gameObject;
        if (panel != null)
        {
            titleLabel = panel.transform.Find("TitleText").gameObject.GetComponent<TMP_Text>();
            mainLabel = panel.transform.Find("MainText").gameObject.GetComponent<TMP_Text>();

            titleLabel.text = titleText;
            mainLabel.text = mainText;
        }

        panel.SetActive(false);
    }

    public void Switch(bool input)
    {
        panel.SetActive(input);
    }
}
