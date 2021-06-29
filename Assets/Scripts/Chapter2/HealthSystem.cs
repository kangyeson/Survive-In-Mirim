using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int health;
    public int numOfHearts=5;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public GameObject GameOver;

    public static HealthSystem instance;
    

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

    void Update()
    {
        if(health > numOfHearts){
            health = numOfHearts;
        }   
    }

    public void outHealth()
    {
        if(health == 1){
            health = health-1;
            Invoke("GameOverSystem", 1.8f);
        }else{
            health = health-1;
        }
        for(int i=0; i<hearts.Length; i++){
            if(i<health){
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }

            if(i<numOfHearts){
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
        PlayerPrefs.SetInt("Heart", health);
        PlayerPrefs.Save(); 
    }

    public void delHeart()
    {
       for(int i=0; i<hearts.Length; i++){
            if(i<health){
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }
       } 
    }
    
    public void GameOverSystem(){
        GameOver.SetActive(true);
    }
}
