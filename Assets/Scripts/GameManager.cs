using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        PreMonster, Shuffle, PostMonster, Play, Win, Reset, End
    }

    public AudioClip lever;
    public AudioClip music;
    public AudioClip scream1;
    public GameObject MonsterRemnant, Blood;

    private List<GameObject> leftovers = new List<GameObject>();

    private float timer = 2;
    private float tempTimer = 2;
    public GameObject Monster;
    public int monsterCount;
    public int monsterCountToo;
    public State currentState;
    public int monsterPlaced;
    private List<GameObject> monsters = new List<GameObject>();
    private bool hasPlayed = false;
    private List<GameObject> players = new List<GameObject>();
    public GameObject black;
    private bool Darkness = false, showButton = true;
    private int round = 1;
    private GameObject button, winButton, winButton2, mainMenuButton;
    private Text buttonText, winButtonText, winButton2Text;
    private bool lastMessageShown = false;
    private bool playScream;

    public GameObject playerTemplate;

    private AudioSource audio;


    private int monsterCountTemp;

    // Use this for initialization
    private void Start()
    {
        button = GameObject.Find("NightButton");
        buttonText = GameObject.Find("NightButtonText").GetComponent<Text>();

        winButton = GameObject.Find("WinnerButton");
        winButtonText = GameObject.Find("WinnerButtonText").GetComponent<Text>();

        winButton2 = GameObject.Find("WinnerButton2");
        winButton2Text = GameObject.Find("WinnerButtonText2").GetComponent<Text>();
        winButton2.SetActive(false);

        mainMenuButton = GameObject.Find("MainMenuButton");
        mainMenuButton.SetActive(false);


        winButton.SetActive(false);

        audio = GetComponent<AudioSource>();
        for (int i = 1; i < 5; i++)
        {
            GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player" + i);
            if (i < GameObject.Find("MenuScript").GetComponent<MenuScript>().Players + 1)
            {
                players.Add(tempPlayer);
            }
            else
            {
                tempPlayer.SetActive(false);
            }
            tempPlayer.GetComponent<PlayerMovement>().PlayerNumber = i;

        }

        monsterCount = GameObject.Find("MenuScript").GetComponent<MenuScript>().EnemiesWaveOne;
        monsterCountToo = GameObject.Find("MenuScript").GetComponent<MenuScript>().EnemiesWaveTwo + monsterCount;


        for (int i = 0; i < monsterCount; i++)
        {
            monsters.Add(Instantiate(Monster, new Vector3(0, 0, -4), Quaternion.identity));
        }


        GetComponent<Shuffle>().Bedplacements = new GameObject[3, 4];
        GetComponent<Shuffle>().Setupbeds(4, 3);
        currentState = State.PreMonster;
        black.SetActive(false);
        monsterCountTemp = monsterCount;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (currentState)
        {
            case State.PreMonster:



                if (tempTimer >= 0 && showButton)
                {
                    button.SetActive(true);

                    tempTimer -= Time.deltaTime;
                }
                else if (tempTimer < 0 && showButton)
                {

                    button.SetActive(false);
                    showButton = false;
                }

                if (monsterPlaced >= monsterCount)
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        foreach (GameObject obj in monsters)
                        {
                            Destroy(obj);
                        }
                        currentState = State.Shuffle;
                        gameObject.GetComponent<Shuffle>().IsMoving = true;
                        gameObject.GetComponent<Shuffle>().StartTimer = true;
                        timer = 2;
                    }
                }
                break;

            case State.Shuffle:

                gameObject.GetComponent<Shuffle>().ShuffleState();
                break;

            case State.PostMonster:


                if (monsterCountTemp <= monsterCountToo)
                {
                    for (int i = 0; i < monsterCountToo - monsterCountTemp; i++)
                    {
                        monsterCountTemp++;
                        monsters.Add(Instantiate(Monster, new Vector3(0, 0, -4), Quaternion.identity));
                    }

                }

                if (monsterPlaced >= monsterCountToo)
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        foreach (GameObject obj in monsters)
                        {
                            Destroy(obj);
                        }
                        currentState = State.Play;
                        timer = 2;
                    }
                }
                break;

            case State.Play:
                if (!hasPlayed)
                {
                    audio.volume = 1;
                    audio.clip = music;
                    audio.Play();
                    foreach (GameObject player in players)
                    {
                        player.GetComponent<PlayerMovement>().AllowMovement = true;
                    }


                    timer = 15;
                    hasPlayed = true;
                }

                timer -= Time.deltaTime;
                int peopleInBed = 0;
                foreach (GameObject player in players)
                {
                    if (player.GetComponent<PlayerMovement>().InBed == true)
                    {
                        peopleInBed++;
                    }
                }

                if (peopleInBed >= players.Count && !Darkness)
                {
                    black.SetActive(true);
                    GameObject.Find("PlayerLives").GetComponent<Text>().text = "";
                    Darkness = true;
                    audio.clip = lever;
                    audio.Play();
                    foreach (GameObject player in players)
                    {
                        player.GetComponent<PlayerMovement>().AllowMovement = false;
                        if (player.GetComponent<PlayerMovement>().Bed.GetComponent<Bed>().isMonsterOccupied)
                        {
                            player.GetComponent<PlayerMovement>().Health--;
                            playScream = true;

                            player.SetActive(false);
                        }
                    }
                    foreach (GameObject bed in GetComponent<Shuffle>().Bedplacements)
                    {
                        if (bed.GetComponent<Bed>().isMonsterOccupied && bed.GetComponent<Bed>().isOccupied)
                        {
                            leftovers.Add(Instantiate(Blood, bed.transform.position, Quaternion.identity));

                        }
                        else if (bed.GetComponent<Bed>().isMonsterOccupied)
                        {
                            leftovers.Add(Instantiate(MonsterRemnant, bed.transform.position, Quaternion.identity));
                        }
                    }
                    timer = 3f;
                }

                if (timer <= 0 && !Darkness)
                {
                    audio.clip = lever;
                    audio.Play();
                    black.SetActive(true);
                    GameObject.Find("PlayerLives").GetComponent<Text>().text = "";
                    Darkness = true;

                    foreach (GameObject player in players)
                    {
                        player.GetComponent<PlayerMovement>().AllowMovement = false;
                        if (player.GetComponent<PlayerMovement>().InBed != true || player.GetComponent<PlayerMovement>().Bed.GetComponent<Bed>().isMonsterOccupied)
                        {
                            player.GetComponent<PlayerMovement>().Health--;
                            playScream = true;
                            player.SetActive(false);
                        }
                    }

                    foreach (GameObject bed in GetComponent<Shuffle>().Bedplacements)
                    {
                        if (bed.GetComponent<Bed>().isMonsterOccupied && bed.GetComponent<Bed>().isOccupied)
                        {
                            leftovers.Add(Instantiate(Blood, bed.transform.position, Quaternion.identity));
                            
                        }
                        else if (bed.GetComponent<Bed>().isMonsterOccupied)
                        {
                            leftovers.Add(Instantiate(MonsterRemnant, bed.transform.position, Quaternion.identity));
                        }
                    }

                    timer = 3f;
                }
                else if (timer <= 0)
                {

                    GameObject.Find("PlayerLives").GetComponent<Text>().text = "";
                    foreach (GameObject player in players)
                    { 

                        GameObject.Find("PlayerLives").GetComponent<Text>().text += (player.GetComponent<PlayerMovement>().color) + " health: " +
(player.GetComponent<PlayerMovement>().Health) + "\n";
                    }
                    black.SetActive(false);
                    currentState = State.Win;
                    timer = 5;
                }

                if (!audio.isPlaying && audio.clip != scream1 && playScream)
                {
                    audio.clip = scream1;
                    audio.volume = 0.5f;
                    audio.Play();
                    playScream = false;
                }

                break;

            case State.Win:
                timer -= Time.deltaTime;
                List<GameObject> killThis = new List<GameObject>();
                if (timer <= 0)
                {
                    foreach (GameObject player in players)
                    {
                        if (player.GetComponent<PlayerMovement>().Health <= 0)
                        {           
                            killThis.Add(player);
                        }
                    }
                    while (killThis.Count > 0)
                    {
                        players.Remove(killThis[0]);
                        killThis.Remove(killThis[0]);
                    }

                    GameObject.Find("PlayerLives").GetComponent<Text>().text = "";
                    foreach (GameObject player in players)
                    {
                        GameObject.Find("PlayerLives").GetComponent<Text>().text += (player.GetComponent<PlayerMovement>().color) + " health: " +
(player.GetComponent<PlayerMovement>().Health) + "\n";
                    }

                    if (players.Count <= 1)
                    {

                        currentState = State.End;

                    }
                    else
                    {
                        currentState = State.Reset;
                    }
                }

                break;
            case State.Reset:
                foreach (GameObject player in players)
                {
                    player.GetComponent<PlayerMovement>().Reset();
                }
                monsterCountTemp = monsterCount;
                
                while (leftovers.Count > 0)
                {
                    Destroy(leftovers[0]);
                    leftovers.Remove(leftovers[0]);
                }

                foreach (GameObject bed in GetComponent<Shuffle>().Bedplacements)
                {
                    bed.GetComponent<Bed>().Reset();
                }
                monsters.Clear();
                for (int i = 0; i < monsterCount; i++)
                {
                    monsters.Add(Instantiate(Monster, new Vector3(0, 0, -4), Quaternion.identity));
                }



                hasPlayed = false;
                monsterPlaced = 0;
                round++;

                showButton = true;
                currentState = State.PreMonster;
                string tempString = buttonText.text.Split(' ')[0] + " " + round;
                buttonText.text = tempString;
                tempTimer = 2;
                Darkness = false;
                timer = 2;
                break;

            case State.End:
                if (players.Count == 1 && !lastMessageShown)
                {
                    

                    tempString = winButtonText.text.Split('$')[0] + " " + players[0].GetComponent<PlayerMovement>().color + " " + winButtonText.text.Split('$')[1];
                    winButtonText.text = tempString;

                    tempString = winButton2Text.text.Split('$')[0] + " " + round + " " + winButton2Text.text.Split('$')[1];
                    winButton2Text.text = tempString;

                    winButton.SetActive(true);
                    winButton2.SetActive(true);
                    lastMessageShown = true;
                    mainMenuButton.SetActive(true);
                }
                else if (players.Count == 0 && !lastMessageShown)
                {
                    winButtonText.text = "Unfortunately, no one was able to survive the endless nights";

                    tempString = winButton2Text.text.Split('$')[0] + " " + round + " " + winButton2Text.text.Split('$')[1];
                    winButton2Text.text = tempString;
                    winButton2.SetActive(true);
                    winButton.SetActive(true);
                    lastMessageShown = true;
                    mainMenuButton.SetActive(true);
                }
                
                break;

            default:
                break;
        }
    }

    public void SwitchToMainMenu()
    {
        

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");

    }
}