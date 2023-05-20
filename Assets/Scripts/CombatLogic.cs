using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CombatLogic : MonoBehaviour
{
    private static CombatLogic instance;
    public static CombatLogic Instance { get { return instance; } }

    [Header("Arena Size")]
    public int width = 15;
    public int height = 6;

    List<Gladiator> gladiators;
    public GameObject gladiatorPrefab;
    public Transform gladiatorsParent;

    [Range(0F, 100F)]
    public int desvX100;

    public GameObject endingPanel;
    public TMP_Text endingText;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        endingPanel.SetActive(false);
        Transform circle = transform.Find("Circle");
        if (circle == null) Debug.Log("Arena Circle not found.");
        else circle.localScale = new Vector2(width, height);

        gladiators = new List<Gladiator>();
        foreach(GL_Data gL_Data in GameManager.Instance.nextBattle.gladiators)
        {
            Gladiator gladiator = new Gladiator(gL_Data);
            gladiators.Add(gladiator);
        }

        for (int i = 0; i < gladiators.Count; i++)
        {
            GameObject newGl = Instantiate(gladiatorPrefab, gladiatorsParent);
            newGl.transform.position = getPosition(i, gladiators.Count);
            newGl.GetComponent<GLController>().Set(gladiators[i]);
        }
    }

    public GameObject GetObjective(Gladiator gladiator)
    {

        if (gladiatorsParent.childCount < 2) return null;
        else
        {
            List<GLController> controllers = new List<GLController>();

            foreach(Transform gladiatorTR in gladiatorsParent)
            {
                GameObject gladiatorGO = gladiatorTR.gameObject;
                Gladiator obj = gladiatorGO.GetComponent<GLController>().gladiator;
                if (!gladiator.Equals(obj))
                {
                    controllers.Add(gladiatorGO.GetComponent<GLController>());
                }
            }

            /*List<Tuple<Gladiator, int>> ratings;


            GameObject ret =                                                            gladiatorsParent.GetChild(0).gameObject;
            if (gladiator.Equals(ret.GetComponent<GLController>().gladiator)) ret =     gladiatorsParent.GetChild(1).gameObject;

            int retScore = GetObjectiveScore(gladiator, ret.GetComponent<GLController>().gladiator);

            foreach (Transform gladiatorTR in gladiatorsParent)
            {
                GameObject gladiatorGO = gladiatorTR.gameObject;
                Gladiator obj = gladiatorGO.GetComponent<GLController>().gladiator;
                if (!gladiator.Equals(obj))
                {
                    int objScore = GetObjectiveScore(gladiator, obj);
                    if (objScore < retScore)
                    {
                        ret = gladiatorGO;
                        retScore = objScore;
                    }
                }
            }
            return ret;*/
            return GetObjectiveRecursive(gladiator, controllers);
        }
    }

    int GetObjectiveScore(Gladiator gladiator, Gladiator objective)
    {
        GL_Data gl_data = gladiator.getData();
        GL_Data obj_data = objective.getData();

        return obj_data.gl_skill + 2 * (obj_data.gl_strength + obj_data.gl_agility) - ApplyDeviationInt(2*gl_data.gl_bravery);

    }

    GameObject GetObjectiveRecursive(Gladiator gladiator, List<GLController> controllers) 
    {

        List<Tuple<GLController, int>> ratings = new List<Tuple<GLController, int>>();

        foreach(GLController controller in controllers)
        {
            ratings.Add(new Tuple<GLController, int>(controller, GetObjectiveScore(gladiator, controller.gladiator)));
        }

        ratings.Sort((x,y) => x.Item2.CompareTo(y.Item2));

        if (ratings.Count == 0) return null;
        if (ratings.Count < 3) return ratings[0].Item1.gameObject;
        else
        {
            GLController bestCandidate = ratings[0].Item1;
            
            if (bestCandidate.objective != null && bestCandidate.objective.Equals(gladiator)) return bestCandidate.gameObject;
            else
            {
                List<GLController> ret = new List<GLController>();

                foreach (Tuple<GLController, int> pair in ratings)
                {
                    if (!pair.Item1.gladiator.Equals(bestCandidate.gladiator)) ret.Add(pair.Item1);
                    return GetObjectiveRecursive(gladiator, ret);
                }
            }
        }
        return null;
    }

    public GLController GetObjectiveRandom(Gladiator gladiator)
    {
        if (gladiatorsParent.childCount < 2) return null;
        else
        {
            List<GLController> controllers = new List<GLController>();

            foreach (Transform gladiatorTR in gladiatorsParent)
            {
                GameObject gladiatorGO = gladiatorTR.gameObject;
                Gladiator obj = gladiatorGO.GetComponent<GLController>().gladiator;
                if (!gladiator.Equals(obj))
                {
                    controllers.Add(gladiatorGO.GetComponent<GLController>());
                }
            }
            int i = UnityEngine.Random.Range(0, controllers.Count);
            return controllers[i];
        }
    }

    public int ApplyDeviationInt(int from)
    {
        return (int)(from + UnityEngine.Random.Range(-from * desvX100 * 0.01f, from * desvX100 * 0.01f));
    }

    public float ApplyDeviationF(float from)
    {
        return (from + UnityEngine.Random.Range(-from * desvX100 * 0.01f, from * desvX100 * 0.01f));
    }

    /*
    Vector2 randomPosition()
    {
        float phi = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
        float rho = UnityEngine.Random.Range(0f, 1f);

        float x = Mathf.Sqrt(rho) * Mathf.Cos(phi);
        float y = Mathf.Sqrt(rho) * Mathf.Sin(phi);

        x = x * (transform.localScale.x-1) / 2f;
        y = y * (transform.localScale.y-1) / 2f;

        return new Vector2(x, y);
    }*/

    Vector2 getPosition(int order, int total)
    {
        float rX = width / 4;
        float rY = height / 4;

        switch (total)
        {
            case 2:
                return new Vector2((order==0?-rX:rX), 0);
            case 3:
                if (order == 0) return  new Vector2(-rX, 0);
                else return             new Vector2(rX, (order == 1 ? -rY : rY));
            case 4:
                if (order < 2)  return new Vector2((order == 0 ? -rX : rX), 0);
                else            return new Vector2(0, (order == 2 ? -rY : rY));    
            default:
                Debug.LogWarning("Total number of gladiators not available. TOTAL GLADIATORS: " + total.ToString());
                return Vector2.zero;
        }
    }

    private void Update()
    {
        int activeGladiators = 0;
        foreach (Transform gladiatorTR in gladiatorsParent)
        {
            GL_State state = gladiatorTR.gameObject.GetComponent<GLController>().state;
            if (state == GL_State.Alive)
            {
                activeGladiators++;
            }
        }
        if (activeGladiators <= 1) StartCoroutine(EndBattle());
    }

    private void SpawnRandomGladiators(int n)
    {
        foreach(Transform child in gladiatorsParent)
        {
            Destroy(child.gameObject);
        }

        gladiators = new List<Gladiator>();
        for (int i = 0; i < n; i++)
        {
            Gladiator gladiator = new Gladiator();
            gladiators.Add(gladiator);
            GameObject newGl = Instantiate(gladiatorPrefab, gladiatorsParent);
            newGl.transform.position = getPosition(i, n);
            newGl.GetComponent<GLController>().Set(gladiator);
        }
    }

    IEnumerator EndBattle()
    {
        ApplyResults();
        yield return new WaitForSeconds(2);
        SetEndingPanel();
    }

    private void ApplyResults()
    {
        List < GLController > gl = new List<GLController>();
        foreach (Transform child in gladiatorsParent)
        {
            gl.Add(child.gameObject.GetComponent<GLController>());
        }
        GameManager.Instance.nextBattle.ApplyResults(gl);
    }

    private void SetEndingPanel()
    {
        endingPanel.SetActive(true);

        string text = "Battle ended. \n \n Results: ";

        foreach (Transform child in gladiatorsParent)
        {
            text += "\n";

            GLController controller = child.GetComponent<GLController>();
            text += controller.gladiator.getData().gl_name + " is " + controller.state.ToString();
            
        }

        endingText.text = text;
    }

    public void Continue()
    {
        GameManager.Instance.LoadScene("Scene_PreCombat");
    }
}
 