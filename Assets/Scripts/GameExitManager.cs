using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExitManager : MonoBehaviour
{
    public GameObject exitModal;


    // Update is called once per frame
    void Update()
    {
        //#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitModal.SetActive(true);
            //Application.Quit();
        }
    }
}
