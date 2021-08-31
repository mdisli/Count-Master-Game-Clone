using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollisionDetectorForMutiplicator : MonoBehaviour
{
    private string name;
    private int num;
    
    private TextMeshProUGUI text;
    private Image img;

    [SerializeField] private Material positiveMat, negativeMat;

    #region  Unity
    private void Start()
    {
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        img = transform.GetChild(1).GetComponent<Image>();
        
        EditTexts();
        EditColor();
    }
    
    #endregion

    #region Multiplication
    void Multiplicate()
    {
        name = transform.name;
        string name2 = "";

        if (name[0].ToString() == "+" || name[0].ToString() == "-")
        {
            int.TryParse(name, out num);
            //PlayerController.instance.OpenPlayers(PlayerController.instance.openTo +num);
            PlayerController.instance.openTo += num;
        }

        if (name[0].ToString() == "X")
        {
            for (int i = 1; i < name.Length; i++)
            {
                name2 += name[i].ToString();
            }

            int.TryParse(name2, out num);
            PlayerController.instance.openTo *= num;
        }
        
        PlayerController.instance.MultiplicatePlayer();
    }
    #endregion


    #region Canvas

    void EditTexts()
    {
        text.text = transform.name;
    }

    void EditColor()
    {
        if (transform.name[0].ToString() == "-")
        {
            img.material = negativeMat;
        }
        else
        {
            img.material = positiveMat;
        }
    }
    

    #endregion
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Main Player"))
        {
            Multiplicate();
        }
    }
}
