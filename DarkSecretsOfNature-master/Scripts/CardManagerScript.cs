using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public enum AbilityType
    {
        NO_ABILITY,
        INSTANT_ACTIVE,
        DOUBLE_ATTACK,
        PROVOCATION,
        REGENERATION,
        STRENGTH_GAIN,
        SHIELD,
        STRENGTH_GAIN_CARDS,
        DAMAGE_CARDS,
        DESTROY_CARDS
    }
    public string name, element, description;
    public Sprite logo;
    public int attack, health, cost; 
    public bool canAttack;
    public bool isPlaced; //фикс бага с повторным отнятием энергии

    public List<AbilityType> abilities;
    public bool isSpell;



    public bool isAlive
    {
        get
        {
            return health > 0;
        }
    }
    public bool hasAbility
    {
        get
        {
            return abilities.Count > 0;
        }
    }
    public bool isProvocation
    {
        get
        {
            return abilities.Exists(x => x == AbilityType.PROVOCATION);
        }
    }


    //public int timesTookDamage;
    public int timesDealeDamage;

    public Card(string name, string element, string description, string logoPath, int attack, int health, int cost, AbilityType abilityType = 0)
    {
        this.name = name;
        this.element = element;
        this.description = description;
        logo = Resources.Load<Sprite>(logoPath);
        this.attack = attack;
        this.health = health;
        this.cost = cost;
        canAttack = false;
        isPlaced = false;
        isSpell = false;

        abilities = new List<AbilityType>();

        if(abilityType != 0)
            abilities.Add(abilityType);

        timesDealeDamage = 0;// для дабл атаки
    }

    public Card(Card card)
    {
        name = card.name;
        element = card.element;
        description = card.description;
        logo = card.logo;
        attack = card.attack;
        health = card.health;
        cost = card.cost;
        canAttack = false;
        isPlaced = false;
        isSpell = false;

        abilities = new List<AbilityType>(card.abilities);

        timesDealeDamage = 0;// для дабл атаки
    }

    public void GetDamage(int dmg)
    {
        if(abilities.Exists(x => x == AbilityType.SHIELD))
            abilities.Remove(AbilityType.SHIELD);
        else
            health -= dmg;   
    }

    public Card GetCopy()
    {
        return new Card(this);
    }
}

public class SpellCard : Card
{
    public enum SpellType
    {
        HEAL_CARDS,
        DAMAGE_CARDS,
        HEAL_CARD,
        DAMAGE_CARD,
        DAMAGE_HERO,
        HEAL_HERO,
        ADD_PROVOCATION,
        DESTROY_CARD,
        ADD_DOUBLE_ATTACK,
        DESTROY_ALL_CARDS
    }

    public enum TargetType
    {
        NO_TARGET,
        ALLY_CARD_TARGET,
        ENEMY_CARD_TARGET
    }

    public SpellType spell;
    public TargetType spellTarget;
    public int spellValue;

    public SpellCard(string name, string element, string description, string logoPath, int cost, 
                SpellType spellType = 0, int spellVal = 0, TargetType targetType = 0) :
                base(name, element, description, logoPath, 0, 0, cost)
    {
        isSpell = true;
        spell = spellType;
        spellTarget = targetType;
        spellValue = spellVal;
    }

    public SpellCard(SpellCard card) : base(card)
    {
        isSpell = true;

        spell = card.spell;
        spellTarget = card.spellTarget;
        spellValue = card.spellValue;
    }

    public new SpellCard GetCopy()
    {
        return new SpellCard(this);
    }

}

public static class CardManager //хранение всех карт
{
    public static List<Card> allPlayerDeckCards = new List<Card>();
    public static List<Card> allEnemyDeckCards = new List<Card>();
}

public class CardManagerScript : MonoBehaviour
{
    public void Awake()
    {
        //CreateDeckWithoutDB(StaticCollection.playerElement, CardManager.allPlayerDeckCards);
        //CreateDeckWithoutDB(StaticCollection.enemyElement, CardManager.allEnemyDeckCards);
        
        ConnectionForSqliteDB.instance.CreateDeck(StaticCollection.playerElement, CardManager.allPlayerDeckCards);
        ConnectionForSqliteDB.instance.CreateDeck(StaticCollection.playerElement, CardManager.allEnemyDeckCards);
    }


  




}
