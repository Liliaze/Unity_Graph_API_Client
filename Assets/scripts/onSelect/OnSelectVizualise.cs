using UnityEngine;

public class OnSelectVizualise : MonoBehaviour {

	public void OnSelectButton (GameObject canvasList)
	{
		if (Graph.singleton != null)
		{
			Graph.singleton.StartCoroutine("DrawGraph");
			canvasList.SetActive(false);
		}
	}

	public void OnSelectButtonNext (GameObject canvasOption)
	{
		canvasOption.SetActive(true);
	}
}
