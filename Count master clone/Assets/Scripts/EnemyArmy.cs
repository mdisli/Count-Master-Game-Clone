using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyArmy : MonoBehaviour
{
    [Header("Creating Army")]
    [SerializeField] public Transform soldierHolder;
    [SerializeField] public int soldierCount;

    [Header("Canvas Process")] [SerializeField]
    public TextMeshProUGUI soldierCountTXT;
   
    #region Unity
    private void Start()
    {
        soldierCountTXT.text = soldierCount.ToString();
        StartCoroutine(CreateArmy());
    }
    #endregion

    #region Creating Army

    IEnumerator CreateArmy()
    {
        for (int i = 0; i < soldierCount; i++)
        {
            GameObject obj = soldierHolder.GetChild(i).gameObject;
            obj.SetActive(true);
        }
        transform.position += Vector3.right*3;
        yield return new WaitForSeconds(.5f);
        transform.position -= Vector3.right*3;
    }
    #endregion

    #region  Collisions

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Main Player"))
        {
            for (int i = 0; i < soldierCount; i++)
            {
                GameObject obj = soldierHolder.GetChild(i).gameObject;
                obj.GetComponent<NavMeshAgent>().SetDestination(other.transform.position);
                obj.GetComponent<Animator>().SetTrigger("Fight");

            }
        }
    }

    #endregion
}
