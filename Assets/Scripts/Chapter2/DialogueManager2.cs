using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;

public class DialogueManager2 : MonoBehaviour
{
    public static DialogueManager2 instance;
    private void Awake()
    {
        if (instance != null)
        {
        }
        else
        {
            instance = this;
        }
    }

    public static string UserName = "User";
    public AudioClip crowdshoutSound; //�������
    public AudioClip outcrowdSound;
    public AudioClip wheeSound;
    public AudioClip twothreeSound;
    public AudioClip jumpropeSound;
    public AudioClip duguduguSound;


    public GameObject DialogueBox;
    public TextMeshProUGUI dialogueName;
    public TextMeshProUGUI dialogueText;
    public Image dialoguePortrait;
    public Image backgroundPortrait;
    public float delay = 2f;
    public QuestStarter2 questStarter;
    public DialogueButton2 DialogBtn;

    public int department;

    private string[] major = { "����Ʈ", "����", "������"};
    private string[] sub = { "����Ʈ! ����Ʈ!", "���� ���� ���֢�", "������! ������!" };
    private string[] F1 = { "�þ���", "������", "������" };
    private string[] F2 = { "������", "���츲", "����" };
    private string[] F3 = { "�����", "���ڹ�", "�����" };
    private string[] F4 = { "�����", "������", "�����" };
    private string[] T2 = { "������ ������", "������ ������", "���Ͼ� ������" };
    private string[] T2_1 = { "�������� ������ ��ٷ�. �� �������ǰž�.", "�������� ������ ��ٷ�. �� �������ǰž�.", "��, ���� �� �𸣰ڳ�. ���弱������ ��� ��� ���̳���." };
    private string[] T2_2 = { "���� �� �� �԰��! ���� ������� ���� �� ���� ���ٿ�. �̵� ����ߵǴµ�.", "���� �� �� �԰��! ���� ������� ���� �� ���� ���ٿ�. �̵� ����ߵǴµ�.", "��ħ�� �� �Ծ���? ���ð��� ���� �� �Ծ����~" };
    private string[] text1 = { "�;ƾƾ�!! ��~�̰� �ι̰� �ְ� �ι̰�~!", "�;ƾ�!!!! ����!!", "�����! ��־־ֹ�!!" };
    private string[] text2 = { "�װ��� ����� ���� ���߱���������. �������� ��", "����! ����! �ַ�!! ���� ����!", "�;ƾ�!!!! 1�� �Դ� �������� è�Ǿ�!!!" };
    private string[] text3 = { "���� �Ф�", "����! ������ ������ ����~ �츮�� ������ ��ӵȴ�!", "�̾�~ ���� ��ɸǵ�! ����� ���ϸ� ��ϳ�~!" };
    private string[] text4 = { "�� ������ȭ�Ф� �����Ҿ� �ְ� �ι̰�~!", "���� �׾�� �� ������ �����ֳפ���", "���� �� ������ �ִ�!" };
    private string[] text5 = { "�������� �����̳�, �̺� ��!", "������ �̺����� �޷�! ������ �԰� ����!!", "��ü�� ȸ�� ����~~~!!!" };

    public Sprite[] F1_1, F1_2, F1_3, F1_4, F1_5;
    public Sprite[] F2_1, F2_2_1, F2_2, F2_3;
    public Sprite[] F3_1, F3_2, F4_1, F4_2;
    public Sprite[] T1;
    public Sprite emptySprite;

    public Sprite[] teachers = new Sprite[3], outstandzone = new Sprite[3], 
        lineup = new Sprite[3], cheerzone = new Sprite[3], 
        endBg = new Sprite[3], jumpropeBg1 = new Sprite[3], jumpropeBg2 = new Sprite[3];
    public Sprite ch1startBg;

