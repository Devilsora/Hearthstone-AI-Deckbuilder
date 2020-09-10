
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.IO;
using UnityEngine;

public class Deck : MonoBehaviour {

    // cardList 
    public List<Card> cards;

    // dataloader ref
    GameObject dataLoader;

    // entire card collection
    public CardLibrary lib; 

    // class and keyword vars
    public string class_;
    public List<string> keywords_;

    // card pools to add cards from
    public List<Card> prefPool;
    public List<Card> otherPool;

    // keyword (mechanic cards) count
    public Dictionary<string, int> mechDict;

    // how many keyword cards out of 30 (preferred)
    const int mechThreshold = 18;

    // mana count
    public Dictionary<int, int> manaDict;

    // target mana curve
    public Dictionary<int, int> targetCurve;

    // approxiamtion threshold
    const int costThreshold = 1;

    // deckstring to be used in hs
    public string DeckCode;

    // minimum acceptable card rating
    const int minRating = 10;

    public int CompareData(Card c1, Card c2)
    {
      //first sort by mana
      int card1_mana = c1.cost;
      int card2_mana = c2.cost;
      //comes before
      if (card1_mana.CompareTo(card2_mana) != 0)
        return card1_mana.CompareTo(card2_mana);
      else //same mana cost, sort by name
      {
        string opt1_name = c1.name;
        string opt2_name = c2.name;

        return opt1_name.CompareTo(opt2_name);
      }
    }

