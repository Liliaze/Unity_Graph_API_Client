using UnityEngine;
using System.Collections;

public class onSelectOption : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnSelectColors (GameObject croix)
	{
		if (croix.activeSelf == true)
			croix.SetActive(false);
		else
			croix.SetActive(true);
	}
}
