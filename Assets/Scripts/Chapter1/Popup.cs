﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Popup : MonoBehaviour
{ 
    public static Popup instance;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (instance != null)
        {
        }
        else
        {
            instance = this;
        }
        
    }
    public void Start()
    {
        //audio = GetComponent<AudioSource>();   
        GameLoad(); 
    }

    //로드/세이브
    public void GameSave(){
        PlayerPrefs.SetInt("LoadId",DialogueManager.instance.thisId);
        PlayerPrefs.Save(); 
    }

    public void GameLoad(){
        if (!PlayerPrefs.HasKey("LoadId")){
            return;
        }

        int thisId = PlayerPrefs.GetInt("LoadId");
        
        DialogueManager.instance.thisId = thisId;
    }
   
   //데이터 초기화 = 새로시작
    public void newGame(){
        PlayerPrefs.DeleteKey("LoadId");
    }

   public void CloseNoDel(){
        StartCoroutine(CloseNoDelay());
   }
   public void Close()
   {
       StartCoroutine(CloseAfterDelay());
   }

   private IEnumerator CloseAfterDelay()
   {
       animator.SetTrigger("close");
       yield return new WaitForSeconds(0.2f);
       gameObject.SetActive(false);
       animator.ResetTrigger("close");
   }

   private IEnumerator CloseNoDelay()
   {
       animator.SetTrigger("close");
       yield return new WaitForSeconds(0.0f);
       gameObject.SetActive(false);
       animator.ResetTrigger("close");
   }

   public void ExitYes()  
    {
        Application.Quit();   // 앱을 종료
    }
}
