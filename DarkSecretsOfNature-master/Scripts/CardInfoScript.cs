﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScript : MonoBehaviour
{
    public CardControllerScript cardController;
    //public Card SelfCard;
    public Image logo;
    public Text name, attack, health, element, description;
    public TextMeshProUGUI cost;
    public GameObject hideObj, highlitedObj;
    public AudioSource cardAttackAudio, cardDieAudio, cardCastAudio;
    //public bool IsPlayer;

    public void HideCardInfo(/*Card card*/) //скрытие руки соперника
    {
        //SelfCard = card;
        hideObj.SetActive(true);
        health.text = "";
        attack.text = "";
        cost.text = "";
        //IsPlayer = false;
    }

    public void ShowCardInfo(/*Card card, bool isPlayer*/)
    {
        //IsPlayer = isPlayer;
        hideObj.SetActive(false);
        //SelfCard = card;

        this.logo.sprite = cardController.thisCard.logo;
        logo.preserveAspect = true;
        name.text = cardController.thisCard.name;
        element.text = cardController.thisCard.element;
        description.text = cardController.thisCard.description;

        if(cardController.thisCard.isSpell)
        {
            attack.gameObject.SetActive(false);
            health.gameObject.SetActive(false);
        }

        RefreshData();
    }

    public void RefreshData()
    {
        attack.text = cardController.thisCard.attack.ToString();
        health.text = cardController.thisCard.health.ToString();
        cost.text = cardController.thisCard.cost.ToString();
    }

    public void HighlightCard(bool highlight) // подсветка карт
    {
        highlitedObj.SetActive(highlight);
    }

    public void HighlightManaAvailability(int currentEnergy)//прозрачность карт, которые нельзя разыграть
    {
        GetComponent<CanvasGroup>().alpha = currentEnergy >= cardController.thisCard.cost ? 1: 0.7f; 
    }

    public void HighlightAsTarget(bool highlite)// подсветка карт для атаки
    {
        highlitedObj.SetActive(highlite);
    }

    
}