    private int[] pos_F1_1 = new int[] { 62, 66, 75 };
    private int[] pos_F1_2 = new int[] { 0, 1, 105 };
    private int[] pos_F1_3 = new int[] { 99, 115, 116 };
    private int[] pos_F1_4 = new int[] { 2, 3, 4, 16};
    private int[] pos_F2_1 = new int[] { 63, 76, 89, 113 };
    private int[] pos_F2_2 = new int[] { 80, 84, 86 };
    private int[] pos_F2_2_1 = new int[] { 59 };
    private int[] pos_F2_3 = new int[] { 109 };
    private int[] pos_F3_1 = new int[] { 34, 47, 48, 49, 98 };
    private int[] pos_other_team = new int[] { 38, 42, 44 };
    private int[] pos_F3_2 = new int[] { 106 };
    private int[] pos_F4_1 = new int[] { 21, 23, 92 };
    private int[] pos_F4_2 = new int[] { 94, 108, 112 };
    private int[] pos_T1 = new int[] { 7, 9 };



    public bool isCurrentlyTyping;
    private string completeText, name;
    public int thisId;
    private bool isDelayturn;
    public GameObject MedalAnimation;
    public GameObject endingAnimation;
    public GameObject MedalGround;

    public Queue<DialogueBase.Info> dialogueInfo;
    public Queue<PrologueBase.Info> prologueInfo;

