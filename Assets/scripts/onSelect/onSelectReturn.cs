using UnityEngine;
using System.Collections;

public class onSelectReturn : MonoBehaviour {

	public void OnSelectReturnFirst (GameObject canvasOption)
	{
		canvasOption.SetActive(false);
	}

	public void OnSelectReturnSecond (GameObject canvasList)
	{
		canvasList.SetActive(true);
	}

	/*
	* fonction pour supprimer les 24 prefabsBar et le Title enfants du dossier parentTools
	*/
	public void DestroyGraphic (GameObject parentTools)
	{
		GameObject child = null;

		for (int i = 1; i <= 25; i++)
		{
			child = parentTools.transform.GetChild(i).gameObject;
			Destroy(child);
		}
	}
}
