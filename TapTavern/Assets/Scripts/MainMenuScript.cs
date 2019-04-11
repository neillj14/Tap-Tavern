using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    private Slider volume;
    private Toggle ads;
    private GameObject settingsMenu;
    private GameObject mainMenu;
    private int utcSinceLastLifeLost;
    private int timeUntilNextLife;
    private Text livesLeft;
    private Text timeUntilNextLifeText;
    private AudioSource soundSource;
    public void Start()
    {
        soundSource = FindObjectOfType<AudioSource>();
        volume = FindObjectOfType<Slider>();
        ads = FindObjectOfType<Toggle>();
        mainMenu = GameObject.Find("MainMenu");
        settingsMenu = GameObject.Find("SettingsMenu");
        settingsMenu.SetActive(false);
        Text[] textObjects = FindObjectsOfType<Text>();
        for (int i = 0; i < textObjects.Length; i++)
        {
            if (textObjects[i].name == "LivesRemaining")
            {
                livesLeft = textObjects[i];
            }
            else if (textObjects[i].name == "TimeUntilNext")
            {
                timeUntilNextLifeText = textObjects[i];
            }
        }
        int numScenes = SceneManager.sceneCount;
        if (numScenes > 1)
        {
            for (int i = 0; i < numScenes; i++)
            {
                SceneManager.UnloadSceneAsync(numScenes);
            }
        }
        LoadSettings();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("volume", volume.value);
        PlayerPrefs.SetFloat("ads", Convert.ToSingle(ads.isOn));
        LoadSettings();
    }
    private bool LoadSettings()
    {
        switch (PlayerPrefs.HasKey("lives"))
        {
            case true:
                livesLeft.text = Convert.ToString(PlayerPrefs.GetInt("lives"));
                break;
            case false:
                livesLeft.text = "5";
                timeUntilNextLifeText.text = " ";
                break;
        }
        if (PlayerPrefs.HasKey("volume"))
        {
            if (PlayerPrefs.HasKey("ads"))
            {
                volume.value = PlayerPrefs.GetFloat("volume");
                ads.isOn = Convert.ToBoolean(PlayerPrefs.GetFloat("ads"));
                soundSource.volume = volume.value;
                return true;
            }
            volume.value = PlayerPrefs.GetFloat("volume");
            return true;
        }
        else
        {
            volume.value = 1.0f;
            ads.isOn = false;
            return true;
        }
    }
    public void Change()
    {
        Debug.Log(volume.value + "\n" + ads.isOn);
    }
    public void Tutorial()
    {
        SceneManager.LoadScene(2);
    }
    public void ChangeMenu()
    {
        if (mainMenu.activeSelf)
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);
        }
        else if (settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}
