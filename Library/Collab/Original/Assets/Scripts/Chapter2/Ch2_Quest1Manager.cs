﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Ch2_Quest1Manager : MonoBehaviour
{
    //Dialog Objects
    public GameObject Quest, DialogBox;
    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueText;
    public Image Portrait, Character;
    public GameObject ChoicesPack;
    public TextMeshProUGUI[] choices = new TextMeshProUGUI[5];
    public Sprite portraitImage;
    public Sprite characterPortrait;

    private int answerNumber, dialogtotalcnt;
    public Queue<QuestBase.Info> QuestInfo;

    public static Ch2_Quest1Manager instance;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("fix this" + gameObject.name);
        }
        else
        {
            instance = this;
        }
    }

    public void Start()
    {
        QuestInfo = new Queue<QuestBase.Info>();  
    }

    public void EnqueueQuest(QuestBase db)
    {
        Portrait.sprite = portraitImage;
        Character.sprite = characterPortrait;
        //이미지 사이즈 지정
        RectTransform rt = (RectTransform)Portrait.transform;
        rt.sizeDelta = new Vector2(0, 1243);
        Quest.SetActive(true);
        Portrait.gameObject.SetActive(false); //초기엔 코드 이미지 NOT show
        QuestInfo.Clear();

        foreach (QuestBase.Info info in db.QuestInfo)
        {
            QuestInfo.Enqueue(info);
        }
        dialogtotalcnt = QuestInfo.Count;
        answerNumber = Random.Range(0, 5);

        DequeueQuest();
    }

    private bool flag = true; //�⺻���� true

    public void DequeueQuest()
    {
        if (QuestInfo.Count == dialogtotalcnt - 1)
        {
            Portrait.gameObject.SetActive(true); //문제 최초 등장
        }
        else if (QuestInfo.Count == dialogtotalcnt - 2)
        {
            Character.gameObject.SetActive(true); //디버거 캐릭터 등장
        }
        else if (QuestInfo.Count == 3) //선택지 등장
        {
            DialogBox.SetActive(false);
            Debug.Log("퀘스트1");
            Character.gameObject.SetActive(false);
            ChoicesPack.SetActive(true);
        }
        else if (QuestInfo.Count == 0) //Quest 다이얼로그 끝나면
        {
            QuestManager.instance.spinStar();
            Invoke("EndofQuest", 4.5f);
            return;
        }

        QuestBase.Info info = QuestInfo.Dequeue();
        dialogueName.text = info.myName;
        dialogueText.text = info.myText;

    }

    private string[] examples = new string[4]
        {"opening = Part.__init__(this, t1, \"개막사\")",
            "opening = Part(self, t1, \"개막사\")",
            "opening = Part.__init__(t1, \"개막사\")",
            "opening = Part(this, t1, \"개막사\")"};
    private string answer = "opening = Part(t1, \"개막사\")";

    private void setChoiceText()
    {
        int j = 0;
        for (int i = 0; i < 5; i++)
        {
            if (i == answerNumber) choices[i].text = answer;
            choices[i].text = examples[j++]; //j<4
        }
    }

    public void chooseAnswer(int choiceNumber) //Trigger choice one
    {
        QuestManager.instance.startLoading(choiceNumber == answerNumber);

        //컴파일 애니메이션
        if (choiceNumber == answerNumber) //정답 맞춘 경우
        {
            Character.gameObject.SetActive(true);
            QuestBase.Info info = QuestInfo.Dequeue();
            dialogueName.text = info.myName;
            dialogueText.text = info.myText;
            ChoicesPack.gameObject.SetActive(false);
        }
        else
        {
            ChoicesPack.gameObject.SetActive(false);
            DialogBox.SetActive(true);
            dialogueName.text = "디버거";
            dialogueText.text = "잘못된 정답인것같아!";
            flag = false;
        }
    }

    private void EndofQuest()
    {
        Quest.SetActive(false);
        DialogueManager2.instance.Qcompleted[0] = true;
        (DialogueManager.instance.DialogueBox).SetActive(true);
        DialogueManager.instance.DequeueDialogue();
    }
}