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


    private float timer = 2;
    public GameObject Monster;
    public int monsterCount = 4;
    public int monsterCountToo = 7;
    public State currentState;
    public int monsterPlaced;
    private List<GameObject> monsters = new List<GameObject>();
    private bool hasPlayed = false;
    private List<GameObject> players = new List<GameObject>();
    public GameObject black;
    private bool Darkness = false;
    private AudioSource audio;

    private int monsterCountTemp;

    // Use this for initialization
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        for (int i = 0; i < monsterCount; i++)
        {
            monsters.Add(Instantiate(Monster, new Vector3(0, 0, -4), Quaternion.identity));
        }
        for (int i = 0; i < 4; i++)
        {
            players.Add(GameObject.FindGameObjectWithTag("Player" + (i + 1)));
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
                        player.GetComponent<PlayerMovement>().allowMovement = true;
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
                        player.GetComponent<PlayerMovement>().allowMovement = false;
                        if (player.GetComponent<PlayerMovement>().Bed.GetComponent<Bed>().isMonsterOccupied)
                        {
                            player.GetComponent<PlayerMovement>().Health--;
                            
                            player.SetActive(false);
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
                        player.GetComponent<PlayerMovement>().allowMovement = false;
                        if (player.GetComponent<PlayerMovement>().InBed != true || player.GetComponent<PlayerMovement>().Bed.GetComponent<Bed>().isMonsterOccupied)
                        {
                            player.GetComponent<PlayerMovement>().Health--;
                            player.SetActive(false);
                        }
                    }

                    timer = 3f;
                }
                else if (timer <= 0)
                {

                    GameObject.Find("PlayerLives").GetComponent<Text>().text = "";
                    foreach (GameObject player in players)
                    {
                        GameObject.Find("PlayerLives").GetComponent<Text>().text += "Player " + (player.GetComponent<PlayerMovement>().playerNumber) + " health: " +
(player.GetComponent<PlayerMovement>().Health) + "\n";
                    }
                    black.SetActive(false);
                    currentState = State.Win;
                    timer = 5;
                }

                if (!audio.isPlaying && audio.clip != scream1)
                {
                    audio.clip = scream1;
                    audio.volume = 0.5f;
                    audio.Play();
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
                        GameObject.Find("PlayerLives").GetComponent<Text>().text += "Player " + (player.GetComponent<PlayerMovement>().playerNumber) + " health: " +
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
                currentState = State.PreMonster;
                Darkness = false;
                timer = 2;
                break;

            case State.End:
                
                break;

            default:
                break;
        }
    }
}