using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class MainSceneLogic : MonoBehaviour
{
    public TMP_Text dateText;
    public TMP_Text moneyText;

    public Transform battlesPanel_transform;
    public GameObject GLBattleButton_Prefab;


    [Header("Reputation Indicator")]
    public TMP_Text currentRepTitleText;
    public TMP_Text currentRepPointsText;
    public TMP_Text nextRepTitleText;
    public TMP_Text nextRepPointsText;
    public RectTransform repSlider;

    Vector2 initialRepSliderRect;

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
        initialRepSliderRect = repSlider.sizeDelta;

    }

    // Update is called once per frame
    void Update()
    {
        if (dateText != null)
            dateText.text = DataManager.Instance.Date(true);

        if (currentRepTitleText != null)
            SetRepIndicator();

        if (moneyText != null)
            moneyText.text = DataManager.Instance.getMoney().ToString();

        if (Input.GetKeyDown(KeyCode.Q)) DataManager.Instance.AddRepPoints(50);
        if (Input.GetKeyDown(KeyCode.W)) DataManager.Instance.AddRepPoints(-50);

    }

    private void SetRepIndicator()
    {
        currentRepTitleText.text = "Current: " + '\n' + DataManager.Instance.names_getTitle();
        nextRepTitleText.text = "Next: " + '\n' + DataManager.Instance.names_getNextTitle();

        int titlePoints = DataManager.Instance.names_getCurrentTitlePoints();
        int currentPoints = DataManager.Instance.getRepPoints();
        int nextPoints = DataManager.Instance.names_getNextTitlePoints();

        currentRepPointsText.text = titlePoints.ToString();
        nextRepPointsText.text = nextPoints.ToString();

        float levelpercent = ((float)currentPoints - (float)titlePoints) / ((float)nextPoints - (float)titlePoints);

        repSlider.sizeDelta = new Vector2(initialRepSliderRect.x * levelpercent, initialRepSliderRect.y);

    }

    public void Continue()
    {
        if (GameManager.Instance.nextFestival != null && GameManager.Instance.nextFestival.battles.Count > 0)
            GameManager.Instance.LoadScene("Scene_PreCombat");
        else DataManager.Instance.Continue();
    }
}
