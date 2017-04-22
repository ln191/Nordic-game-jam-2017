using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        PreMonster, Shuffle, PostMonster, Play, Win
    }

    private float timer = 2;
    public GameObject Monster;
    public int monsterCount = 4;
    public int monsterCountToo = 7;
    public State currentState;
    public int monsterPlaced;
    private List<GameObject> monsters = new List<GameObject>();
    private bool hasPlayed = false;
    private List<GameObject> players = new List<GameObject>();

    // Use this for initialization
    private void Start()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            monsters.Add(Instantiate(Monster, new Vector3(0, 0, -4), Quaternion.identity));
        }

        gameObject.GetComponent<Shuffle>().Bedplacements = new GameObject[3, 4];
        gameObject.GetComponent<Shuffle>().Setupbeds(4, 3);
        currentState = State.PreMonster;

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
                            obj.active = false;
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

                if (monsterCount <= monsterCountToo)
                {
                    for (int i = 0; i < monsterCountToo - monsterCount; i++)
                    {
                        monsterCount++;
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
                            obj.SetActive(false);
                        }
                        currentState = State.Play;
                        timer = 2;
                    }
                }
                break;

            case State.Play:
                if (!hasPlayed)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        players.Add(GameObject.FindGameObjectWithTag("Player" + (i+1)));
                        players[i].GetComponent<PlayerMovement>().allowMovement = true;
                    }
                    timer = 15;
                    hasPlayed = true;
                }

                timer -= Time.deltaTime;

                foreach (GameObject player in players)
                {

                }
                int peopleInBed = 0;
                foreach (GameObject player in players)
                {
                    if (player.GetComponent<PlayerMovement>().InBed == true)
                    {
                        peopleInBed++;
                    }
                }
                if (peopleInBed >= players.Count)
                {
                    timer = 0;
                }

                if (timer <= 0)
                {
                    foreach (GameObject player in players)
                    {



                        if (player.GetComponent<PlayerMovement>().InBed != true)
                        {
                            player.GetComponent<PlayerMovement>().Health--;
                            player.SetActive(false);
                        }
                        else if (player.GetComponent<PlayerMovement>().Bed.GetComponent<Bed>().isMonsterOccupied)
                        {
                            player.GetComponent<PlayerMovement>().Health--;
                            player.SetActive(false);
                        }
                        else
                        {
                            player.GetComponent<PlayerMovement>().allowMovement = true;
                        }
                    }


                }
                break;

            case State.Win:
                break;

            default:
                break;
        }
    }
}