using UnityEngine;
using System.Collections;

public class onSelectOption : MonoBehaviour {

	public void byCheckingColors (GameObject croix)
	{
		if (croix.activeSelf == true)
		{
			croix.SetActive(false);
			Graph.activeColors = false;
			Graph.singleton.reDrawGraphic();
		}
		else
		{
			croix.SetActive(true);
			Graph.activeColors = true;
			Graph.singleton.reDrawGraphic();
		}
	}

	public void byCheckingUpdate (GameObject croix)
	{
		if (croix.activeSelf == true)
		{
			croix.SetActive(false);
			print("nada");
			StopAllCoroutines();
		}
		else
		{
			croix.SetActive(true);
			StartCoroutine("onSelectButonUpdate");
		}
	}

	private IEnumerator onSelectButonUpdate ()
	{
		while (true)
		{
			yield return Graph.singleton.StartCoroutine("updateList");
			yield return new WaitForSeconds(2);
			Graph.singleton.reDrawGraphic();
		}
	}

	public void closeOptionsOnSelectReturn (GameObject croix)
	{
		croix.SetActive(false);
	}
}