    // Use this for initialization
    void Start ()
    {
        // initializes variables
        cards = new List<Card>();	
        prefPool = new List<Card>();	
        otherPool = new List<Card>();
        dataLoader = GameObject.Find("DataHandler");
        mechDict = new Dictionary<string, int>();
        manaDict = new Dictionary<int, int>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Dictionary<int, int> SimpleBellCurve()
    {
        Dictionary<int, int> bell = new Dictionary<int, int>();

        bell.Add(0, 0);
        bell.Add(1, 3);
        bell.Add(2, 9);
        bell.Add(3, 7);
        bell.Add(4, 5);
        bell.Add(5, 3);
        bell.Add(6, 2);
        bell.Add(7, 1);
        bell.Add(8, 1);
        bell.Add(9, 1);
        bell.Add(10, 1);

        return bell;
    }

    public void Configure(string cardClass, List<string> keywords)
    {
        // clear current deck
        if(cards.Count() > 0)
        {
            cards.Clear();
            keywords_.Clear();
            prefPool.Clear();
            otherPool.Clear();
            mechDict.Clear();
            manaDict.Clear();
        }



        class_ = cardClass.ToUpper();

        Debug.Log("CONFIGURE CALLED");
        Debug.Log(class_);

        foreach (string kw in keywords)
        {
            keywords_.Add(kw.ToUpper());
  
        }

        foreach (string k in keywords_)
        {
            mechDict.Add(k, 0);
            Debug.Log(k);
        }

        for (int i = 0; i < 11; i++)
          manaDict.Add(i, 0);



        switch (class_)
        {
            case "HUNTER":
                lib = dataLoader.GetComponent<DataLoader>().hunterLib;
            break;

            case "PALADIN":
                lib = dataLoader.GetComponent<DataLoader>().paladinLib;
            break;

            case "WARLOCK":
                lib = dataLoader.GetComponent<DataLoader>().warlockLib;
            break;

            case "WARRIOR":
                lib = dataLoader.GetComponent<DataLoader>().warriorLib;
            break;

            case "PRIEST":
                lib = dataLoader.GetComponent<DataLoader>().priestLib;
            break;

            case "MAGE":
                lib = dataLoader.GetComponent<DataLoader>().mageLib;
            break;

            case "SHAMAN":
                lib = dataLoader.GetComponent<DataLoader>().shamanLib;
            break;

            case "ROGUE":
                lib = dataLoader.GetComponent<DataLoader>().rogueLib;
            break;

            case "DRUID":
                lib = dataLoader.GetComponent<DataLoader>().druidLib;
            break;

            default:
                Debug.Log("DECKBUILDER: CLASS ERROR");
            break;
        }

        bool matches;

        //debug - mana

        foreach (Card cd in lib.Cards)
        {
            if (!manaDict.ContainsKey(cd.cost))
                Debug.Log(cd.cost);
        }


        foreach (Card cd in lib.Cards)
        {
            matches = false;

            foreach(string mech in cd.mechanics)
            {
                if (keywords_.Contains(mech))
                {
                    prefPool.Add(cd);

                    if (cd.rarity != "LEGENDARY")
                    {
                        prefPool.Add(cd);
                    }

                    matches = true;
                    break;
                }
            }

            if (!matches)
            {
                otherPool.Add(cd);

                if (cd.rarity != "LEGENDARY")
                {
                    otherPool.Add(cd);
                }
            }

        }

        targetCurve = SimpleBellCurve();


        Build();


        // deck file writing, might remove later
        string path = "Assets/Resources/Deck.txt";

        StreamWriter wrt = new StreamWriter(path, true);


        wrt.WriteLine("Cards: ");

        foreach(Card cd in cards)
        {
            wrt.WriteLine(cd.name);
        }

        wrt.WriteLine(" ");

        wrt.WriteLine("Mana Curve");

        for (int i = 0; i < manaDict.Count(); ++i)
        {
            string temp = i.ToString() + ": " + manaDict[i].ToString();
            wrt.WriteLine(temp);
        }

        wrt.Close();

        // DECKSTRING BUILDING WOULD GO HERE, USE the List<Card> cards variable to get all cards



    }

    float Rate(Card cd)
    {
        float baseR = ((float)cd.attack + (float)cd.health) / ((float)cd.cost + (float)cd.cost + 1.0f) * 50.0f;

        if (cd.attack == 0 && cd.health == 0)
        {
            baseR = 50.0f;
        }

        float prefR = 0.0f;

        bool isPref = false;

        foreach (string mech in cd.mechanics)
        {
            if (keywords_.Contains(mech))
            {
                isPref = true;
                break;
            }
        }

        if (isPref)
        {
            prefR = 100.0f;
        }

        // mana rating
        float manaR = 0.0f;

        int cost = cd.cost;

        if (cost > 10)
          cost = 10;

        // if we already have enough cards at this manacost
        if (manaDict[cost] > (targetCurve[cost] + costThreshold))
        {
          // unplayable
          manaR = -1000.0f;
        }

        // returns added ratings
        return baseR + manaR;
    }

    // adds a card to the deck
    void AddCard(Card cd)
    {
      cards.Add(cd);
      manaDict[cd.cost]++;

      foreach (string mech in cd.mechanics)
        if (keywords_.Contains(mech))
          mechDict[mech]++;
    }

    void Build()
    {
        Card c1, c2, c3;
        float r1, r2, r3;

        

        List<Card> pool;

        while (cards.Count() < 30)
        {

            if (prefPool.Count() > 3 && (cards.Count() < mechThreshold + 1))
            {
                pool = prefPool;
            }
            else
            {
                pool = otherPool;
            }


            c1 = pool[UnityEngine.Random.Range(0, pool.Count())];

            pool.Remove(c1);

            c2 = pool[UnityEngine.Random.Range(0, pool.Count())];

            pool.Remove(c2);

            c3 = pool[UnityEngine.Random.Range(0, pool.Count())];

            pool.Remove(c3);

            r1 = Rate(c1);
            r2 = Rate(c2);
            r3 = Rate(c3);
            
            if (r1 <= minRating && r2 <= minRating && r3 <= minRating)
            {
                continue;
            }
            else if (r1 >= r2 && r1 >= r3)
            {
                AddCard(c1);
                pool.Add(c2);
                prefPool.Add(c3);
            }
            else if (r2 >= r1 && r2 >= r3)
            {
                AddCard(c2);
                pool.Add(c1);
                pool.Add(c3);
            }
            else if (r3 >= r1 && r3 >= r2)
            {
                AddCard(c3);
                pool.Add(c1);
                pool.Add(c2);
            }
        }

        cards.Sort(CompareData);
        DeckCode = GenerateDeckCode();
    }

  public string GenerateDeckCode()
  {
    MemoryStream allMem = new MemoryStream();

    string deckCode = "";
    int classID = 0;

    writeVarInt(allMem, 0); //first one is always zero
    writeVarInt(allMem, 1); //encode version #
    writeVarInt(allMem, 2); //if wild decks get enabled in future, this could be 1 for wild. but standard only, so 2
    writeVarInt(allMem, 1); //number of heroes in hero array, shoudl always be 1

    switch(class_)
    {
      case "HUNTER":
        classID = 31;
        break;

      case "MAGE":
        classID = 637;
        break;

      case "DRUID":
        classID = 274;
        break;

      case "ROGUE":
        classID = 930;
        break;

      case "WARLOCK":
        classID = 893;
        break;

      case "PRIEST":
        classID = 813;
        break;

      case "PALADIN":
        classID = 671;
        break;

      case "SHAMAN":
        classID = 1066;
        break;

      case "WARRIOR":
        classID = 7;
        break;
    }
    writeVarInt(allMem, classID); //class ID


    //seperate the cards into lists of duplicates and singletons
    List<Card> singletons = new List<Card>();
    List<Card> dupes = new List<Card>();
    for(int i = 0; i < 30; i++)
    {
      bool is_duplicate = false;
      //get the current card
      Card current_checkForDupes = cards[i];
      
      for(int j = i + 1; j < 30; j++)
      {
        Card comparator = cards[j];

        if (comparator.name == current_checkForDupes.name)
        {
          is_duplicate = true;
          Debug.Log("Breaking out from loop since cards had the same name");
          break;
        }
          
      }

      if(is_duplicate)
      {
        dupes.Add(current_checkForDupes);
      }
      else
      {
        singletons.Add(current_checkForDupes);
      }
    }
    //string singleTons_str = "";
    //string dupes_str = "";

    for (int i = 0; i < dupes.Count; i++)
    {
      for (int j = 0; j < singletons.Count; j++)
      {
        if (dupes[i] == singletons[j])
        {
          singletons.Remove(singletons[j]);
          if (i > 0)
            i--;
          else
            i = 0;
        }

      }
    }

   //for (int i = 0; i < singletons.Count; i++)
   //{
   //  singleTons_str += singletons[i].name + ", ";
   //}
   //
   //Debug.Log("Singletons: " + singleTons_str + "       Count: " + singletons.Count);
   //
   //
   //for (int i = 0; i < dupes.Count(); i++)
   //{
   //  dupes_str += dupes[i].name + "  ";
   //}
   //Debug.Log("Dupes: " + dupes_str + "       Count: " + dupes.Count);

    


    //write them to the memory
    writeVarInt(allMem, singletons.Count);

    for(int i = 0; i < singletons.Count; i++)
    {
      writeVarInt(allMem, singletons[i].dbfId);
    }

    //get the number of duplicates in the deck
    writeVarInt(allMem, dupes.Count);
    //write them to the memory
    for (int i = 0; i < dupes.Count; i++)
    {
      writeVarInt(allMem, dupes[i].dbfId);
    }

    //get the number of cards that have 2+ in the deck [impossible in standard deck building]
    writeVarInt(allMem, 0);

    if (allMem != null)
      allMem.Close();
    //seperate deck in single copy and double copy groups
    deckCode = Convert.ToBase64String(allMem.ToArray());
    return deckCode;
  }
  public static void writeVarInt(MemoryStream mem, int value)
  {
    if (value == 0)
      mem.WriteByte(0);
    else
    {
      var bytes = GetBytes((ulong)value);
      mem.Write(bytes, 0, bytes.Length);
    }
  }

  public static byte[] GetBytes(ulong value)
  {
    MemoryStream ms = new MemoryStream();

    while(value != 0)
    {
      var b = value & 0x7f;
      value >>= 7;

      if (value != 0)
        b |= 0x80;

      ms.WriteByte((byte)b);

    }

    return ms.ToArray();
  }
}
