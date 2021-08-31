using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offSet;
    [SerializeField] private bool lookAt;
    [SerializeField] private Transform towerHolder;
    [SerializeField] private Vector3 towerOffSet = new Vector3(10, 5, 15);

    private void Start()
    {
        target = GameObject.FindWithTag("Main Player").transform;
        towerHolder = GameObject.FindWithTag("Tower Holder").transform;
    }

    void FollowOnPlaying()
    {
        if (GameManager.instance.playerState == GameManager.PlayerState.Preparing || GameManager.instance.playerState == GameManager.PlayerState.Playing)
        {
            if (lookAt)
            {
                transform.LookAt(target.position);
            }

            transform.DOMove(target.position - offSet, .2f);
        }
    }

    void FollowOnMakingTower()
    {
        transform.DOMove(towerHolder.position + towerOffSet, 1);
        transform.DOLookAt(towerHolder.position - new Vector3(0,5,0),1);
        
    }

    private void Update()
    {
        if (GameManager.instance.playerState != GameManager.PlayerState.Tower)
        {
            FollowOnPlaying();
        }
        else
        {
            FollowOnMakingTower();
        }
    }
}
