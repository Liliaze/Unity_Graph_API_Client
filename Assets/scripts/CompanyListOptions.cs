using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class CompanyListOptions : MonoBehaviour
{
	private Dropdown dropDown;
	private List<string> options;
	private CompanyListM companyList;

	//public GameObject prefabDebug = null;

	private IEnumerator Start ()
	{
		dropDown = GetComponent<Dropdown>();
		dropDown.ClearOptions();

		//WWW www = new WWW(Config.graph_api_base_path + Config.companyListPath);
		UnityWebRequest www = UnityWebRequest.Get(Config.graph_api_base_path + Config.companyListPath);

		yield return www.Send();
		//print(www.text);
		print(www.url);
		print(www.downloadHandler.text);

		/*
		//Impression pour débugger dans le casque//
		GameObject titleDebug = (GameObject)Instantiate(prefabDebug, new Vector3((float)-0.3, (float)0.2, (float)1.9), Quaternion.identity);
		titleDebug.GetComponent<TextMesh>().text = www.text;
		GameObject titleError = (GameObject)Instantiate(prefabDebug, new Vector3((float)-0.3, (float)-0.2, (float)1.9), Quaternion.identity);
		titleDebug.GetComponent<TextMesh>().text = www.error;
		GameObject titleTest = (GameObject)Instantiate(prefabDebug, new Vector3((float)-0.3, (float)0, (float)1.9), Quaternion.identity);
		titleDebug.GetComponent<TextMesh>().text = "pattate";
		*/

		//companyList = JsonUtility.FromJson<CompanyListM>(www.text);
		companyList = JsonUtility.FromJson<CompanyListM>(www.downloadHandler.text);
		options = new List<string>();

		foreach (CompanyInfosM company in companyList.companyList)
		{
			options.Add(company.name);
		}

		dropDown.AddOptions(options);
	}

	public void OnSelectName (Text t)
	{
		//print(t.text);
		Config.companyName = t.text;
		print(Config.companyName);
	}
}
