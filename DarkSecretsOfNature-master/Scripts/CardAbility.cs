using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbility : MonoBehaviour
{
    public CardControllerScript cardController;

    public GameObject shield, provocation;

    public void OnCast()
    {
        foreach(var ability in cardController.thisCard.abilities)
        {
            switch(ability)
            {
                case Card.AbilityType.INSTANT_ACTIVE:
                    cardController.thisCard.canAttack = true;

                    if(cardController.isPlayerCard)
                        cardController.info.HighlightCard(true);
                break;

                case Card.AbilityType.SHIELD:
                    shield.SetActive(true);
                break;

                case Card.AbilityType.PROVOCATION:
                    provocation.SetActive(true);
                break;

                case Card.AbilityType.STRENGTH_GAIN_CARDS: //увеличение атаки всех своих карт
                    var allyCards = cardController.isPlayerCard ? 
                                    GameManagerScript.instance.playerFieldCards : 
                                    GameManagerScript.instance.enemyFieldCards;

                    foreach(var card in allyCards)
                    {
                        card.thisCard.attack += 2000;
                        card.info.RefreshData();
                    } 
                break;

                case Card.AbilityType.DAMAGE_CARDS: //нанесение урона всем вражеским картам
                    var allCards = cardController.isPlayerCard ? 
                                    GameManagerScript.instance.enemyFieldCards : 
                                    GameManagerScript.instance.playerFieldCards;

                    foreach(var card in allCards)
                    {
                        card.thisCard.GetDamage(2000);
                        //card.thisCard.health -= 2000;
                        card.info.RefreshData();
                        card.CheckForAlive();
                    } 
                break;

                case Card.AbilityType.DESTROY_CARDS: //Уничтожает все карты соперника
                    var allEnemyCards = cardController.isPlayerCard ? 
                                    GameManagerScript.instance.enemyFieldCards : 
                                    GameManagerScript.instance.playerFieldCards;

                    foreach(var card in allEnemyCards)
                    {
                        card.thisCard.health = 0;
                        card.info.RefreshData();
                        card.CheckForAlive();
                    } 
                break;
            }

        }
    }
    
    public void OnDamageDeal()
    {
        foreach(var ability in cardController.thisCard.abilities)
        {
            switch(ability)
            {
                case Card.AbilityType.DOUBLE_ATTACK:
                    if(cardController.thisCard.timesDealeDamage == 1)
                    {
                        cardController.thisCard.canAttack = true;

                        if(cardController.isPlayerCard)
                            cardController.info.HighlightCard(true);
                    }
                break;
                    
            }

        }
    }

    public void OnDamageTake()
    {
        shield.SetActive(false);
        /*foreach(var ability in cardController.thisCard.abilities)
        {
            switch(ability)
            {
                case Card.AbilityType.SHIELD:
                    shield.SetActive(true);
                break;
                    
            }

        }*/
    }

    public void OnNewTurn()
    {
        cardController.thisCard.timesDealeDamage = 0;
        foreach(var ability in cardController.thisCard.abilities)
        {
            switch(ability)
            {
                case Card.AbilityType.REGENERATION:
                    cardController.thisCard.health += 2000;
                    cardController.info.RefreshData();
                break;

                case Card.AbilityType.STRENGTH_GAIN:
                    cardController.thisCard.attack += 2000;
                    cardController.info.RefreshData();
                break;
            }

        }
    }
}
