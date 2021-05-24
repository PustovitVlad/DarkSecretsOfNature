using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.IO;


public class ConnectionForSqliteDB : MonoBehaviour
{
    public static ConnectionForSqliteDB instance;

    public SqliteConnection dbConnection;

    private string _path;

    void Awake()
    {
        if(instance == null)
            instance = this;
        
        SetConnection();
    }

    public void SetConnection()
    {
        _path = Application.dataPath + "/SqliteDB/DarkSecretsOfNature.db";
        Debug.Log(_path);
        dbConnection = new SqliteConnection("URI=file:" + _path);

        dbConnection.Open();
    }

    public void CreateDeck(string element, List<Card> deck)
    {
        if(dbConnection.State == ConnectionState.Open)
        {
            Debug.Log("I`m open");

            SqliteCommand selectCreatures = new SqliteCommand();

            selectCreatures.Connection = dbConnection;
            selectCreatures.CommandText = "select * from cards_creatures where element = '" + element + "'";

            SqliteDataReader readerCreatures = selectCreatures.ExecuteReader();

            while(readerCreatures.Read())
            {
                deck.Add(new Card(
                    readerCreatures[0].ToString(), 
                    readerCreatures[1].ToString(), 
                    readerCreatures[2].ToString(), 
                    readerCreatures[3].ToString(), 
                    int.Parse(readerCreatures[4].ToString()), 
                    int.Parse(readerCreatures[5].ToString()), 
                    int.Parse(readerCreatures[6].ToString()),
                    (Card.AbilityType) System.Enum.Parse(typeof(Card.AbilityType), readerCreatures[7].ToString(), true) 
                ));
            }



            SqliteCommand selectSpells = new SqliteCommand();

            selectSpells.Connection = dbConnection;
            selectSpells.CommandText = "select * from cards_spells where element = '" + element + "'";

            SqliteDataReader readerSpells = selectSpells.ExecuteReader();

            while(readerSpells.Read())
            {
                deck.Add(new SpellCard(
                    readerSpells[0].ToString(), 
                    readerSpells[1].ToString(), 
                    readerSpells[2].ToString(), 
                    readerSpells[3].ToString(),
                    int.Parse(readerSpells[4].ToString()),
                    (SpellCard.SpellType) System.Enum.Parse(typeof(SpellCard.SpellType), readerSpells[5].ToString(), true), 
                    int.Parse(readerSpells[6].ToString()),
                    (SpellCard.TargetType) System.Enum.Parse(typeof(SpellCard.TargetType), readerSpells[7].ToString(), true)
                ));
            }
        }
        else
        {
            Debug.Log("DB not open");
        }
    }
}
