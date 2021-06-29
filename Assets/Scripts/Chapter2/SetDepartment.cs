using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Event")]
public class SetDepartment : ScriptableObject
{
    public void SetSoft()
    {
        PlayerPrefs.SetInt("Department",0);
        PlayerPrefs.Save(); 
    }

    public void Setsolu()
    {
        PlayerPrefs.SetInt("Department",1);
        PlayerPrefs.Save(); 
    }

    public void Setde()
    {
        PlayerPrefs.SetInt("Department",2);
        PlayerPrefs.Save(); 
    }
}
