using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Toast : MonoBehaviour
{
    public static Toast instance;
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

    // Start is called before the first frame update
    public void CloseToast(float delaytime)
    {
        animator.SetTrigger("close");
        //StartCoroutine(CloseThis());
        Invoke("CloseThis", delaytime);
        animator.ResetTrigger("close");
    }

    private void CloseThis()
    {
        //StartCoroutine(CloseAfterDelay());
        gameObject.SetActive(false);
    }

    private IEnumerator CloseAfterDelay()
     {
       animator.SetTrigger("close");
       yield return new WaitForSeconds(0.5f);
       gameObject.SetActive(false);
       animator.ResetTrigger("close");
     }

}
