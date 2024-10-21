using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject player1Life1;
    [SerializeField] private GameObject player1Life2;
    [SerializeField] private GameObject player1Life3;
    [SerializeField] private GameObject player2Life1;
    [SerializeField] private GameObject player2Life2;
    [SerializeField] private GameObject player2Life3;

    // Start is called before the first frame update
    void Start()
    {
        DisplayHealth(1, 2);
    }

    public void DisplayHealth(int player1Health, int player2Health)
    {
        switch(player1Health)
        {
            case 0:
                player1Life3.SetActive(false);
                player1Life2.SetActive(false);
                player1Life1.SetActive(false);
                break;
            case 1: 
                player1Life3.SetActive(false);
                player1Life2.SetActive(false);
                player1Life1.SetActive(true);
                break;
            case 2: 
                player1Life3.SetActive(false);
                player1Life2.SetActive(true);
                player1Life1.SetActive(true);
                break;
            case 3:
                player1Life3.SetActive(true);
                player1Life2.SetActive(true);
                player1Life1.SetActive(true);
                break;
        }

        switch(player2Health)
        {
            case 0:
                player2Life3.SetActive(false);
                player2Life2.SetActive(false);
                player2Life1.SetActive(false);
                break;
            case 1:
                player2Life3.SetActive(false);
                player2Life2.SetActive(false);
                player2Life1.SetActive(true);
                break;
            case 2:
                player2Life3.SetActive(false);
                player2Life2.SetActive(true);
                player2Life1.SetActive(true);
                break;
            case 3:
                player2Life3.SetActive(true);
                player2Life2.SetActive(true);
                player2Life1.SetActive(true);
                break;
        }
    }
}
