using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    //BaseStats
    Animator thisAnimator;
    private const float MAX_HP = 100;
    private float CurrentHP = MAX_HP;
    private bool canAttack = true;
    // Target value corresponds to index in player array
    private int target;
    private GameObject[] HealthBars;


    GameObject[] players;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Characters");
        thisAnimator = gameObject.GetComponent<Animator>();
        HealthBars = GameObject.FindGameObjectsWithTag("HealthBars");
    }

    public float gsCurrentHP
    {
        get
        {
            return CurrentHP;
        }
        set
        {
            if (CurrentHP - value <= 0)
            {
                CurrentHP = 0;
            }
            else
            {
                CurrentHP -= value;
            }
        }
    }
    void Update()
    {
        target = UnityEngine.Random.Range(0, players.Length);
        if (canAttack)
        {
            bool allDead = false;
            for (int i = 0; i < players.Length; i++)
            {
                try
                {
                    if (players[i].GetComponent<Character>().gsCurrentHP <= 0)
                    {
                        allDead = true;
                    }
                    else
                    {
                        allDead = false;
                    }
                }
                catch (Exception)
                {
                    allDead = true;
                }
            }
            if (!allDead)
            {
                Attack();
                foreach (GameObject HealthBar in HealthBars)
                {
                    switch (HealthBar.name)
                    {
                        case "KnightHealth":
                            foreach (GameObject character in players)
                            {
                                if (character.name == "Knight" && character.GetComponent<Character>().gsCurrentHP > 0)
                                {
                                    HealthBar.GetComponentInChildren<Text>().text = "Knight Health: " + character.GetComponent<Character>().gsCurrentHP.ToString();
                                }
                            }
                            break;
                        case "DragonHealth":
                            foreach (GameObject character in players)
                            {
                                if (character.name == "DragonWarrior" && character.GetComponent<Character>().gsCurrentHP > 0)
                                {
                                    HealthBar.GetComponentInChildren<Text>().text = "Dragon Warrior Health: " + character.GetComponent<Character>().gsCurrentHP.ToString();
                                }
                            }
                            break;
                        default:
                            Debug.Log("She's dead Jim");
                            break;
                    }
                }

            }
        }
    }

    void Attack()
    {
        // Deal Damage
        float Damage;
        if(gameObject.name != "Boss")
        {
            Damage = 10;
        }
        else
        {
            Damage = 25;
        }
        float TargetHealth = players[target].gameObject.GetComponentInChildren<Character>().gsCurrentHP;
        Debug.Log(gameObject.name + " is attacking");
        //Set enemy animator to attack state
        thisAnimator.SetTrigger("attack");
        AnimatorClipInfo[] attackInfo = thisAnimator.GetCurrentAnimatorClipInfo(0);
        float clipLength = attackInfo[0].clip.length;
        StartCoroutine(AttackAnim(clipLength));
        //Deal Damage
        print("Attacking " + players[target].name);
        players[target].gameObject.GetComponentInChildren<Character>().gsCurrentHP = TargetHealth - Damage;

        //Play Player hurt animation animation
        Animator damagedCharAnimator = players[target].GetComponent<Animator>();
        switch (players[target].name)
        {
            case "Knight":
                damagedCharAnimator.SetBool("isHurt", true);
                damagedCharAnimator.SetFloat("health", (damagedCharAnimator.GetFloat("health") - Damage));
                damagedCharAnimator.SetBool("isHurt", false);
                break;
            case "DragonWarrior":
                damagedCharAnimator.SetBool("hurt", true);
                damagedCharAnimator.SetFloat("health", (damagedCharAnimator.GetFloat("health") - Damage));
                damagedCharAnimator.SetBool("hurt", false);
                break;
        }
       
        canAttack = false;
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        print(Time.time);
        yield return new WaitForSeconds(10.0f);
        print(Time.time);
        canAttack = true;
    }
    private IEnumerator AttackAnim(float t)
    {
        yield return new WaitForSeconds(t);
        thisAnimator.SetTrigger("attack");
    }
}