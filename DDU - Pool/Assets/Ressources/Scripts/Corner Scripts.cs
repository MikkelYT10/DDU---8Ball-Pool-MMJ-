using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerScripts : MonoBehaviour
{
    //Identify the corner
    public int cornerNumber;
    public GameObject player;
    public GameManager GameManager;

    private void Awake()
    {
        GameManager = FindObjectOfType<GameManager>();
        if (GameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
            // Handle this situation appropriately
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Corner " + cornerNumber + " entered by " + other.gameObject);
        if (other.gameObject == player)
        {
            // Centre the ball and switch the player's turn
        }


        if (other.gameObject.tag == "Striped")
        {
            GameManager.removeBallFromPlayerList(true, other.gameObject);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Solid")
        {
            GameManager.removeBallFromPlayerList(false, other.gameObject);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "8Ball")
        {

        }
    }
}
