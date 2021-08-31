using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject preparingScreen, playingScreen, finishScreen, failScreen;

    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI prepareLevelTXT, playingLevelTXT;
    [SerializeField] private Transform mapHolder;

    private Transform player;

  

    #region Unity

    private void Start()
    {
        player = GameObject.FindWithTag("Main Player").transform;
        EditTextOnStart();
    }

    private void Update()
    {
        CheckGameStatus();
        EditSlider();
    }
    #endregion


    #region EditUI

    void CheckGameStatus()
    {
        // PREPARING SCREEN
        if (GameManager.instance.playerState == GameManager.PlayerState.Preparing)
        {
            preparingScreen.SetActive(true);
        }
        else
        {
            preparingScreen.SetActive(false);
        }
        
        // PLAYING SCREEN
        if (GameManager.instance.playerState == GameManager.PlayerState.Playing)
        {
            playingScreen.SetActive(true);
        }
        else
        {
            playingScreen.SetActive(false);
        }
        
        // FINISH SCREEN
        if (GameManager.instance.playerState == GameManager.PlayerState.Finish)
        {
            finishScreen.SetActive(true);
        }
        else
        {
            finishScreen.SetActive(false);
        }
        
        // FAIL SCREEN
        if (GameManager.instance.playerState == GameManager.PlayerState.Fail)
        {
            failScreen.SetActive(true);
        }
        else
        {
            failScreen.SetActive(false);
        }
    }

    void EditTextOnStart()
    {
        prepareLevelTXT.text = PlayerPrefs.GetInt("Level").ToString();
        playingLevelTXT.text = PlayerPrefs.GetInt("Level").ToString();
    }
    void EditSlider()
    {
        int length = mapHolder.childCount * 10;
        float value = player.position.z / length;
        slider.value = value;
    }

    #endregion
   
}
