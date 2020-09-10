using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckGeneration : MonoBehaviour {

	// Use this for initialization

  

  public enum CARDCLASS
  {
    WARRIOR = 0,
    SHAMAN = 1,
    ROGUE = 2,
    PALADIN = 3,
    HUNTER = 4,
    DRUID = 5,
    WARLOCK = 6,
    MAGE = 7,
    PRIEST = 8
  }

  public CARDCLASS availableCardPool;
  public string[] requestedKeywords;

  

	void Start () {
		
    //load the appropriate card pool based on the chosen cardclass;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
