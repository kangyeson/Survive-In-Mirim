using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenURL : MonoBehaviour
{
    public void MirimHomeURL()
    {
        Application.OpenURL("https://www.e-mirim.hs.kr/");
    }
}
