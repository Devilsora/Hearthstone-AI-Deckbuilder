using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddBackToList : MonoBehaviour {

  GameObject ChooseKeywordsDropdown;

	// Use this for initialization
	void Start () {
    ChooseKeywordsDropdown = GameObject.Find("KeywordSelector");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void AddBack()
  {
    Dropdown.OptionData newData = new Dropdown.OptionData();
    newData.text = gameObject.GetComponentInChildren<Text>().text;
    ChooseKeywordsDropdown.GetComponentInChildren<Dropdown>().options.Add(newData);

    ChooseKeywordsDropdown.GetComponentInChildren<DropdownManagement>().keywords.Remove(newData.text);

    //sort back to alphabetical order
    //SortDropdown(ChooseKeywordsDropdown.GetComponentInChildren<Dropdown>());
        ChooseKeywordsDropdown.GetComponentInChildren<Dropdown>().options.Sort(CompareData);
    
    Destroy(gameObject);
  }

  
  public int CompareData(Dropdown.OptionData opt1, Dropdown.OptionData opt2)
  {
    string opt1_name = opt1.text;
    string opt2_name = opt2.text;

    return opt1_name.CompareTo(opt2_name);

  }


  public void SortDropdown(Dropdown dd)
  {
    Dropdown.OptionData[] data = dd.options.ToArray();

    for(int i = 0; i < data.Length; i++)
    {

    }
  }
}
