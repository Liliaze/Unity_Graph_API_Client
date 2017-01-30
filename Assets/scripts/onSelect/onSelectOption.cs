using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class onSelectOption : MonoBehaviour {

	public void byCheckingColors (GameObject croix)
	{
		if (croix.activeSelf == true)
		{
			Graph.activeColors = false;
			croix.SetActive(false);
		}
		else
		{
			Graph.activeColors = true;
			croix.SetActive(true);
		}
		Graph.singleton.reDrawGraphic();
	}

	public void byCheckingUpdate (GameObject croix)
	{
		if (croix.activeSelf == true)
		{
			StopAllCoroutines();
			croix.SetActive(false);
		}
		else
		{
			StartCoroutine("onSelectButonUpdate");
			croix.SetActive(true);
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
