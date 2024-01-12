using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerScripts : MonoBehaviour
{
    //Identify the corner
    public int cornerNumber;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Corner " + cornerNumber + " entered by " + other.gameObject);
    }
}
