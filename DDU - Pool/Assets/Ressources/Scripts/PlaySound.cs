using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{

    public AudioSource ClickSound; 

    public void playThisSoundEffect()
    {
        ClickSound.Play();
        Debug.Log("LYYYD!");
    }
        
}
