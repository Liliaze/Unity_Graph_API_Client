using UnityEngine;

public class OnSelectVizualise : MonoBehaviour {

	public void OnSelectButton (GameObject canvas)
	{
		if (Graph.singleton != null)
		{
			print("go2");
			Graph.singleton.StartCoroutine("DrawGraph");
			canvas.SetActive(false);
		}
	}
}
