using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum CardClass
{
    NEUTRAL,
    HUNTER,
    WARRIOR,
    WARLOCK,
    DRUID,
    ROGUE,
    PRIEST,
    SHAMAN,
    PALADIN,
    MAGE
}

[System.Serializable]
public class PlayRequirements
{
    public int REQ_TARGET_TO_PLAY;
    public int REQ_FRIENDLY_TARGET;
    public int REQ_MINION_TARGET;
    public int REQ_TARGET_IF_AVAILABLE;
    public int REQ_TARGET_WITH_RACE;
    public int REQ_NUM_MINION_SLOTS;
    public int REQ_TARGET_WITH_DEATHRATTLE;
    public int REQ_TARGET_FOR_COMBO;
    public int REQ_FRIENDLY_MINIONS_OF_RACE_DIED_THIS_GAME;
    public int REQ_ENEMY_TARGET;
    public int REQ_UNDAMAGED_TARGET;
    public int REQ_WEAPON_EQUIPPED;
    public int REQ_DAMAGED_TARGET;
    public int REQ_MINIMUM_ENEMY_MINIONS;
    public int REQ_NONSELF_TARGET;
    public int REQ_TARGET_MAX_ATTACK;
    public int REQ_MUST_TARGET_TAUNTER;
    public int REQ_TARGET_MIN_ATTACK;
    public int REQ_MINIMUM_TOTAL_MINIONS;
    public int REQ_HERO_TARGET;
    public int REQ_DRAG_TO_PLAY;
    public int REQ_SECRET_ZONE_CAP_FOR_NON_SECRET;
    public int REQ_FRIENDLY_MINION_DIED_THIS_GAME;
    public int REQ_FROZEN_TARGET;
    public int REQ_TARGET_IF_AVAILABLE_AND_NO_3_COST_CARD_IN_DECK;
    public int REQ_CANNOT_PLAY_THIS;
    public int REQ_TARGET_IF_AVAILABLE_AND_DRAGON_IN_HAND;
    public int REQ_TARGET_IF_AVAILABE_AND_ELEMENTAL_PLAYED_LAST_TURN;
}


[System.Serializable]
public class Card
{
    public string artist;
    public int attack;
    public string cardClass;
    public bool collectible;
    public int cost;
    public int dbfId;
    public string flavor;
    public int health;
    public string id;
    public List<string> mechanics;
    public string name;
    public string race;
    public string rarity;
    public string set;
    public string text;
    public string type;
    public PlayRequirements playRequirements;
    public bool elite;
    public List<string> referencedTags;
    public string targetingArrowText;
    public int armor;
    public int overload;
    public int durability;
    public string faction;
    public int spellDamage;
    public string howToEarnGolden;
    public string howToEarn;
    public List<string> entourage;
    public string collectionText;
    public bool hideStats;
    public string questReward;
}

[System.Serializable]
public class CardLibrary
{
    public List<Card> Cards;
}

[System.Serializable]
public class PlayerStats
{

    public int life;
    public string name;
    public int energy;
}

[System.Serializable]
public class PlayerStatsList
{

    public List<PlayerStats> Players;
}


public class DataLoader : MonoBehaviour {

    public CardLibrary myLib;

    public CardLibrary hunterLib = new CardLibrary();
    public CardLibrary mageLib = new CardLibrary();
    public CardLibrary druidLib = new CardLibrary();
    public CardLibrary rogueLib = new CardLibrary();
    public CardLibrary warlockLib = new CardLibrary();
    public CardLibrary priestLib = new CardLibrary();
    public CardLibrary paladinLib = new CardLibrary();
    public CardLibrary shamanLib = new CardLibrary();
    public CardLibrary warriorLib = new CardLibrary();


    public CardLibrary leftoverLib = new CardLibrary();



    // Use this for initialization
    void Start () {


        // loads all cards from the json file
        TextAsset tA = (TextAsset)Resources.Load("cards");
        string jsonString = tA.text;
        myLib = JsonUtility.FromJson<CardLibrary>(jsonString);

        // puts cards in their corresponding classes
        foreach (Card cd in myLib.Cards)
        {
            switch(cd.cardClass)
            {
                case "NEUTRAL":
                    hunterLib.Cards.Add(cd);
                    mageLib.Cards.Add(cd);
                    druidLib.Cards.Add(cd);
                    rogueLib.Cards.Add(cd);
                    warlockLib.Cards.Add(cd);
                    priestLib.Cards.Add(cd);
                    paladinLib.Cards.Add(cd);
                    shamanLib.Cards.Add(cd);
                    warriorLib.Cards.Add(cd);
                break;

                case "HUNTER":
                    hunterLib.Cards.Add(cd);
                break;

                case "MAGE":
                    mageLib.Cards.Add(cd);
                break;

                case "DRUID":
                    druidLib.Cards.Add(cd);
                break;

                case "ROGUE":
                    rogueLib.Cards.Add(cd);
                break;

                case "WARLOCK":
                    warlockLib.Cards.Add(cd);
                break;

                case "PRIEST":
                    priestLib.Cards.Add(cd);
                break;

                case "PALADIN":
                    paladinLib.Cards.Add(cd);
                break;

                case "SHAMAN":
                    shamanLib.Cards.Add(cd);
                break;

                case "WARRIOR":
                    warriorLib.Cards.Add(cd);
                break;

                default:
                    leftoverLib.Cards.Add(cd);
                break;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
