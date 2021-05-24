using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject choosePlayerDeckBGBg, chooseOpponentDeckBG;
    public AudioSource clickButton;
    
    void Awake()
    {
        choosePlayerDeckBGBg.SetActive(false);
        chooseOpponentDeckBG.SetActive(false);
    }

    public void StartBtn()
    {
        clickButton.Play();
        choosePlayerDeckBGBg.SetActive(true);
    }

    public void IcePlayerBtn()
    {
        FirstChoose("Ice");
    }
    public void ForestPlayerBtn()
    {
        FirstChoose("Forest");
    }
    public void FlamePlayerBtn()
    {
        FirstChoose("Flame");
    }
    public void DesertPlayerBtn()
    {
        FirstChoose("Desert");
    }
    public void RandomPlayerBtn()
    {
        int rand = Random.Range(1,5);
        switch(rand)
        {
            case 1:
                FirstChoose("Ice");
            break;
            case 2:
                FirstChoose("Forest");
            break;
            case 3:
                FirstChoose("Flame");
            break;
            case 4:
                FirstChoose("Desert");
            break;
        }
    }
    private void FirstChoose(string element)
    {
        clickButton.Play();
        StaticCollection.playerElement = element;
        chooseOpponentDeckBG.SetActive(true);
        choosePlayerDeckBGBg.SetActive(false);
    }


    public void IceEnemyBtn()
    {
        SecondChoose("Ice");
    }
    public void ForestEnemyBtn()
    {
        SecondChoose("Forest");
    }
    public void FlameEnemyBtn()
    {
        SecondChoose("Flame");
    }
    public void DesertEnemyBtn()
    {
        SecondChoose("Desert");
    }
    public void RandomEnemyBtn()
    {
        int rand = Random.Range(1,5);
        switch(rand)
        {
            case 1:
                SecondChoose("Ice");
            break;
            case 2:
                SecondChoose("Forest");
            break;
            case 3:
                SecondChoose("Flame");
            break;
            case 4:
                SecondChoose("Desert");
            break;
        }
    }
    private void SecondChoose(string element)
    {
        clickButton.Play();
        StaticCollection.enemyElement = element;
        SceneManager.LoadScene("Game");
    }

    public void FirstBack()
    {
        clickButton.Play();
        choosePlayerDeckBGBg.SetActive(false);
    }
    public void SecondBack()
    {
        clickButton.Play();
        chooseOpponentDeckBG.SetActive(false);
    }

    public void Exit()
    {
        clickButton.Play();
        Application.Quit();
    }
}
