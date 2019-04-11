using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : MonoBehaviour
{
    private GameObject knight;
    private Animator knightAnimator;
    private GameObject KnightControls;
    // Start is called before the first frame update
    void Start()
    {
        KnightControls = GameObject.Find("KnightControls");
        knight = GameObject.Find("Knight");
        knightAnimator = knight.GetComponentInChildren<Animator>();
    }
    public void SwordAttack(GameObject selectedEnemy)
    {
        Debug.Log("Sword Button");
        Button[] buttons = KnightControls.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].name == "Sword")
            {
                buttons[i].enabled = false;
                StartCoroutine(AttackCoolDown(1.0f, buttons[i]));
            }
        }
        Vector3 oldPos = knight.transform.position;
        Vector3 newPos = new Vector3(selectedEnemy.transform.position.x - 3.5f, knight.transform.position.y, knight.transform.position.z);
        knight.transform.position = newPos;
        knightAnimator.SetBool("isAttacking", true);
        AnimatorClipInfo[] clipInfo = knightAnimator.GetCurrentAnimatorClipInfo(0);
        float clipLength = clipInfo[0].clip.length;
        Debug.Log("Attacking");
        StartCoroutine(KnightAttackAnimation(clipLength, oldPos));
    }
    public void SpellAttack(GameObject selectedEnemy)
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].name == "Spell")
            {
                buttons[i].enabled = false;
                StartCoroutine(AttackCoolDown(1.0f, buttons[i]));
            }
        }
        Debug.Log("Cast Button");
        Vector3 oldPos = knight.transform.position;
        Vector3 newPos = new Vector3(selectedEnemy.transform.position.x - 3.5f, knight.transform.position.y, knight.transform.position.z);
        knight.transform.position = newPos;
        knightAnimator.SetBool("isCasting", true);
        AnimatorClipInfo[] clipInfo = knightAnimator.GetCurrentAnimatorClipInfo(0);
        float clipLength = clipInfo[0].clip.length;
        Debug.Log("Casting");
        StartCoroutine(KnightSpellAnimation(clipLength, oldPos));
    }

    private IEnumerator KnightAttackAnimation(float t, Vector3 oldPos)
    {
        yield return new WaitForSeconds(t);
        knightAnimator.SetBool("isAttacking", false);
        Debug.Log("Not Attacking");
        if (oldPos != Vector3.one)
        {
            knight.transform.position = oldPos;
        }

    }
    private IEnumerator KnightSpellAnimation(float t, Vector3 oldPos)
    {
        yield return new WaitForSeconds(t);
        knightAnimator.SetBool("isCasting", false);
        Debug.Log("Not Casting");
        if (oldPos != Vector3.one)
        {
            knight.transform.position = oldPos;
        }
    }
    private IEnumerator AttackCoolDown(float t, Button button)
    {
        yield return new WaitForSeconds(t);
        button.enabled = true;
    }

}
