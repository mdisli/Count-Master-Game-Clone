using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerState playerState;
    private Transform towerHolder;
    private int level;
    public int towerPlayerCount;

    public enum PlayerState
    {
        Preparing,
        Playing,
        Tower,
        Finish,
        Fail
    }

    #region Unity

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        towerHolder = GameObject.FindWithTag("Tower Holder").transform;
        // PLAYER PREFS
        level = PlayerPrefs.GetInt("Level", 1);
    }

    private void Start()
    {
        playerState = PlayerState.Preparing;
    }

    private void Update()
    {
        // STARTING GAME
        if (playerState == PlayerState.Preparing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerState = PlayerState.Playing;
                // START RUN
                PlayerController.instance.canMove = true;
            }
        }
        //
        CheckGameFailed();
        //
        CheckGameFinish();
    }

    #endregion

    void CheckGameFailed()
    {
        if (PlayerController.instance.openTo <= 0)
        {
            playerState = PlayerState.Fail;
        }
    }

    void CheckGameFinish()
    {
        if (playerState == PlayerState.Tower)
        {
            if (towerPlayerCount == 0)
            {
                // GAME FINISH SUCCESSFULLY
                playerState = PlayerState.Finish;
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        level++;
        PlayerPrefs.SetInt("Level",level);

        if (level > SceneManager.sceneCountInBuildSettings)
        {
            int randomNum = Random.Range(0, SceneManager.sceneCountInBuildSettings);
            SceneManager.LoadScene(randomNum);
        }
        else
        {
            SceneManager.LoadScene(level - 1);
        }
    }
}
