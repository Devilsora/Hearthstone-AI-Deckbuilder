using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class GenerateButton : MonoBehaviour {

    GameObject heroSelect;
    GameObject dm;
    GameObject deck;
    GameObject codeHolder;

    // Use this for initialization
    void Start () {
        heroSelect = GameObject.Find("HeroSelectors");
        dm = GameObject.Find("KeywordSelector");
        deck = GameObject.Find("Deck");
        codeHolder = GameObject.Find("DeckCodeBox");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickedGenerator()
    {
        // kickstarts the deck building
        Debug.Log("generator clicked");
        deck.GetComponent<Deck>().Configure(heroSelect.GetComponent<ClassButtonLogic>().chosenClass, dm.GetComponentInChildren<DropdownManagement>().keywords);
        codeHolder.GetComponent<Text>().text = deck.GetComponent<Deck>().DeckCode;

        //copy the text onto the users' clipboard
        TextEditor te = new TextEditor();
        te.text = codeHolder.GetComponent<Text>().text;
        te.SelectAll();
        te.Copy();
    }
}
