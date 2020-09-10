using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownManagement : MonoBehaviour {

  GameObject selectedKeywords;

  public GameObject keywordTemplate;

  public List<string> keywords;

	// Use this for initialization
	void Start () {
    selectedKeywords = GameObject.Find("ChosenKeywords");
        keywords = new List<string>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void AddToKeywords(Dropdown change)
  {
    GameObject btn = Instantiate(keywordTemplate);
    btn.transform.SetParent(selectedKeywords.transform);
    btn.GetComponentInChildren<Text>().text = gameObject.GetComponent<Dropdown>().options[change.value].text;

        keywords.Add(gameObject.GetComponent<Dropdown>().options[change.value].text);
    //remove the values from the dropdown when they've been added to the list
    gameObject.GetComponent<Dropdown>().options.RemoveAt(change.value);
  }
}
