﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Ch1_Quest3Manager : MonoBehaviour
{
    //Dialog Objects
    public GameObject QuestDialogBox;
    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueText;
    public Image Portrait;
    public Image SuccessPortrait;
    public Sprite[] portraitImages = new Sprite[2];
    public Sprite successImage;
    
    //Q1-1
    public TMP_InputField InputF_1;
    public GameObject Input_1;
    //Q1-2
    public TMP_InputField InputF_2;
    public GameObject Input_2;
    public Button GetAnswerBtn1;
    public Button GetAnswerBtn2;

    private int dialogtotalcnt;
    public Queue<QuestBase.Info> QuestInfo;

    public static Ch1_Quest3Manager instance;
    public void Awake()
    {
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
        QuestInfo = new Queue<QuestBase.Info>();  //초기화
    }

    public void EnqueueQuest(QuestBase db)
    {
        Portrait.gameObject.SetActive(true);
        SuccessPortrait.sprite = successImage;
        Portrait.sprite = portraitImages[0];
        SuccessPortrait.gameObject.SetActive(false);
        RectTransform rt = (RectTransform)Portrait.transform;
        rt.sizeDelta = new Vector2(0, 944);
        QuestDialogBox.SetActive(true);
        Destroy(GameObject.Find("othertexts"));
        QuestInfo.Clear();

        foreach (QuestBase.Info info in db.QuestInfo)
        {
            QuestInfo.Enqueue(info);
        }
        dialogtotalcnt = QuestInfo.Count;

        InputF_1.characterLimit = 2;
        InputF_2.characterLimit = 50;

        DequeueQuest();
    }

    private bool flag = true; //기본값은 true
    private QuestBase.Info Qinfo_1, Qinfo_2;

    public void DequeueQuest()
    {
        if (QuestInfo.Count.Equals(dialogtotalcnt - 5))
        {
            if (!flag) //문제 틀린 직후
            {
                Portrait.gameObject.SetActive(true);
                Input_1.SetActive(true);
                GetAnswerBtn1.gameObject.SetActive(true);
                GetAnswerBtn1.onClick.AddListener(GetAnswer1);
                dialogueName.text = Qinfo_1.myName;
                dialogueText.text = Qinfo_1.myText;
                flag = true;
            }
            else //문제 답 입력
            {
                if ((InputF_1.text.ToString()).Equals("4"))
                {
                    QuestBase.Info info = QuestInfo.Dequeue();
                    dialogueName.text = info.myName;
                    dialogueText.text = info.myText;
                    Portrait.sprite = portraitImages[1];
                    Input_1.SetActive(false);
                    GetAnswerBtn1.gameObject.SetActive(false);
                }
                else if ((InputF_1.text.ToString()).Trim().Equals("") || (InputF_1.text.ToString()).Equals(null))
                {
                    return; //미입력시 아무 반응 안함
                }
                else //오답 입력시
                {
                    GetAnswerBtn1.gameObject.SetActive(false);
                    Portrait.gameObject.SetActive(true);
                    Input_1.SetActive(false);
                    dialogueName.text = "에러 발생";
                    dialogueText.text = "음··· 이곳이 아닌 것 같아.";
                    flag = false;
                }
            }
            InputF_1.text = null;
        }
        else if (QuestInfo.Count.Equals(dialogtotalcnt - 8))
        {
            if (!flag) //문제 틀린 직후
            {
                Portrait.gameObject.SetActive(true);
                Input_2.SetActive(true);
                GetAnswerBtn2.gameObject.SetActive(true);
                GetAnswerBtn2.onClick.AddListener(GetAnswer2);
                dialogueName.text = Qinfo_2.myName;
                dialogueText.text = null;
                flag = true;
            }
            else //문제 답 입력
            {
                if ((InputF_2.text.ToString()).Trim().Equals("") || (InputF_2.text.ToString()).Equals(null)) //
                {
                    return; //미입력시 아무 반응 안함
                }
                else
                {
                    QuestManager.instance.startLoading(isCorrect(InputF_2.text.ToString()));

                    if (isCorrect(InputF_2.text.ToString()))
                    {
                        //정답의경우
                        QuestBase.Info info = QuestInfo.Dequeue();
                        dialogueName.text = info.myName;
                        dialogueText.text = info.myText;
                        InputF_2.text = null;
                        SuccessPortrait.gameObject.SetActive(true);
                        Portrait.gameObject.SetActive(false);
                        Input_2.SetActive(false);
                        GetAnswerBtn2.gameObject.SetActive(false);
                    }
                    else //오답 입력시
                    {
                        GetAnswerBtn2.gameObject.SetActive(false);
                        Portrait.gameObject.SetActive(true);
                        Input_2.SetActive(false);
                        dialogueName.text = null;
                        dialogueText.text = "문제가 해결되지 않았어\n다시 생각해보자!";
                        flag = false;
                    }
                }
            }
        }
        else if (QuestInfo.Count.Equals(0)) //Quest 다이얼로그 끝나면
        {
            SuccessPortrait.gameObject.SetActive(false);
            Portrait.gameObject.SetActive(false);
            QuestManager.instance.spinStar();
            Invoke("EndofQuest", 4.5f);
            return;
        }
        else
        {
            QuestBase.Info info = QuestInfo.Dequeue();
            if (QuestInfo.Count.Equals(dialogtotalcnt - 5)) //input 1 최초 로드
            {
                Input_1.SetActive(true);
                GetAnswerBtn1.gameObject.SetActive(true);
                GetAnswerBtn1.onClick.AddListener(GetAnswer1);
                InputF_1.text = "";
                Qinfo_1 = info;
            }
            else if (QuestInfo.Count.Equals(dialogtotalcnt - 8)) //input 2 최초 로드 
            {
                Input_2.SetActive(true);
                GetAnswerBtn2.gameObject.SetActive(true);
                GetAnswerBtn2.onClick.AddListener(GetAnswer2);
                InputF_2.text = "";
                Qinfo_2 = info;
            }
            dialogueName.text = info.myName;
            dialogueText.text = info.myText;
        }
    }

    private string Qinfo_2_CorrectA = "if ( set_ink.equals( \"setink\" ) ) {";  //if ( set_ink.equals( "setink" ) ) {

    private bool isCorrect(string answer)
    {
        answer = answer.Trim();

        string[] raw_list = Qinfo_2_CorrectA.Split('\x020');
        
        //필수 단어들이 들어가 있는지
        if (answer.IndexOf(raw_list[0]).Equals(-1) || answer.IndexOf(raw_list[2]).Equals(-1) || answer.IndexOf(raw_list[3]).Equals(-1))
        {
            return false;
        }

        //전체 문자열이 다르면 오답
        if (!answer.Replace(" ", "").Equals(Qinfo_2_CorrectA.Replace(" ", "")))
        {
            return false;
        }

        //문자들의 위치 순서가 맞는지
        int pos = -1, nowpos;
        for (int i = 0; i < raw_list.Length; i++)
        {
            if(i.Equals(5)) nowpos = answer.LastIndexOf(raw_list[i]);
            else nowpos = answer.IndexOf(raw_list[i]);

            if (nowpos > -1 && nowpos > pos)
            {
                pos = nowpos;
            }
            else
            {
                return false;
            }
            
            
        }

        return true;
    }

    public void GetAnswer1()
    {
        InputF_1.text = "4";
    }

    public void GetAnswer2()
    {
        InputF_2.text = "if ( set_ink.equals( \"setink\" ) ) {";
    }

    private void EndofQuest()
    {
        InputF_2.text = null;
        QuestDialogBox.SetActive(false);//화면에서 없앰
        DialogueManager.instance.Q3completed = true;
        (DialogueManager.instance.DialogueBox).SetActive(true);
        DialogueManager.instance.DequeueDialogue();
    }

}
