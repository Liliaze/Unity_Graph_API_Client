using UnityEngine;
using System.Collections;

public class MovePrecisionBar : MonoBehaviour {

	public GameObject cursorInWorld = null;
	public GameObject prefabTypeOfGraph = null;
	private float CursorPosX = 0;
	private float CursorPosY = 0; 


	// SCRIPT NON FINI EN COURS DE REDACTION
	void Update () {
		if (this.isActiveAndEnabled)
		{
			CursorPosX = cursorInWorld.transform.position.x;
			CursorPosY = cursorInWorld.transform.position.y;
			Ray ray = Camera.main.ScreenPointToRay(new Vector2(CursorPosX, CursorPosY));
			RaycastHit hit;
			print("update moveBar");
			if (Physics.Raycast(ray, out hit, 300))
			{
				print(hit.GetType());
				if (hit.GetType() == prefabTypeOfGraph.GetType())
				{
					this.gameObject.transform.position = hit.transform.position;
					print("hello");
				}
			}
		}
	}


}
