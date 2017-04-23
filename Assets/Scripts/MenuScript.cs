using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    private bool quit= false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (quit)
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
    }

    public void SwitchToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("main");
    }
}
