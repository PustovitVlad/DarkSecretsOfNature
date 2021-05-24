using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Game
{
    public List<Card> enemyDeck, playerDeck;

    public Game()
    {
        enemyDeck = GiveDeckCard(CardManager.allEnemyDeckCards);
        playerDeck = GiveDeckCard(CardManager.allPlayerDeckCards);
    }

    List<Card> GiveDeckCard(List<Card> deck)
    {
        List<Card> list = new List<Card>();

        foreach(var card in deck) //заролнение колоды картами
        {
            if(card.isSpell)
                list.Add(((SpellCard)card).GetCopy());
            else
                list.Add(card.GetCopy());
        }

        for (int i = list.Count - 1; i > 0; i--) //тасовка карт
        {
            int j = Random.Range(0, i);
            Card temp = list[j];
            list[j] = list[i];
            list[i] = temp;
        }

        return list;
    }
}

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    public Game currentGame;
    public Transform enemyHand, playerHand, enemyField, playerField;
    public GameObject cardPref;
    int turn, turnTime = 25;
    public Text turnTimeText;
    public Button endTurnBtn;
    public Slider turnTimeSlider;

    public int playerEnergy, enemyEnergy;
    public Text selfEnergyTxt, enemyEnergyTxt;

    public int playerHP, enemyHP;
    public Text playerHPTxt, enemyHPTxt;

    public AttackedHeroScript enemyHero, playerHero;

    public Text playerDeckCardCountTxt, enemyDeckCardCountTxt;

    public AI enemyAI; 

    public Button addEnemyCadrBtn, addPlayerCadrBtn;
    public AudioSource audioClickAddCard;
    public Animation addEnemyCardClickAnim, addPlayerCardClickAnim;

    public AudioSource clickButton;

    public List<CardControllerScript> playerHandCards = new List<CardControllerScript>(),
                                playerFieldCards = new List<CardControllerScript>(),
                                enemyHandCards = new List<CardControllerScript>(),
                                enemyFieldCards = new List<CardControllerScript>();

    public bool isPlayerTurn
    {
        get{return turn % 2 == 0;}
    }

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        turn = 0;

        currentGame = new Game();

        GiveHandCards(currentGame.enemyDeck, enemyHand);
        GiveHandCards(currentGame.playerDeck, playerHand);
        playerEnergy = enemyEnergy = 10;
        playerHP = enemyHP = 20000;

        ShowEnergy();
        ShowHP();

        addEnemyCadrBtn.interactable = false;
        
        StartCoroutine(TurnFunk());
    }

    void GiveHandCards(List<Card> deck, Transform hand) //стартовая рука
    {
        int i = 0;
        while(i++ < 3)
            GiveCardToHand(deck, hand);
    }

    void GiveCardToHand(List<Card> deck, Transform hand) // взять новую карту
    {
        if(deck.Count == 0)
            return;
        
        CreateCardPref(deck[0], hand);
        deck.RemoveAt(0);
        ShowDeckCard();
    }

    void ShowDeckCard()
    {
        playerDeckCardCountTxt.text = currentGame.playerDeck.Count.ToString();
        enemyDeckCardCountTxt.text = currentGame.enemyDeck.Count.ToString();
    }

    void CreateCardPref(Card card, Transform hand)//создание префаба карты
    {
        GameObject cardGO = Instantiate(cardPref, hand, false);
        CardControllerScript cardC = cardGO.GetComponent<CardControllerScript>();

        cardC.Init(card, hand == playerHand);

        if(cardC.isPlayerCard)
            playerHandCards.Add(cardC);
        else
            enemyHandCards.Add(cardC);
    }

    IEnumerator TurnFunk() // карутина хода
    {
        turnTime = 25;
        turnTimeText.text = turnTime.ToString();

        foreach(var card in playerFieldCards)
            card.info.HighlightCard(false);

        CheckCardsForManaAvailability();
        
        if(isPlayerTurn)//ход игрока
        {
            foreach(var card in playerFieldCards)
            {
                card.thisCard.canAttack = true; //разрешить картам атаку
                card.info.HighlightCard(true);
                card.ability.OnNewTurn();
            }

            while(turnTime-- > 0)
            {
                turnTimeText.text = turnTime.ToString();
                int i = 1;
                while(i++ < 51)
                {
                    turnTimeSlider.value = 25 - (turnTime - i/50f);
                    yield return new WaitForSeconds(0.02f);
                }
                
                
            }
            ChangeTurn();
        }
        else//ход противника
        {
            foreach(var card in enemyFieldCards)
            {
                card.thisCard.canAttack = true;
                card.ability.OnNewTurn();
            }

            enemyAI.MakeTurn();

            while(turnTime --> 0)
            {
                turnTimeText.text = turnTime.ToString();
                turnTimeSlider.value = 25 - turnTime;
                int i = 1;
                while(i++ < 51)
                {
                    turnTimeSlider.value = 25 - (turnTime - i/50f);
                    yield return new WaitForSeconds(0.02f);
                }
            }
            ChangeTurn();
        }
        
    }

    
    public void ChangeTurn() //смена хода
    {
        StopAllCoroutines();
        turn++;
        endTurnBtn.interactable = isPlayerTurn;
        clickButton.Play();

        if(isPlayerTurn)
        {
            playerEnergy += turn/2*5;

            if(playerHandCards.Count < 5/*макс. кол-во карт в руке*/)
                GiveCardToHand(currentGame.playerDeck, playerHand);
            
        }
        else
        {
            enemyEnergy += turn/2*5;

            if(enemyHandCards.Count < 5/*макс. кол-во карт в руке*/)
            GiveCardToHand(currentGame.enemyDeck, enemyHand);
        }
        ShowEnergy();

        StartCoroutine(TurnFunk());
    }

    public void GiveNewCardForPlayer()
    {
        addPlayerCardClickAnim.Play();
        audioClickAddCard.Play();
        if(playerHandCards.Count < 5/*макс. кол-во карт в руке*/ && playerEnergy >= 50 && isPlayerTurn)
        {
                GiveCardToHand(currentGame.playerDeck, playerHand);
                ReduceEnergy(true, 50);
                ShowEnergy();
                CheckCardsForManaAvailability();
        }
    }

    public void GiveNewCardForEnemy()
    {
        addEnemyCardClickAnim.Play();
        audioClickAddCard.Play();
        if(enemyHandCards.Count < 5/*макс. кол-во карт в руке*/ && enemyEnergy >= 50 && !isPlayerTurn)
        {
                GiveCardToHand(currentGame.enemyDeck, enemyHand);
                ReduceEnergy(false, 50);
                ShowEnergy();
        }
    }

    public void CardsFight(CardControllerScript attacker, CardControllerScript defender)
    {
        attacker.OnDamageDeal();
        
        defender.OnTakeDamage();
        attacker.OnTakeDamage();
        defender.thisCard.GetDamage(attacker.thisCard.attack);
        attacker.thisCard.GetDamage(defender.thisCard.attack);

        defender.CheckForAlive();
        attacker.CheckForAlive();
    }


    void ShowEnergy()
    {
        selfEnergyTxt.text = playerEnergy.ToString();
        enemyEnergyTxt.text = enemyEnergy.ToString();
    }

    public void ShowHP()
    {
        playerHPTxt.text = playerHP.ToString();
        enemyHPTxt.text = enemyHP.ToString();
    }

    public void ReduceEnergy(bool playerEnergy, int cost)
    {
        if(playerEnergy)
            this.playerEnergy = Mathf.Clamp(this.playerEnergy - cost, 0, int.MaxValue);
        else
            enemyEnergy = Mathf.Clamp(enemyEnergy - cost, 0, int.MaxValue);

        ShowEnergy();
    }

    public void DamageHero(CardControllerScript card, bool isEnemyAttacked)
    {
        if(isEnemyAttacked)
            enemyHP -= card.thisCard.attack;
        else
            playerHP -= card.thisCard.attack;
        
        ShowHP();

        card.OnDamageDeal();
        CheckForResult();
    }

    public void CheckForResult()
    {
        if(enemyHP <= 0)
        {
            StopAllCoroutines();
            StaticCollection.endGame = "Victory";
            SceneManager.LoadScene("EndGame");
        }
        else if(playerHP <= 0)
        {
            StopAllCoroutines();
            StaticCollection.endGame = "Defeat";
            SceneManager.LoadScene("EndGame");
        }
    }

    public void CheckCardsForManaAvailability() //прозрачность карт
    {
        foreach(var card in playerHandCards)
            card.info.HighlightManaAvailability(playerEnergy);
    }

    public void HighlightTargets(CardControllerScript attacker, bool highlight)// подсветка карт для атаки
    {
        List<CardControllerScript> targets = new List<CardControllerScript>();

        if(attacker.thisCard.isSpell)
        {
            var spellCard = (SpellCard)attacker.thisCard;

            if(spellCard.spellTarget == SpellCard.TargetType.NO_TARGET)
                targets = new List<CardControllerScript>();
            else if(spellCard.spellTarget == SpellCard.TargetType.ALLY_CARD_TARGET)
                targets = playerFieldCards;
            else
                targets = enemyFieldCards;
        }
        else{
            if(enemyFieldCards.Exists(x => x.thisCard.isProvocation))
                targets = enemyFieldCards.FindAll(x => x.thisCard.isProvocation);
            else
            {
                targets = enemyFieldCards;
                enemyHero.HighlightHero(highlight);
            }
        }
        foreach(var card in targets)
            card.info.HighlightAsTarget(highlight);
        
    }
}
