﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Ch2_Quest4Manager : MonoBehaviour
{
    public static string UserName = "User";
    private string completeText, name;
    //Dialog Objects
    public GameObject Quest, DialogBox;
    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueText;
    public Image Portrait, Character;
    public GameObject ChoicesPack;
    public TextMeshProUGUI[] choices = new TextMeshProUGUI[5];
    public Sprite[] portraitImages = new Sprite[2];
    public Sprite[] characterPortrait = new Sprite[2];
    public Sprite bgPortrait;
    public Image background;

    private int answerNumber, dialogtotalcnt;
    public Queue<QuestBase.Info> QuestInfo;

    public static Ch2_Quest4Manager instance;

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
        if(PlayerPrefs.HasKey("Name"))
        {
            UserName = PlayerPrefs.GetString("Name");
        }
        else
        {
            UserName = "User";
        }
        QuestInfo = new Queue<QuestBase.Info>();  //초기화
    }


    public void EnqueueQuest(QuestBase db)
    {
        Portrait.sprite = portraitImages[0];
        Character.sprite = characterPortrait[1];
        //이미지 사이즈 지정
        RectTransform rt = (RectTransform)Portrait.transform;
        rt.sizeDelta = new Vector2(0, 1275);
        Quest.SetActive(true);
        Portrait.gameObject.SetActive(false); //초기엔 코드 이미지 NOT show
        QuestInfo.Clear();

        foreach (QuestBase.Info info in db.QuestInfo)
        {
            QuestInfo.Enqueue(info);
        }
        dialogtotalcnt = QuestInfo.Count;
        answerNumber = Random.Range(0, 4); //정답-매번 순서 섞임 / 정답 번호 부여

        background.sprite = bgPortrait;
        RectTransform rt2 = (RectTransform)background.transform;
        rt2.sizeDelta = new Vector2(Screen.height, 0);
        setChoiceText();
        DequeueQuest();
    }

    private bool flag = true; //기본값은 true

    public void DequeueQuest()
    {
        if (QuestInfo.Count.Equals(dialogtotalcnt - 1))
        {
            Portrait.gameObject.SetActive(true);
            Character.gameObject.SetActive(true);
        }
        else if(QuestInfo.Count.Equals(dialogtotalcnt - 2))
        {
            Portrait.sprite = portraitImages[1];
        }
        else if(QuestInfo.Count.Equals(2)) //선택지
        {
            Character.gameObject.SetActive(false);
            DialogBox.SetActive(false);
            ChoicesPack.SetActive(true);
            return;
        }
        else if (QuestInfo.Count.Equals(0)) //Quest 다이얼로그 끝나면
        {
            Character.gameObject.SetActive(false);
            QuestManager.instance.spinStar();
            Invoke("EndofQuest", 4.5f);
            return;
        }

        QuestBase.Info info = QuestInfo.Dequeue();
        completeText = info.myText;
        name = info.myName;
        completeText = completeText.Replace("[User]", UserName);
        name = name.Replace("[User]", UserName);
        dialogueName.text = name;
        dialogueText.text = completeText;
    }

    private string[] examples = new string[4]
        {"[-1::]", ".reverse(True)", ".sort(reverse=True)", ".sort(False)"};
    private string answer = ".reverse()";

    private void setChoiceText()
    {
        int j = 0;
        for (int i = 0; i < 5; i++)
        {
            if (i.Equals(answerNumber)) choices[i].text = answer;
            else choices[i].text = examples[j++]; //j<4
        }
    }

    public void chooseAnswer(int choiceNumber) //Trigger choice one
    {
        QuestManager.instance.startLoading(choiceNumber.Equals(answerNumber));

        //컴파일 애니메이션
        if (choiceNumber.Equals(answerNumber)) //정답 맞춘 경우
        {
            Portrait.gameObject.SetActive(false);
            Character.gameObject.SetActive(true);
            QuestBase.Info info = QuestInfo.Dequeue();
            DialogBox.SetActive(true);
            completeText = info.myText;
            name = info.myName;
            completeText = completeText.Replace("[User]", UserName);
            name = name.Replace("[User]", UserName);
            dialogueName.text = name;
            dialogueText.text = completeText;
            ChoicesPack.gameObject.SetActive(false);
        }
        else
        {
            ChoicesPack.gameObject.SetActive(false);
            DialogBox.SetActive(true);
            dialogueName.text = "디버거";
            dialogueText.text = "잘못된 정답인것같아!";
            flag = false;
            Invoke("healout", 3f);
        }
    }

    private void healout(){
        HealthSystem.instance.outHealth();
    }

    private void EndofQuest()
    {
        Quest.SetActive(false);
        DialogueManager2.instance.Qcompleted[3] = true;
        (DialogueManager2.instance.DialogueBox).SetActive(true);
        DialogueManager2.instance.DequeueDialogue();
    }
}