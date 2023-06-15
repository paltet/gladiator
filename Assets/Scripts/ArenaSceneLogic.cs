using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ArenaPerk
{
    Main,
    Stands,
    Chambers,
    Loges
}

public class ArenaSceneLogic : MonoBehaviour
{
    public ArenaPerk perk;

    public TMP_Text moneyText;

    public TMP_Text standsText;
    public TMP_Text chambersText;
    public TMP_Text logesText;

    public GameObject buyButton;

    public Sprite[] standsSprites;
    public Sprite[] chambersSprites;
    public Sprite[] logesSprites;

    public Image backImage;
    public Image backUpgradeImage;
    public Image front1Image;
    public Image front2Image;
    public Image frontUpgradeImage;

    private int upgradeCost;
    private int standsLevel;
    private int chambersLevel;
    private int logesLevel;

    // Start is called before the first frame update
    void Start()
    {
        standsLevel = DataManager.Instance.getPerkLevel("stands");
        chambersLevel = DataManager.Instance.getPerkLevel("chambers");
        logesLevel = DataManager.Instance.getPerkLevel("loges");

        if (standsText) standsText.text = "Grandstands\nLevel " + standsLevel;
        if (chambersText) chambersText.text = "Chambers\nLevel " + chambersLevel;
        if (logesText) logesText.text = "Loges\nLevel " + logesLevel;

        upgradeCost = -1;
        
        switch (perk)
        {
            case ArenaPerk.Stands:
                upgradeCost = getImprovementCost(standsLevel + 1);
                break;
            case ArenaPerk.Chambers:
                upgradeCost = getImprovementCost(chambersLevel + 1);
                break;
            case ArenaPerk.Loges:
                upgradeCost = getImprovementCost(logesLevel + 1);
                break;
            default: break;
        }

        if (perk != ArenaPerk.Main)
        {
            if (upgradeCost > DataManager.Instance.getMoney() || upgradeCost == 0) buyButton.GetComponent<Button>().interactable = false;
            else buyButton.GetComponent<Button>().interactable = true;



            buyButton.GetComponentInChildren<TMP_Text>().text = (upgradeCost != 0 ? upgradeCost.ToString() : "MAX");
        }
        SetArenaPlan();
    }

    // Update is called once per frame
    void Update()
    {
        if (moneyText != null)
            moneyText.text = DataManager.Instance.getMoney().ToString();

        if (Input.GetKeyDown(KeyCode.M)) DataManager.Instance.AddMoney(1000);
    }

    int getImprovementCost(int level)
    {
        switch (level)
        {
            case 2: return 500;
            case 3: return 1500;
            case 4: return 3000;
            default: return 0;
        }
    }

    public void Upgrade()
    {
        switch (perk)
        {
            case ArenaPerk.Stands:
                DataManager.Instance.UpgradePerk("stands", upgradeCost);
                break;
            case ArenaPerk.Chambers:
                DataManager.Instance.UpgradePerk("chambers", upgradeCost);
                break;
            case ArenaPerk.Loges:
                DataManager.Instance.UpgradePerk("loges", upgradeCost);
                break;
            default: break;
        }

        GameManager.Instance.LoadScene("Scene_Arena");
    }

    Sprite getSpriteFromPerkLevel(string perk, int level)
    {
        switch (perk)
        {
            case "stands":
                if (level <= standsSprites.Length) return standsSprites[level - 1];
                return null;
            case "loges":
                if (level <= logesSprites.Length) return logesSprites[level - 1];
                else return null;
            case "chambers":
                if (level <= chambersSprites.Length) return chambersSprites[level - 1];
                else return null;
            default:
                Debug.LogError("ERROR: Perk " + perk + " level not found");
                return null;
        }
    }


    void SetArenaPlan()
    {
        Color transparent = Color.white;
        transparent.a = 0f;

        Sprite standsSp, chambersSp, logesSp;
        Sprite upgradeSp;

        standsSp = getSpriteFromPerkLevel("stands", standsLevel);
        chambersSp = getSpriteFromPerkLevel("chambers", chambersLevel);
        logesSp = getSpriteFromPerkLevel("loges", logesLevel);

        if (standsSp == null || chambersSp == null || logesSp == null) { Debug.LogWarning("ERROR: Perk sprites array overflow."); return; }

        switch (perk)
        {
            case ArenaPerk.Main:
                backImage.sprite = chambersSp;
                front1Image.sprite = logesSp;
                front2Image.sprite = standsSp;
                backUpgradeImage.color = transparent;
                frontUpgradeImage.color = transparent;
                break;

            case ArenaPerk.Stands:
                backImage.sprite = chambersSp;
                backUpgradeImage.color = transparent;

                upgradeSp = getSpriteFromPerkLevel("stands", standsLevel + 1);
                if (upgradeSp == null) frontUpgradeImage.color = transparent; else frontUpgradeImage.sprite = upgradeSp;

                front1Image.sprite = standsSp;
                front2Image.color = transparent;
                break;

            case ArenaPerk.Chambers:
                front1Image.sprite = standsSp;
                front2Image.sprite = logesSp;
                frontUpgradeImage.color = transparent;
                backImage.sprite = chambersSp;

                upgradeSp = getSpriteFromPerkLevel("chambers", chambersLevel + 1);
                if (upgradeSp == null) backUpgradeImage.color = transparent; else backUpgradeImage.sprite = upgradeSp;

                break;

            case ArenaPerk.Loges:
                backImage.sprite = chambersSp;
                backUpgradeImage.color = transparent;

                upgradeSp = getSpriteFromPerkLevel("loges", logesLevel + 1);
                if (upgradeSp == null) frontUpgradeImage.color = transparent; else frontUpgradeImage.sprite = upgradeSp;

                front1Image.sprite = logesSp;
                front2Image.color = transparent;
                break;
        }
    }
}
