using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Ch2_Quest5Manager : MonoBehaviour
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

    public static Ch2_Quest5Manager instance;

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
        QuestInfo = new Queue<QuestBase.Info>();  //�ʱ�ȭ
    }

    public void EnqueueQuest(QuestBase db)
    {
        Portrait.sprite = portraitImage;
        Character.sprite = characterPortrait;
        //�̹��� ������ ����
        RectTransform rt = (RectTransform)Portrait.transform;
        rt.sizeDelta = new Vector2(0, 1243);
        Quest.SetActive(true);
        Portrait.gameObject.SetActive(false); //�ʱ⿣ �ڵ� �̹��� NOT show
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
        if (QuestInfo.Count == dialogtotalcnt)
        {
            Character.gameObject.SetActive(true);
        }
        else if(QuestInfo.Count == dialogtotalcnt-4)
        {
            Portrait.gameObject.SetActive(true);
        }
        else if (QuestInfo.Count == 2) //������
        {
            Character.gameObject.SetActive(false);
            DialogBox.SetActive(false);
            ChoicesPack.SetActive(true);
        }
        else if (QuestInfo.Count == 0) //Quest ���̾�α� ������
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
        {"scores.item", "scores.item()", "scores.key()", "scores.keys()"};
    private string answer = "scores.items()";

    public void chooseAnswer(int number) //Trigger choice one
    {
        QuestManager.instance.startLoading(number == answerNumber);

        //������ �ִϸ��̼�
        if (number == answerNumber) //���� ���� ���
        {
            QuestBase.Info info = QuestInfo.Dequeue();
            dialogueName.text = info.myName;
            dialogueText.text = info.myText;
            ChoicesPack.gameObject.SetActive(false);
        }
        else
        {
            ChoicesPack.gameObject.SetActive(false);
            DialogBox.SetActive(true);
            dialogueName.text = "�����";
            dialogueText.text = "�߸��� �����ΰͰ���!";
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
