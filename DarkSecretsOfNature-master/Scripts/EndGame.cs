using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Animation victoryEndGameAnim, defeatEndGameAnim;
    public AudioSource clickButton, victoryAudio, defeatAudio;
    //public GameObject vinGO, loseGO;
    void Awake()
    {
        switch(StaticCollection.endGame)
        {
            case "Victory":
                //vinGO.SetActive(true);
                victoryEndGameAnim.Play();
                victoryAudio.Play();
            break;

            case "Defeat":
                //loseGO.SetActive(true);
                defeatEndGameAnim.Play();
                defeatAudio.Play();
            break;
        }
    }

    public void ToMenu()
    {
        clickButton.Play();
        SceneManager.LoadScene("Menu");
        //vinGO.SetActive(false);
        //loseGO.SetActive(false);
    }
}
