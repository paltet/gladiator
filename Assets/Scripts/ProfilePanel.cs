using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ProfilePanel : MonoBehaviour
{
    public Gladiator gladiator;

    public TMP_Text nameLabel;
    public TMP_Text originLabel;
    public TMP_Text heightLabel;
    public TMP_Text weightLabel;
    public TMP_Text typeLabel;

    public TMP_Text stValue;
    public TMP_Text spValue;
    public TMP_Text agValue;
    public TMP_Text brValue;
    public TMP_Text skValue;
    public TMP_Text luValue;

    public TMP_Text priceValue;

    public Person face;

    [Header("Attributes colors")]
    public Color bottomColor = Color.red;
    [Range(0, 10)]
    public int bottomColorRangeTop = 4;
    public Color middleColor = Color.yellow;
    [Range(0, 10)]
    public int middleColorRangeBottom = 8;
    public Color topColor = Color.green;

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void Set(Gladiator gladiator)
    {
        this.gameObject.SetActive(true);
        this.gladiator = gladiator;

        nameLabel.text = gladiator.getData().gl_name + ", " + DataManager.Instance.ActualAge(gladiator.getData()).ToString();
        originLabel.text = gladiator.getData().gl_origin.ToString();
        heightLabel.text = gladiator.getHeiText();
        weightLabel.text = gladiator.getWeiText();
        typeLabel.text =  gladiator.getData().gl_type.ToString();

        GL_Data data = gladiator.getData();

        SetValueColor(stValue, data.gl_strength);
        SetValueColor(spValue, data.gl_speed);
        SetValueColor(agValue, data.gl_agility);
        SetValueColor(brValue, data.gl_bravery);
        SetValueColor(skValue, data.gl_skill);
        SetValueColor(luValue, data.gl_luck);

        face.seed = data.gl_randomSeed;
        face.RandomizeFace();

        if (priceValue) priceValue.text = gladiator.MarketValue().ToString();
    }

    void SetValueColor(TMP_Text label, int value)
    {
        label.text = value.ToString();
        if (value <= bottomColorRangeTop) label.color = bottomColor;
        else if (value <= middleColorRangeBottom) label.color = middleColor;
        else label.color = topColor;
    }

}