    public bool[] Qcompleted = new bool[5];
    private AudioSource audio; //����� ����� �ҽ� ������Ʈ

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
        audio = GetComponent<AudioSource>();
    }

    public void EnqueueDialogue(DialogueBase db)
    {
        dialogueInfo = new Queue<DialogueBase.Info>();  //���̾�α� �ʱ�ȭ
        DialogueBox.SetActive(true); //ȭ�鿡 ���
        dialogueInfo.Clear();

        foreach (DialogueBase.Info info in db.dialogueInfo)
        {
            dialogueInfo.Enqueue(info);
        }
        isDelayturn = false;
        DequeueDialogue();
    }

    //���̺�� thisId�����Ͱ� ����Ʈ�κ��϶�
    public void QuestDialogue(DialogueBase db)
    {
        dialogueInfo = new Queue<DialogueBase.Info>();
        foreach (DialogueBase.Info info in db.dialogueInfo)
        {
            dialogueInfo.Enqueue(info);
        }
        for (int i = 0; i < thisId; i++)
        {
            dialogueInfo.Dequeue(); //thisId���� ���� ���� thisId ����
        }
        DequeueDialogue();
        DialogueBox.SetActive(false);
    }

    //���̺�� thisId������ �ε�
    public void LoadDialogue(DialogueBase db)
    {
        dialogueInfo = new Queue<DialogueBase.Info>();
        DialogueBox.SetActive(true); //ȭ�鿡 ���
        foreach (DialogueBase.Info info in db.dialogueInfo)
        {
            dialogueInfo.Enqueue(info);
        }
        for (int i = 0; i < thisId; i++)
        {
            dialogueInfo.Dequeue(); //thisId���� ���� ���� thisId ����
        }
        DequeueDialogue();
    }

    public void DequeueDialogue()
    {
        if (!isCurrentlyTyping) //�� ��� Ÿ������ ������ ����
        {
            if (dialogueInfo.Count.Equals(0)) //é�� 2 ����
            {
                DialogueBox.SetActive(false);
                EndofDialogue();

            }
            else
            { //���̾�α� ����
                if (isDelayturn)
                {
                    delayDialog(); return;
                }

                DialogueBox.SetActive(true);

                lock (dialogueInfo)
                {
                    if ((thisId.Equals(13)) && (!Qcompleted[0])) //����Ʈ 1 ����
                    {
                        backgroundPortrait.sprite = ch1startBg;
                        DialogueBox.SetActive(false);
                        DialogBtn.questnum = 1;
                        questStarter.questnum = 1;
                        questStarter.start();
                        return;
                    }
                    else if ((thisId.Equals(29)) && (!Qcompleted[1])) //����Ʈ 2 ����
                    {
                        DialogueBox.SetActive(false);
                        DialogBtn.questnum = 2;
                        questStarter.questnum = 2;
                        questStarter.start();
                        return;
                    }
                    else if ((thisId.Equals(45)) && (!Qcompleted[2])) //����Ʈ 3 ����
                    {
                        DialogueBox.SetActive(false);
                        DialogBtn.questnum = 3;
                        questStarter.questnum = 3;
                        questStarter.start();
                        return;
                    }
                    else if ((thisId.Equals(86)) && (!Qcompleted[3])) //����Ʈ 4 ����
                    {
                        DialogueBox.SetActive(false);
                        DialogBtn.questnum = 4;
                        questStarter.questnum = 4;
                        questStarter.start();
                        return;
                    }
                    else if ((thisId.Equals(102)) && (!Qcompleted[4])) //����Ʈ 5 ����
                    {
                        DialogueBox.SetActive(false);
                        DialogBtn.questnum = 5;
                        questStarter.questnum = 5;
                        questStarter.start();
                        return;
                    }

                    dialogueText.text = "";

                    DialogueBase.Info info = dialogueInfo.Dequeue();
                    thisId = info.id;
                    completeText = info.myText;
                    name = info.myName;
                    completeText = completeText.Replace("[User]", UserName);
                    name = name.Replace("[User]", UserName);
                    int department = PlayerPrefs.GetInt("Department");

                    Sprite bgSprite = info.bgSprite;

                    if (bgSprite == null) //����̹����� ������(������ ���� �����ϴ� ����̸�)
                    {
                        if (thisId <= 4) bgSprite = outstandzone[department];
                        else if (thisId == 5 || thisId == 10 || thisId == 11 || thisId == 12) bgSprite = lineup[department];
                        else if (thisId >= 6 && thisId <= 9) bgSprite = teachers[department];
                        else if(new int[] {53, 58, 59}.Contains(thisId)) bgSprite = jumpropeBg1[department];
                        else if(new int[] {55, 56, 57}.Contains(thisId)) bgSprite = jumpropeBg2[department];
                        else if (thisId == 52 || thisId == 54 || (thisId >= 68 && thisId <= 91)) bgSprite = cheerzone[department];
                        else if (thisId >= 110) bgSprite = endBg[department];

                    }

                    if ((int)bgSprite.bounds.size.x == (int)bgSprite.bounds.size.y)
                    {
                        RectTransform rt = (RectTransform)backgroundPortrait.transform;
                        rt.sizeDelta = new Vector2(2160, 0);
                    }
                    else
                    {
                        RectTransform rt = (RectTransform)backgroundPortrait.transform;
                        rt.sizeDelta = new Vector2(1080, 0);
                    }



                    backgroundPortrait.sprite = bgSprite;
                    

                    //���� ���� �̸��� ����
                    completeText = completeText.Replace("[F1]", F1[department]);
                    name = name.Replace("[F1]", F1[department]);
                    completeText = completeText.Replace("[F2]", F2[department]);
                    name = name.Replace("[F2]", F2[department]);
                    completeText = completeText.Replace("[F3]", F3[department]);
                    name = name.Replace("[F3]", F3[department]);
                    completeText = completeText.Replace("[F4]", F4[department]);
                    name = name.Replace("[F4]", F4[department]);
                    name = name.Replace("[T2]", T2[department]);
                    completeText = completeText.Replace("[T2_1]", T2_1[department]);
                    completeText = completeText.Replace("[T2_2]", T2_2[department]);
                    completeText = completeText.Replace("[text1]", text1[department]);
                    completeText = completeText.Replace("[text2]", text2[department]);
                    completeText = completeText.Replace("[text3]", text3[department]);
                    completeText = completeText.Replace("[text4]", text4[department]);
                    completeText = completeText.Replace("[text5]", text5[department]);
                    completeText = completeText.Replace("[major]", major[department]);
                    completeText = completeText.Replace("[sub]", sub[department]);

                    dialogueName.text = name;

                    Sprite p;

                    //Array.Exists(language, element => element == "Ruby")
                    //�ι� portrait
                    if (pos_F1_1.Contains(thisId)) p = F1_1[department];
                    else if (pos_F1_2.Contains(thisId)) p = F1_2[department];
                    else if (pos_F1_3.Contains(thisId)) p = F1_3[department];
                    else if (pos_F1_4.Contains(thisId)) p = F1_4[department];
                    else if (pos_F2_1.Contains(thisId)) p = F2_1[department];
                    else if (pos_F2_2.Contains(thisId)) p = F2_2[department];
                    else if (pos_F2_2_1.Contains(thisId)) p = F2_2_1[department];
                    else if (pos_F2_3.Contains(thisId)) p = F2_3[department];
                    else if (pos_F3_1.Contains(thisId)) p = F3_1[department];
                    else if (pos_F3_2.Contains(thisId)) p = F3_2[department];
                    else if (pos_F4_1.Contains(thisId)) p = F4_1[department];
                    else if (pos_F4_2.Contains(thisId)) p = F4_2[department];
                    else if (pos_T1.Contains(thisId)) p = T1[department];
                    else if (pos_other_team.Contains(thisId))
                    {
                        int other_ = 0;
                        if (department == 2) other_ = 1;
                        else other_ = department + 1;
                        p = F3_1[other_];
                    }
                    else p = info.portrait;

                    if (p == null) dialoguePortrait.sprite = emptySprite;
                    else dialoguePortrait.sprite = p;
                   
                    
                    

                    //����� ����
                    if (thisId.Equals(1))
                    {
                        GetComponent<AudioSource>().clip = outcrowdSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 10) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(55))
                    {
                        GetComponent<AudioSource>().clip = wheeSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 55) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(58))
                    {
                        GetComponent<AudioSource>().clip = jumpropeSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 58) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(61))
                    {
                        GetComponent<AudioSource>().clip = duguduguSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 61) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(69))
                    {
                        GetComponent<AudioSource>().clip = crowdshoutSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 72) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(73))
                    {
                        GetComponent<AudioSource>().clip = wheeSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 73) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(74))
                    {
                        GetComponent<AudioSource>().clip = crowdshoutSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 76) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(79))
                    {
                        GetComponent<AudioSource>().clip = twothreeSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 79) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(101))
                    {
                        GetComponent<AudioSource>().clip = duguduguSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 101) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(103))
                    {
                        GetComponent<AudioSource>().clip = duguduguSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 103) { GetComponent<AudioSource>().Stop(); }

                    if (thisId.Equals(105))
                    {
                        GetComponent<AudioSource>().clip = crowdshoutSound;
                        GetComponent<AudioSource>().Play();
                    }
                    else if (thisId > 110) { GetComponent<AudioSource>().Stop(); }

                    StartCoroutine(TypeText(completeText));

                    isDelayturn = info.isDelayed;
                }//end of lock
            }
        }
        else
        {
            StopAllCoroutines();
            isCurrentlyTyping = false;
            dialogueText.text = completeText;
            return;
        }


    }

    IEnumerator TypeText(string completeText)
    {
        isCurrentlyTyping = true;
        string text = completeText;
        foreach (char c in text.ToCharArray())
        {
            yield return new WaitForSeconds(delay);
            dialogueText.text += c;
        }
        isCurrentlyTyping = false;
    }

    private void EndofDialogue()
    {
        MedalGround.SetActive(true);
        MedalAnimation.SetActive(true);
        endingAnimation.SetActive(true);
        Invoke("GoEnd", 5f);
    }

    private void GoEnd()
    {
        MedalGround.SetActive(false);
        MedalAnimation.SetActive(false);
        endingAnimation.SetActive(true);
        FadeOutScript.instance.Fade();
    }

    //��� 2�� �ڵ� ����̱� �Լ�
    private void delayDialog()
    {
        DialogueBox.SetActive(false);
        isDelayturn = false;
        Invoke("DequeueDialogue", 2f);
    }

}