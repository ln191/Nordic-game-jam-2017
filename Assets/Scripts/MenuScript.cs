using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuScript : MonoBehaviour
{

    private bool quit = false;
    private int players = 2;
    private int enemiesWaveOne = 1, enemiesWaveTwo = 1;

    public int Players
    {
        get
        {
            return players;
        }

        set
        {
            players = value;
        }
    }

    public int EnemiesWaveOne
    {
        get
        {
            return enemiesWaveOne;
        }

        set
        {
            enemiesWaveOne = value;
        }
    }

    public int EnemiesWaveTwo
    {
        get
        {
            return enemiesWaveTwo;
        }

        set
        {
            enemiesWaveTwo = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (quit || Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Exit()
    {
        quit = true;
    }
    public void SwitchToHelp()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HelpScene");
        GameObject.Find("MainMenuCanvas").active = false;
    }

    public void SwitchToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        if (GameObject.Find("OptionCanvas") != null)
        {
            GameObject.Find("OptionCanvas").active = false;
        }
        else if (GameObject.Find("HelpCanvas") != null)
        {
            GameObject.Find("HelpCanvas").active = false;
        }
    }
    public void SetOptions()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Options");
        GameObject.Find("MainMenuCanvas").active = false;
    }
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("main");
        GameObject.Find("OptionCanvas").active = false;
    }

    public void ChangeNumPlayers()
    {
        players = Convert.ToInt32(GameObject.Find("PlayerSlider").GetComponent<Slider>().value);
        GameObject.Find("NumPlayers").GetComponent<Text>().text = "Num of players: " + players;
    }

    public void ChangeEnemiesWaveOne()
    {
        enemiesWaveOne = Convert.ToInt32(GameObject.Find("EnemyWave1Slider").GetComponent<Slider>().value);
        GameObject.Find("EnemyWave1Slider").GetComponent<Slider>().maxValue = 11 - Convert.ToInt32(GameObject.Find("EnemyWave2Slider").GetComponent<Slider>().value);

        GameObject.Find("NumEnemiesOne").GetComponent<Text>().text = "Enemies in wave one: " + enemiesWaveOne;
    }

    public void ChangeEnemiesWaveTwo()
    {
        enemiesWaveTwo = Convert.ToInt32(GameObject.Find("EnemyWave2Slider").GetComponent<Slider>().value);
        GameObject.Find("EnemyWave2Slider").GetComponent<Slider>().maxValue = 11 - Convert.ToInt32(GameObject.Find("EnemyWave1Slider").GetComponent<Slider>().value);

        GameObject.Find("NumEnemiesTwo").GetComponent<Text>().text = "Enemies in wave two: " + enemiesWaveTwo;
    }
}
