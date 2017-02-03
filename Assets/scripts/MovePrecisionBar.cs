using UnityEngine;
using System.Collections;

public class MovePrecisionBar : MonoBehaviour
{

	// SCRIPT NON FINI EN COURS DE REDACTION
	void Update ()
	{
		var headPosition = Camera.main.transform.position;
		var gazeDirection = Camera.main.transform.forward;

		if (this.isActiveAndEnabled)
		{
			//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(headPosition, gazeDirection, out hit, 3))
			{
				gameObject.transform.position = new Vector3(gameObject.transform.position.x, hit.transform.position.y, gameObject.transform.position.z);
			}
			print("gameObject pos = " + gameObject.transform.position + ",hit pos : " + hit.transform.position);
		}
	}
}