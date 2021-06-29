using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class QuestStarter2 : MonoBehaviour
{
    public QuestBase[] quests = {};
    public GameObject splashImage;
    public Image IndexImage; //=splashImage
    public Sprite[] idxs = {}; //포트레잇 이미지(퀘스트 문제 이미지)
    public GameObject QuestObject;
    public int questnum;
    public AudioClip quest_effectSound;
    AudioSource soundSource;


    public void start()
    {
        soundSource = GetComponent<AudioSource>();
        Invoke("splashIndex", 2f);
    }

    public void splashIndex()
    {
        soundSource.clip = quest_effectSound;
        soundSource.Play();
        IndexImage.sprite = idxs[questnum-1];
        splashImage.SetActive(true); //퀘스트 인덱스 이미지
        Invoke("TriggerDialogue", 3f);
    }

    public void TriggerDialogue()
    {
        
        splashImage.SetActive(false); //퀘스트 인덱스 이미지
        QuestObject.SetActive(true);
        switch (questnum)
        {
            case 1:
                Ch2_Quest1Manager.instance.EnqueueQuest(quests[questnum - 1]);
                break;
            case 2:
                Ch2_Quest2Manager.instance.EnqueueQuest(quests[questnum - 1]);
                break;
            case 3:
                Ch2_Quest3Manager.instance.EnqueueQuest(quests[questnum - 1]);
                break;
            case 4:
                Ch2_Quest4Manager.instance.EnqueueQuest(quests[questnum - 1]);
                break;
            case 5:
                Ch2_Quest5Manager.instance.EnqueueQuest(quests[questnum - 1]);
                break;
            default: break;
        }
        
    }
}
