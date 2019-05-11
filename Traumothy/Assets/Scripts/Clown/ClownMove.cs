﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClownMove : MonoBehaviour
{

    public int level = 1;

    public float speed = 1.5f;

    public GameObject cup1;
    public GameObject cup2;
    public GameObject cup3;

    public GameObject pos1, pos2, pos3;
    Vector2 temp;

    public int winner;
    public Vector3 final;
    public int flip;
    public float count = 0;
    public float time = 20f;
    public float timer = 0f;

    int answer;

    bool isMoving1 = false, isMoving2 = false, start = true, guess = false, pressed = false, isDone = false;
    bool animStarting = false;

    // Use this for initialization
    void Start()
    {
        cup1.transform.position = new Vector2(-3, 0);
        cup2.transform.position = new Vector2(0, 0);
        cup3.transform.position = new Vector2(3, 0);
        pos1.transform.position = new Vector2(-3, 0);
        pos2.transform.position = new Vector2(0, 0);
        pos3.transform.position = new Vector2(3, 0);

        level = 1;

        count = 0;

        speed = 1.5f;
        cup2.GetComponentInChildren<Animator>().SetTrigger("Start");
        animStarting = true;
        StartTimer();


    }

    void Shuffle()
    {
        if (start)
        {
            flip = Random.Range(1, 4);

            switch (flip)
            {
                case 1:
                    StartCoroutine(move1(cup1.transform, pos2.transform.position, speed));
                    StartCoroutine(move2(cup2.transform, pos1.transform.position, speed));
                    temp = pos1.transform.position;
                    pos1.transform.position = pos2.transform.position;
                    pos2.transform.position = temp;
                    break;
                case 2:
                    StartCoroutine(move1(cup2.transform, pos3.transform.position, speed));
                    StartCoroutine(move2(cup3.transform, pos2.transform.position, speed));
                    temp = pos2.transform.position;
                    pos2.transform.position = pos3.transform.position;
                    pos3.transform.position = temp;
                    break;
                case 3:
                    StartCoroutine(move1(cup1.transform, pos3.transform.position, speed));
                    StartCoroutine(move2(cup3.transform, pos1.transform.position, speed));
                    temp = pos1.transform.position;
                    pos1.transform.position = pos3.transform.position;
                    pos3.transform.position = temp;
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
    }

    IEnumerator move1(Transform fromPosition, Vector2 toPosition, float duration)
    {
        start = false;
        if (isMoving1)
        {
            yield break; ///exit if this is still running
        }
        isMoving1 = true;
        float counter = 0;
        //Get the current position of the object to be moved
        Vector2 startPos = fromPosition.position;
        count++;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            fromPosition.position = Vector2.Lerp(startPos, toPosition, counter / duration);
            yield return null;
        }
        isMoving1 = false;
    }

    IEnumerator move2(Transform fromPosition, Vector2 toPosition, float duration)
    {
        if (isMoving2)
        {
            yield break; ///exit if this is still running
        }
        isMoving2 = true;
        float counter = 0;
        //Get the current position of the object to be moved
        Vector2 startPos = fromPosition.position;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            fromPosition.position = Vector2.Lerp(startPos, toPosition, counter / duration);
            yield return null;
        }
        isMoving2 = false;
        start = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (animStarting)
            StartTimer();

        if (pressed)
            StartTimer();

        CheckTimer();
        CheckDone();

        if (count < 6 && animStarting == false)
        {
            Shuffle();
            Wait();
        }
        else
        {
            guess = true;
        }

        if (guess && pressed == false)
        {
            if (Input.GetKeyDown("1"))
            {
                CheckAnswer(-3f);

            }
            if (Input.GetKeyDown("2"))
            {
                CheckAnswer(0);
            }
            if (Input.GetKeyDown("3"))
            {
                CheckAnswer(3f);

            }
            guess = false;
        }
    }

    void CheckDone()
    {
        if (isDone)
        {
            isDone = false;
            ResetTimer();
            pressed = false;
            cup2.GetComponentInChildren<Animator>().SetBool("isRight", false); //Resets animation
            cup2.GetComponentInChildren<Animator>().SetBool("isWrong", false);

            switch (level)
            {
                case 3:
                    SceneManager.LoadScene("scene");
                    break;
                default:
                    speed /= 2;
                    count = 0;
                    level++;
                    break;
            }
        }
    }

    void CheckTimer()
    {
        if (animStarting == true)
        {
            if (timer >= 2.5f)
            {
                animStarting = false;
                ResetTimer();
            }
        }
        else
        {
            if (timer >= 2f)
                isDone = true;
        }
    }

    void CheckAnswer(float answer)
    {
        float win = cup2.transform.position.x;

        if (answer == win)
        { 
            pressed = true;
            guess = false;
            cup2.GetComponentInChildren<Animator>().SetBool("isRight", true);
            cup2.GetComponentInChildren<Animator>().SetBool("isWrong", false);

            Debug.Log("WIN");
            //isDone
        }
        else
        {
            pressed = true;
            cup2.GetComponentInChildren<Animator>().SetBool("isRight", false);
            cup2.GetComponentInChildren<Animator>().SetBool("isWrong", true);

            Debug.Log("LOSE");
        }
    }

    void StartTimer()
    {
        timer += Time.deltaTime;
        Debug.Log(timer);
    }

    void ResetTimer()
    {
        timer = 0;
    }
}