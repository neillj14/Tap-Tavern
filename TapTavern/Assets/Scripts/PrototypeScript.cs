using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrototypeScript : MonoBehaviour
{
    private int dead = 0;
    private int stage = 1;
    private GameObject player;
    private Animator selectedCharAnimator;
    private GameObject KnightControls;
    private GameObject dragonControls;
    private GameObject selectedEnemy;
    private GameObject charGroup;
    private GameObject[] enemies;
    private GameObject[] stage2enemies;
    private GameObject[] characters;
    private GameObject transitionPanel;
    private GameObject enemyGroupStage2;
    private GameObject boss;
    private Image panelImage;
    private GameObject[] HealthBars;
    private GameObject gameOverSplash;
    private GameObject victoryScreen;
    private GameObject pauseMenu;
    private GameObject[] stage2CharPositions;
    private GameObject[] stage3CharPositions;
    private AudioSource sound;
    private bool victory = false;
    private void Awake()
    {
        sound = FindObjectOfType<AudioSource>();
        HealthBars = GameObject.FindGameObjectsWithTag("HealthBars");
        transitionPanel = GameObject.Find("Panel");
        charGroup = GameObject.FindGameObjectWithTag("CharGroup");
        dragonControls = GameObject.FindGameObjectWithTag("DragonControls");
        characters = GameObject.FindGameObjectsWithTag("Characters");
        enemies = GameObject.FindGameObjectsWithTag("Stage1Enemy");
        stage2enemies = GameObject.FindGameObjectsWithTag("Stage2Enemy");
        KnightControls = GameObject.Find("KnightControls");
        enemyGroupStage2 = GameObject.Find("Stage2Enemies");
        enemyGroupStage2.SetActive(false);
        panelImage = transitionPanel.GetComponentInChildren<Image>();
        gameOverSplash = GameObject.Find("GameOver");
        gameOverSplash.SetActive(false);
        pauseMenu = GameObject.Find("Pause");
        boss = GameObject.Find("Boss");
        boss.SetActive(false);
        pauseMenu.SetActive(false);
        stage2CharPositions = GameObject.FindGameObjectsWithTag("Stage2Positioners");
        stage3CharPositions = GameObject.FindGameObjectsWithTag("Stage3Positioners");
        victoryScreen = GameObject.Find("Victory");
        victoryScreen.SetActive(false);
        foreach (GameObject HealthBar in HealthBars)
        {
            switch (HealthBar.name)
            {
                case "KnightHealth":
                    foreach (GameObject character in characters)
                    {
                        if (character.name == "Knight" && character.GetComponent<Character>().gsCurrentHP > 0)
                        {
                            HealthBar.GetComponentInChildren<Text>().text = "Knight Health: " + character.GetComponent<Character>().gsCurrentHP.ToString();
                        }
                    }
                    break;
                case "DragonHealth":
                    foreach (GameObject character in characters)
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
        Debug.Log(KnightControls.name);
    }
    // Start is called before the first frame update
    private void Start()
    {
        sound.volume = PlayerPrefs.GetFloat("volume");
       
    }
    private void Update()
    {
        //Check if a player and an enemy have been selected
        //If controls can be enabled, enalble them
        controlCheck();
        //Check if any player characters are dead and de-activate them if they are
        playerPartyCheck();
        /*Check if any of the enemies in the current stage are dead
         * if any are, de-activate them, if they all are, move to the next
         * stage*/
        if (!victory)
        {
            enemyCheck();
        }
        else
        {
            victoryScreen.SetActive(true);
        }
  
    }
    //Checks for stage completion and any transitions/character deaths
    private void nextStage()
    {
        print("Moving to the next stage");
        dead = 0;
        stage++;
        Camera mainCam = FindObjectOfType<Camera>();
        switch (stage)
        {
            case 2:
                for (int i = 0; i < characters.Length; i++)
                {
                    characters[i].GetComponent<Character>().gsCurrentHP = 100;
                }
                TransitionIn();
                mainCam.transform.position = new Vector3(19.85f, mainCam.transform.position.y, mainCam.transform.position.z);
                charGroup.transform.position = new Vector3(19.4f, -0.01f, charGroup.transform.position.z);
                foreach (GameObject character in characters)
                {
                    switch (character.name)
                    {
                        case "Knight":
                            foreach (GameObject position in stage2CharPositions)
                            {
                                if (position.name == "KnightStage2")
                                {
                                    character.transform.position = position.transform.position;
                                }
                            }
                            break;
                        case "DragonWarrior":
                            foreach (GameObject position in stage2CharPositions)
                            {
                                if (position.name == "DragonStage2")
                                {
                                    character.transform.position = position.transform.position;
                                }
                            }
                            break;
                        default:
                            Debug.Log("RIP");
                            break;
                    }
                }
                enemyGroupStage2.SetActive(true);
                TransitionOut();
                break;
            case 3:
                for (int i = 0; i < characters.Length; i++)
                {
                    characters[i].GetComponent<Character>().gsCurrentHP = 100;
                }
                TransitionIn();
                mainCam.transform.position = new Vector3(40.13f, mainCam.transform.position.y, mainCam.transform.position.z);
                charGroup.transform.position = new Vector3(39.4f, 0.28f, charGroup.transform.position.z);
                foreach (GameObject character in characters)
                {
                    switch (character.name)
                    {
                        case "Knight":
                            foreach (GameObject position in stage3CharPositions)
                            {
                                if (position.name == "KnightStage3")
                                {
                                    character.transform.position = position.transform.position;
                                }
                            }
                            break;
                        case "DragonWarrior":
                            foreach (GameObject position in stage3CharPositions)
                            {
                                if (position.name == "DragonStage3")
                                {
                                    character.transform.position = position.transform.position;
                                }
                            }
                            break;
                        default:
                            Debug.Log("RIP");
                            break;
                    }
                }
                boss.SetActive(true);
                TransitionOut();
                break;
            default:
                print("Yer in trouble now son");
                break;
        }

        selectedEnemy = null;
        player = null;
    }
    private void TransitionIn()
    {
        panelImage.CrossFadeAlpha(1.0f, 1.0f, true);

    }
    private void TransitionOut()
    {
        panelImage.CrossFadeAlpha(0.0f, 1.0f, false);
    }
    private void controlCheck()
    {
        if (isNull(selectedEnemy) || isNull(player))
        {
            //If either player or enemy isnt selected, hide the controls
            Debug.Log("Buttons Off");
            KnightControls.SetActive(false);
            dragonControls.SetActive(false);

        }
        else
        {
            //If both are selected, show the controls
            switch (player.name)
            {
                case "Knight":
                    KnightControls.SetActive(true);
                    dragonControls.SetActive(false);
                    break;
                case "DragonWarrior":
                    dragonControls.SetActive(true);
                    KnightControls.SetActive(false);
                    break;
                default:
                    Debug.Log("Ain't no player done selected");
                    break;
            }
        }
    }
    private void playerPartyCheck()
    {
        bool allDead = false;
        foreach (GameObject character in characters)
        {
            if (character.gameObject.GetComponent<Character>().gsCurrentHP <= 0)
            {
                Animator currentAnimator = character.GetComponent<Animator>();
                AnimatorClipInfo[] charAnimInfo = currentAnimator.GetCurrentAnimatorClipInfo(0);
                float deathAnimLength = charAnimInfo[0].clip.length;
                StartCoroutine(DeathAnimation(deathAnimLength, character));
                allDead = true;
            }
            else
            {
                allDead = false;
            }
        }
        if (allDead)
        {
            gameOverSplash.SetActive(true);
        }
    }
    private void enemyCheck()
    {
        print("Dead enemies:" + dead);
        switch (stage)
        {
            case 1:
                dead = 0;
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (enemies[i].gameObject.GetComponent<Enemy>().gsCurrentHP <= 0)
                    {
                        enemies[i].SetActive(false);
                        dead++;
                        if (selectedEnemy == enemies[i])
                        {
                            selectedEnemy = new GameObject();
                        }
                    }
                }
                if (dead == 2)
                {
                    nextStage();
                }
                break;
            case 2:
                dead = 0;
                print("Dead enemies: " + dead);
                for (int i = 0; i < stage2enemies.Length; i++)
                {
                    if (stage2enemies[i].gameObject.GetComponent<Enemy>().gsCurrentHP <= 0)
                    {
                        stage2enemies[i].SetActive(false);
                        dead++;
                       // if (selectedEnemy == enemies[i])
                       // {
                       //    selectedEnemy = new GameObject();
                       // }
                    }
                }
                if (dead == 3)
                {
                    nextStage();
                }
                break;
            case 3:
                if(boss.GetComponent<Enemy>().gsCurrentHP <= 0)
                {
                    boss.SetActive(false);
                    victory = true;
                }
                break;
            default:
                print("You've killed it");
                break;
        }
    }
    public bool isNull(GameObject obj)
    {
        //Try and get the name of the GameObject, if it returns anything, the object is selected
        //if it throws a NullReference Exception, it's unselected.
        try
        {
            string name = obj.name;
            return false;
        }
        catch (NullReferenceException)
        {
            return true;
        }
    }

    //Player
    public void SelectPlayer(GameObject selected)
    {
        player = selected;
        selectedCharAnimator = selected.GetComponent<Animator>();
        Debug.Log(selected.name);
    }
    public void SelectEnemy(GameObject selected)
    {
        selectedEnemy = selected;
        Debug.Log(selectedEnemy.name);
    }

    public void DamageSelected(float damage)
    {
        selectedEnemy.gameObject.GetComponent<Enemy>().gsCurrentHP = damage;
    }

    //Button Press
    public void AbilityButton(Button buttonClicked)
    {
        switch (player.name)
        {
            case "Knight":
                Knight knightScr = player.GetComponent<Knight>();
                switch (buttonClicked.name)
                {
                    case "Sword":
                        if ((selectedEnemy.gameObject.GetComponentInChildren<Enemy>().gsCurrentHP - 20 <= 0))
                        {
                            int numLeft = 0;
                            foreach (GameObject enemy in enemies)
                            {
                                if (enemy.activeSelf)
                                {
                                    numLeft++;
                                }
                            }
                            if (numLeft <= 1)
                            {
                                selectedEnemy.GetComponent<Enemy>().gsCurrentHP = 10;
                            }
                        }
                        else
                        {
                            knightScr.SwordAttack(selectedEnemy);
                        }
                        break;
                    default:
                        Debug.Log("She's dead Jim");
                        break;

                }
                break;
            case "DragonWarrior":
                DragonWarrior dragWarScr = player.GetComponent<DragonWarrior>();
                switch (buttonClicked.name)
                {
                    case "Fire":
                        if ((selectedEnemy.gameObject.GetComponentInChildren<Enemy>().gsCurrentHP - 20 <= 0))
                        {
                            int numLeft = 0;
                            foreach (GameObject enemy in enemies)
                            {
                                if (enemy.activeSelf)
                                {
                                    numLeft++;
                                }
                            }
                            if (numLeft <= 1)
                            {
                                selectedEnemy.GetComponent<Enemy>().gsCurrentHP = 10;
                            }
                        }
                        else
                        {
                            dragWarScr.DragonWarriorFire(selectedEnemy);
                        }
                        break;
                    default:
                        Debug.Log("She's dead Jim");
                        break;
                }
                break;
            default:
                Debug.Log("She's Dead Jim");
                break;
        }
    }
    public void PauseButton()
    {
        Button[] allButtons = FindObjectsOfType<Button>();
        for (int i = 0; i < allButtons.Length; i++)
        {
            if (allButtons[i].name == "PauseBtn")
            {
                allButtons[i].enabled = true;
            }
        }
        pauseMenu.SetActive(true);
    }
    public void ResumeButton()
    {
        pauseMenu.SetActive(false);
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        foreach(GameObject character in characters)
        {
            character.GetComponent<Character>().gsCurrentHP = 120;
        }
    }
    public void QuitGameButton()
    {
        Application.Quit(0);
    }
    //Cooldowns

    private IEnumerator StageTransition(float t)
    {
        yield return new WaitForSecondsRealtime(t);
    }
    private IEnumerator DeathAnimation(float t, GameObject character)
    {
        yield return new WaitForSeconds(t);
        character.SetActive(false);
    }
    private IEnumerator AttackCoolDown(float t, Button button)
    {
        yield return new WaitForSeconds(t);
        button.enabled = true;
    }
}
