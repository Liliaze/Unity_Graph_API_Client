using UnityEngine;
using System.Collections;

public class OnSelectDetails : MonoBehaviour {

	public void byCheckingDetails (GameObject repere)
	{
		if (repere.activeSelf == true)
		{
			repere.SetActive(false);
			Graph.activeDetails = false;
			Graph.singleton.reDrawGraphic();
		}
		else
		{
			repere.SetActive(true);
			Graph.activeDetails = true;
			Graph.singleton.reDrawGraphic();
		}
	}

	public void displayCurrency (GameObject textCurrency)
	{
		textCurrency.GetComponent<TextMesh>().text = "(" + Config.currency + ")";
	}


}
