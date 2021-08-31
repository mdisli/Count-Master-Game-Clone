using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("-- MOVEMENT --")] 
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private float forwardSpeed;
    [SerializeField] float sideSpeed;
    public bool canMove = false;

    [Header("-- Multiplication --")] 
    [SerializeField] private TextMeshProUGUI openPlayerCountText;
    [SerializeField] private int maxPlayerCount;
    public Transform aiPlayerHolder;
    public int openTo = 3;

    [Header("-- Make Tower --")] 
    [SerializeField] private Transform towerHolder;
    [SerializeField] private float towerSpeed;
    [SerializeField] private bool moveTower = false;

    [Header("-- Fight --")] 
    [SerializeField] private Transform soldierHolder;
    [SerializeField] private int enemyCount;
    [SerializeField] private TextMeshProUGUI enemyCountTXT;

    public static PlayerController instance;

    #endregion

    #region Unity

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        aiPlayerHolder = transform.GetChild(0);
    }

    private void Start()
    {
        MultiplicatePlayer();
    }

    private void Update()
    {
        EditTexts();

        if (GameManager.instance.playerState == GameManager.PlayerState.Playing)
        {
            if (canMove)
            {
                Move();
            }
        }
        
        MoveTower(moveTower);
    }

    #endregion

    #region MOVEMENT

    void Move(bool canSwipe = true)
    {
        // MOVE FORWARD ALLWAYS
        transform.position +=forwardSpeed * Time.deltaTime *Vector3.forward;

        if (canSwipe)
        {
            if (joystick.Horizontal > 0)
            {
                transform.position += sideSpeed * Time.deltaTime * Vector3.right;
            }
            else if (joystick.Horizontal < 0)
            {
                transform.position += sideSpeed * Time.deltaTime * Vector3.left;
            }
        }
        
        // HOLD IN MAP

        Vector3 newPos;
        if (transform.position.x > 7.2f)
        {
            newPos = new Vector3(7.2f, transform.position.y, transform.position.z);
            transform.position = newPos;
        }
        else if (transform.position.x < -7.2f)
        {
            newPos = new Vector3(-7.2f, transform.position.y, transform.position.z);
            transform.position = newPos;
        }
        
    }

    #endregion

    #region Multiplication

    void EditTexts()
    {
        openPlayerCountText.text = openTo.ToString();
    }
    public void MultiplicatePlayer()
    {
        int playerCount;
        if (openTo > maxPlayerCount)
        {
            playerCount = maxPlayerCount;
        }
        else
        {
            playerCount = openTo;
        }
        
        // OPEN PLAYERS

        for (int i = 0; i < playerCount; i++)
        {
            aiPlayerHolder.GetChild(i).gameObject.SetActive(true);
        }
        // CLOSE OTHER PLAYERS
        for (int i = playerCount; i < aiPlayerHolder.childCount; i++)
        {
            aiPlayerHolder.GetChild(i).gameObject.SetActive(false);
        }
        
        // OPEN NAVMESH
        StartCoroutine(AIPlayerController.instance.EditAgentStatus());
    }
    

    #endregion

    #region Tower Making

    public void OpenPlayers(int count)
    {
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject obj = aiPlayerHolder.GetChild(i).gameObject;
                obj.SetActive(true);
                obj.GetComponent<Animator>().SetTrigger("Run");
                obj.GetComponent<NavMeshAgent>().enabled = false;
            }

            for (int i = count; i < aiPlayerHolder.childCount; i++)
            {
                GameObject obj = aiPlayerHolder.GetChild(i).gameObject;
                obj.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < aiPlayerHolder.childCount; i++)
            {
                GameObject obj = aiPlayerHolder.GetChild(i).gameObject;
                obj.SetActive(false);
            }
        }
    }
    public void CalculatePlayerCount()
    {
        
        //  We find the required number of people to build the tower.
        int playerCount;
        int columnCount = 0;
        if (openTo > maxPlayerCount)
        {
            playerCount = maxPlayerCount;
        }
        else
        {
            playerCount = openTo;
        }

        for (int i = 0; i < playerCount; i++)
        {
            if (((i *(i+1)) / 2 > playerCount))
            {
                playerCount = (i * (i + 1)) / 2;
                columnCount = i;
                break;
            }
        }
        
        // Open players
        OpenPlayers(playerCount);
        
        // Make Tower

        StartCoroutine(MakeTower(columnCount));

    }

    void MoveTower(bool move)
    {
        if (move)
        {
            if (towerHolder.childCount > 0)
            {
                towerHolder.position += towerSpeed * Time.deltaTime * Vector3.forward;
            }
        }
    }

    IEnumerator MakeTower(int columnCount)
    {
        int columnNum = 1;
        towerHolder.position = new Vector3(0, 0, transform.position.z);
        Vector3 pos1 = towerHolder.position;
        Vector3 pos2 = towerHolder.position;
        
        for (int i = 0; i < columnCount; i++)
        {
            for (int j = 0; j < columnNum; j++)
            {
                GameObject obj = aiPlayerHolder.GetChild(0).gameObject;
                obj.transform.position = pos2;
                obj.transform.SetParent(towerHolder);
                pos2 += new Vector3(.56f, 0, 0);
                yield return new WaitForSeconds(.02f);
            }
            columnNum++;
            towerHolder.position += new Vector3(0, 1.2f, 0);

            pos1 -= new Vector3(.28f, 0, 0);
            pos2 = pos1;
        }
        towerHolder.position -= new Vector3(0, 1, 0);
        moveTower = true;
    }

    #endregion

    #region Fight

    void EditPlayerAfterFight()
    {
        for (int i = 0; i < openTo; i++)
        {
            aiPlayerHolder.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = openTo; i < aiPlayerHolder.childCount; i++)
        {
            aiPlayerHolder.GetChild(i).gameObject.SetActive(false);
        }
        
    }

    void SetAnim(string trigger)
    {
        for (int i = 0; i < aiPlayerHolder.childCount; i++)
        {
            aiPlayerHolder.GetChild(i).GetComponent<Animator>().SetTrigger(trigger);
        }
    }
    IEnumerator Fight(Collider other)
    {
        // ANIM
        SetAnim("Fight");
        
        canMove = false;
        soldierHolder = other.GetComponent<EnemyArmy>().soldierHolder;
        enemyCount = other.GetComponent<EnemyArmy>().soldierCount;
        enemyCountTXT = other.GetComponent<EnemyArmy>().soldierCountTXT;
        int x = enemyCount;
        
        for (int i = 0; i < x; i++)
        {
            if (GameManager.instance.playerState == GameManager.PlayerState.Playing)
            {
                openTo--;
                enemyCount--;
                soldierHolder.GetChild(i).gameObject.SetActive(false);
                aiPlayerHolder.GetChild(i).gameObject.SetActive(false);
                enemyCountTXT.text = enemyCount.ToString();
                yield return new WaitForSeconds(.1f);
            }
        }
        EditPlayerAfterFight();
        SetAnim("Run");
        canMove = true;

    }

    #endregion

    #region Collisions

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Tower"))
        {
            GameManager.instance.playerState = GameManager.PlayerState.Tower;
            canMove = false;
            CalculatePlayerCount();
        }

        if (other.transform.CompareTag("Enemy Army"))
        {
            // FIGHT
            StartCoroutine(Fight(other));
        }
    }

    #endregion
}
