using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public enum GL_State
{
    Alive,
    Dead,
    Stun,
    Fainted
}

public class GLController : MonoBehaviour
{
    float attackTimer = 0, staminaRestorationTimer = 0;
    float attackSpeedModifier = 0.1f;

    float attack;
    float defense;
    float dodge;
    float movementSpeed;
    float attackSpeed;
    int luck;
    float stamina = 100f;

    public Gladiator gladiator;
    public GLController objective = null;
    public GL_State state = GL_State.Alive;
    public GameObject floatingTextPrefab;

    NavMeshAgent agent;

    Animator animator;

    [Header("Configuration Stats")]
    [Range(0f, 5f)]
    public float nearDistance;

    [Range(1f, 100f)]
    public float movementSpeedMultiplier;

    [Range(0f,25f)]
    public float staminaRestorationRate = 0.5f; // % de stamina que es recupera cada segon


    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        transform.rotation = Quaternion.identity;

        //Debug.Log(gladiator.getData().gl_name + " has objective " + objective.gladiator.getData().gl_name);
    }

    public void Set(Gladiator gladiator)
    {
        this.gladiator = gladiator;
        Configure();
        
    }

    void Configure()
    {
        GL_Data data = gladiator.getData();

        luck = data.gl_luck;

        switch (data.gl_type)
        {
            case GL_TYPE.Thraex:

                attack =        1f * data.gl_skill +        0.7f * data.gl_agility +    0.3f * data.gl_strength +   0.5f * data.gl_bravery;
                defense =       1.5f * data.gl_strength +   0.5f * data.gl_bravery;
                dodge =         2f * data.gl_agility +      0.5f * data.gl_speed;
                movementSpeed = 2.3f * data.gl_speed;
                break;

            default:

                attack = 5;
                defense = 5;
                dodge = 5;
                movementSpeed = 5;

                Debug.LogWarning("GL_TYPE combat stats not configfured. All stats set to 5. GL_TYPE: " + data.gl_type.ToString());
                break;
        }

        attackSpeed = movementSpeed * attackSpeedModifier;
        movementSpeed *= movementSpeedMultiplier * 0.01f;
    }



    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        staminaRestorationTimer += Time.deltaTime;

        if (objective == null || (objective.state == GL_State.Fainted || objective.state == GL_State.Dead))
        {
            objective = CombatLogic.Instance.GetObjectiveRandom(gladiator);
        }
        else if (objective != null)
        {

            Vector2 direction = objective.transform.position - transform.position;
            direction.Normalize();
            Debug.DrawLine(transform.position, transform.position + (Vector3)direction, UnityEngine.Color.red, 0.1f);
            Debug.DrawLine(transform.position - Vector3.left * 0.5f, transform.position - Vector3.left * 0.5f + Vector3.right * stamina * 0.01f, UnityEngine.Color.green, 0.1f);

            if (direction.x < 0) GetComponent<SpriteRenderer>().flipX = true;
            else GetComponent<SpriteRenderer>().flipX = false;

            if (Vector2.Distance(transform.position, objective.transform.position) > nearDistance)
            {
                agent.SetDestination(objective.transform.position);
                agent.speed = movementSpeed;
            }
            else agent.speed = 0;
            
        }

        switch (state)
        {
            case GL_State.Alive:
                if (attackTimer > attackSpeed && ObjectiveIsNear() && stamina > attack)
                {
                    Attack();
                    attackTimer = 0;
                    CombatLogic.Instance.ApplyDeviationF(attackSpeed);
                }
                if (staminaRestorationTimer > 1)
                {
                    stamina = Mathf.Clamp(stamina + staminaRestorationRate, 0, 100);
                    staminaRestorationTimer = 0;
                    //Debug.Log(stamina);
                }
                break;
        }

        GetComponent<SpriteRenderer>().sortingOrder = (int)(-transform.localPosition.y * 100);
    }

    bool ObjectiveIsNear()
    {
        if (objective == null) return false;
        else if (objective.state != GL_State.Alive) return false;
        return Vector2.Distance(transform.position, objective.transform.position) < nearDistance;
    }

    void Attack()
    {
        if (objective.state == GL_State.Alive)
        {
            objective.Hurt(attack, false);
            animator.SetTrigger("Attack1");
            stamina -= CombatLogic.Instance.ApplyDeviationF(8);
        }
    }

    public void Hurt(float attack, bool stun)
    {

        float damage = CombatLogic.Instance.ApplyDeviationF(attack);

        if ((int)Random.Range(0, 100) <= dodge)
        {
            Dodge();
            return;
        }
        
        damage *= CombatLogic.Instance.ApplyDeviationF((100 - defense)*0.01f);

        if (DamageIsCritical(damage))
        {
            Debug.Log("CRITICAL: damage received: " + damage + " stamina: " + stamina);
            Die();
            return;
        }

        stamina = Mathf.Clamp(stamina - damage, 0, 100);
        if (stamina <= 0)
        {
            Faint();
        }
    }

    void ShowText(string text, UnityEngine.Color color)
    {
        if (floatingTextPrefab)
        {
            GameObject textObject = Instantiate(floatingTextPrefab, new Vector3(0, 5), Quaternion.identity, transform);
            textObject.GetComponent<TextMesh>().text = text;
            textObject.GetComponent<TextMesh>().color = color;
        }
    }


    void Dodge()
    {
        animator.SetTrigger("Block");
        ShowText("DODGE", UnityEngine.Color.magenta);
    }

    void Die()
    {
        state = GL_State.Dead; 
        animator.SetBool("noBlood", false);
        animator.SetTrigger("Death");
        ShowText("DEAD", UnityEngine.Color.red);
    }

    void Faint()
    {
        state = GL_State.Fainted;
        animator.SetBool("noBlood", true);
        animator.SetTrigger("Death");
        ShowText("FAINTED", UnityEngine.Color.yellow);
    }

    bool DamageIsCritical(float damage)
    {
        if (damage > stamina) return true;
        float probability = 1 - (stamina - damage)*0.1f;
        return Random.Range(0,1) < probability;
    }
}
