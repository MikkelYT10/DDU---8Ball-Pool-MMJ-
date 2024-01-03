using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public float Force;
    private GameObject ballInstance;

    private void Start()
    {
        // Instantiate the ball prefab
        ballInstance = Instantiate(ballPrefab, new Vector3(0, 1, 0), Quaternion.identity);

        // Make the ball black
        ballInstance.GetComponent<Renderer>().material.color = Color.black;

        // Spawn another ball
        Instantiate(ballPrefab, new Vector3(1, 1, 2), Quaternion.identity);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Push the ball forward
            PushBall();
        }
    }

    //Push the ball forward
    private void PushBall()
    {
        // Get the ball rigidbody
        Rigidbody ballRigidbody = ballInstance.GetComponent<Rigidbody>();

        // Add a force to the ball
        ballRigidbody.AddForce(Vector3.forward * Force, ForceMode.Impulse);
    }



}