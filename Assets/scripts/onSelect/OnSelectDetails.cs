using UnityEngine;
using System.Collections;

public class OnSelectDetails : MonoBehaviour {
	/*
	public void byChecking (GameObject croix)
	{
		if (croix.activeSelf == true)
			croix.SetActive(false);
		else
			croix.SetActive(true);
	}
	*/
	public void byCheckingDetails (GameObject repere)
	{
		if (repere.activeSelf == true)
		{
			repere.SetActive(false);
			Graph.activeDetails = false;
		}
		else
		{
			repere.SetActive(true);
			Graph.activeDetails = true;
		}
		Graph.singleton.reDrawGraphic();
	}

	public void displayCurrency (GameObject textCurrency)
	{
		textCurrency.GetComponent<TextMesh>().text = "(" + Config.currency + ")";
	}
}
