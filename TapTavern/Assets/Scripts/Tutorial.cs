using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Tutorial : MonoBehaviour
{
    private static GameObject knight;
    private static GameObject bag;
    private static GameObject attackBtn;
    private static GameObject[] tutorialPrompts;
    private GameObject mainMenu;
    private int attacks = 0;
    private AudioSource sound;
    private void Awake()
    {
        sound = FindObjectOfType<AudioSource>();
        attackBtn = GameObject.Find("Sword");
        mainMenu = GameObject.Find("Main Menu");
        mainMenu.SetActive(false);
        attackBtn.SetActive(false);
        tutorialPrompts = GameObject.FindGameObjectsWithTag("TutPrompts");
        knight = GameObject.Find("Knight");
        bag = GameObject.Find("punchBag");

    }
    // Start is called before the first frame update
    private void Start()
    {
        sound.volume = PlayerPrefs.GetFloat("volume");
        foreach (GameObject prompt in tutorialPrompts)
        {
            if (prompt.name != "Welcome")
            {
                prompt.gameObject.GetComponent<Image>().enabled = false;
                prompt.gameObject.GetComponentInChildren<Text>().enabled = false;
            }
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if(attacks >= 2 && (mainMenu.activeInHierarchy ==false))
        {
            mainMenu.SetActive(true);
        }
    }
    public void MainMenuBtn()
    {
        SceneManager.LoadScene(0);
    }
    public void Advance()
    {
        Debug.Log("Advancing!!");
        bool alreadyAdvanced = false;
        for (int i = 0; i < tutorialPrompts.Length; i++)
        {
            if (!alreadyAdvanced)
            {
                if ((tutorialPrompts[i].gameObject.GetComponent<Image>().enabled == true) && (tutorialPrompts[i].gameObject.GetComponentInChildren<Text>().enabled == true) && (i != (tutorialPrompts.Length - 1)))
                {
                    tutorialPrompts[i].gameObject.GetComponent<Image>().enabled = false;
                    tutorialPrompts[i].gameObject.GetComponentInChildren<Text>().enabled = false;
                    tutorialPrompts[i + 1].gameObject.GetComponent<Image>().enabled = true;
                    tutorialPrompts[i + 1].gameObject.GetComponentInChildren<Text>().enabled = true;
                    alreadyAdvanced = true;
                }
                if (i == tutorialPrompts.Length - 1)
                {
                    tutorialPrompts[i].gameObject.GetComponent<Image>().enabled = false;
                    tutorialPrompts[i].gameObject.GetComponentInChildren<Text>().enabled = false;
                    alreadyAdvanced = true;
                }
            }
        }
    }
    public void SelectBag()
    {
        Advance();
        attackBtn.SetActive(true);
    }
    public void SelectKnight()
    {
        Advance();
    }
    public void Attack()
    {
        attacks++;
        Advance();
        Knight knightScr = knight.GetComponent<Knight>();
        knightScr.SwordAttack(bag);
    }
}
