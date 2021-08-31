using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPlayerController : MonoBehaviour
{
    public static AIPlayerController instance;
    private NavMeshAgent agent;
    private Transform target;
    private Animator animator;

    private bool isGround;
    #region Unity

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Main Player").transform;
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // ANIMS
        if (GameManager.instance.playerState == GameManager.PlayerState.Playing)
        {
            animator.SetInteger("Status",1);
        }

        StartCoroutine(EditAgentStatus());
    }

    private void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.SetDestination(target.position);
        }
        
        if (GameManager.instance.playerState == GameManager.PlayerState.Finish)
        {
            animator.SetInteger("Status",3);
        }
        
        CheckFall();
        CheckPlayerStatus();
    }
    
    #endregion
    
    public IEnumerator EditAgentStatus()
    {
        agent.enabled = true;
        yield return new WaitForSeconds(.5f);
        agent.enabled = false;
    }

    void CheckPlayerStatus()
    {
        if (GameManager.instance.playerState == GameManager.PlayerState.Playing)
        {
            if (!CheckFall())
            {
                gameObject.AddComponent<Rigidbody>();
                Destroy(GetComponent<Rigidbody>(),2);
            }
        }
        if (transform.position.y < -.5f)
        {
            gameObject.SetActive(false);
            transform.position = transform.parent.position;
            PlayerController.instance.openTo--;
        }
    }
    bool CheckFall()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, new Vector3(-30,-1,0), out hit, 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            transform.position = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
        }
        if (other.CompareTag("Ladder"))
        {
            transform.SetParent(null);
            GameManager.instance.towerPlayerCount--;
        }
        if (other.transform.CompareTag("Obstacle"))
        {
            PlayerController.instance.openTo--;
            gameObject.SetActive(false);
        }

    }
}
