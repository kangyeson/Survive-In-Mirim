using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

// [InitializeOnLoad]
public class SaveNameFadeIn : MonoBehaviour
{
    [SerializeField]
    public TMP_InputField NameField;
    public Image FadeImage;
    public GameObject Lockchap2;
    float time = 0f;
    float F_time = 2f;
    public void Awake()
    {
        StartCoroutine(FadeFlow());
    }

    IEnumerator FadeFlow()
    {
        if (PlayerPrefs.HasKey("Name"))
        {
            string UserName = PlayerPrefs.GetString("Name");
            NameField.text = UserName;
        }

        if (PlayerPrefs.HasKey("Chapter1"))
        {
            Lockchap2.gameObject.SetActive(false);
        }

        FadeImage.gameObject.SetActive(true);
        time = 0f;
        Color alpha = FadeImage.color;
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            FadeImage.color = alpha;
            yield return null;
        }
        yield return null;
        FadeImage.gameObject.SetActive(false);
    }
}
