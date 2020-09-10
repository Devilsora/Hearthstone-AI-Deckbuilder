using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassButtonLogic : MonoBehaviour {

  public GameObject[] buttons;

  public string chosenClass;
  
  // Use this for initialization
	void Start () {
        chosenClass = "None";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void ClickedClass()
  {
    Debug.Log("In clicked class - gameobject is " + gameObject.name);

    GameObject.Find("HeroSelectors").GetComponent<ClassButtonLogic>().chosenClass = gameObject.name;

    for(int i = 0; i < buttons.Length; i++)
    {
      //darken the classes not picked
      if (buttons[i] != gameObject)
      {
        buttons[i].GetComponent<Image>().color = new Color(45f/255f, 45f / 255f, 45f / 255f, 1f);
      }
      else
      {
        gameObject.GetComponent<Image>().color = Color.white;
        //GameObject.Find("GenerateDeckCodeButton").GetComponent<DeckGeneration>().availableCardPool = (DeckGeneration.CARDCLASS)i;
      }
    }
  }
}
