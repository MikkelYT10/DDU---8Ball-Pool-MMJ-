using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Timers;

public class Scenechange : MonoBehaviour
{
    string farfar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChangeSceneUI(string a)
    {
        farfar = a;
        Invoke("dims", 0.1f);
    }

    void dims()
    {
        SceneManager.LoadScene(farfar);
    }



}
