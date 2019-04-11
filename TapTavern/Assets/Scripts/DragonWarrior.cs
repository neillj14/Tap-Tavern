using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonWarrior : MonoBehaviour
{
    private static GameObject thisDragonWarrior;
    private GameObject DragonControls;
    private Animator dragonAnimator;
    // Start is called before the first frame update
    void Start()
    {
        DragonControls = GameObject.Find("DragonControls");
        thisDragonWarrior = GameObject.Find("DragonWarrior");
        dragonAnimator = thisDragonWarrior.GetComponentInChildren<Animator>();
    }
    public void DragonWarriorFire(GameObject selectedEnemy)
    {
        Debug.Log("Fire Button");
        Button[] buttons = DragonControls.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].name == "Fire")
            {
                buttons[i].enabled = false;
                StartCoroutine(AttackCoolDown(1.0f, buttons[i]));
            }
        }
        Vector3 oldPos = thisDragonWarrior.transform.position;
        Vector3 newPos = new Vector3(selectedEnemy.transform.position.x - 3.5f, thisDragonWarrior.transform.position.y, thisDragonWarrior.transform.position.z);
        thisDragonWarrior.transform.position = newPos;
        dragonAnimator.SetBool("attacking", true);
        AnimatorClipInfo[] clipInfo = dragonAnimator.GetCurrentAnimatorClipInfo(0);
        float clipLength = clipInfo[0].clip.length;
        Debug.Log("Firing");
        StartCoroutine(DragonFireAttack(clipLength, oldPos));
    }
    private IEnumerator DragonFireAttack(float t, Vector3 oldPos)
    {
        yield return new WaitForSeconds(t);
        dragonAnimator.SetBool("attacking", false);
        thisDragonWarrior.transform.position = oldPos;
    }
    private IEnumerator AttackCoolDown(float t, Button button)
    {
        yield return new WaitForSeconds(t);
        button.enabled = true;
    }
}
