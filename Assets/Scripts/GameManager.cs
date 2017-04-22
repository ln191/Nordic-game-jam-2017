using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        PreMonster, Shuffle, PostMonster, Play, Win
    }

    public State currentState;

    // Use this for initialization
    private void Start()
    {
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
                currentState = State.Shuffle;
                gameObject.GetComponent<Shuffle>().IsMoving = true;
                gameObject.GetComponent<Shuffle>().StartTimer = true;
                break;

            case State.Shuffle:

                gameObject.GetComponent<Shuffle>().ShuffleState();
                break;

            case State.PostMonster:
                break;

            case State.Play:
                break;

            case State.Win:
                break;

            default:
                break;
        }
    }
}