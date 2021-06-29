using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadChap : MonoBehaviour
{
    public void loadchap1()
    {
        LoadingSceneController.Instance.LoadScene("Chapter1");
    }
    public void loadchap2()
    {
        LoadingSceneController.Instance.LoadScene("Chapter2");
    }
    public void loadchapmenu()
    {
        LoadingSceneController.Instance.LoadScene("ChapterScene");
    }
}
