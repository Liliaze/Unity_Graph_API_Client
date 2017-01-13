using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Graph : MonoBehaviour {

	public static Graph singleton;
	public GameObject prefabTitle = null;
	public GameObject prefabBar = null;
	public GameObject parentTools = null;

	private static SharePricesM sharePriceList;

	// Use this for initialization
	void Start () {
		if (singleton != null)
		{
			Destroy(singleton);
		}
		singleton = this as Graph;
	}

	private IEnumerator getSharePriceList ()
	{
		print("Récupération des données du Graph");

		string url = string.Format("{0}?{1}={2}&{3}={4}&{5}={6}", Config.graph_api_base_path + Config.companySharePricePath, Config.paramOneName, Config.companyName, Config.paramTwoDate, Config.date, Config.paramThreeCurrency, Config.currency);
		print(url);

		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();

		print(www.downloadHandler.text);

		sharePriceList = JsonUtility.FromJson<SharePricesM>(www.downloadHandler.text);
	}

	public IEnumerator DrawGraph ()
	{
		print("DrawGraph");
		yield return StartCoroutine("getSharePriceList");
		
		GameObject titleCompany = (GameObject)Instantiate(prefabTitle, new Vector3((float)-0.3, (float)0.2, (float)1.9), Quaternion.identity);
		titleCompany.GetComponent<TextMesh>().text = Config.companyName;
		
		double posX = -0.7;

		foreach (CompanySharePriceM value in sharePriceList.sharePrices)
		{
			GameObject newElemGraph = (GameObject)Instantiate(prefabBar, new Vector3((float)posX, (float)(-0.2 + (value.amount/100)), 2), Quaternion.identity);
			newElemGraph.transform.localScale = new Vector3((float)0.05, value.amount/100, (float)0.05);
			newElemGraph.transform.parent = parentTools.transform;

			//print("heure =" + value.time + ", montant = " + value.amount + " " + Config.currency);
			posX += 0.07;
		}

		print("list = " + sharePriceList);
		print ("Lancement du Graph");
	}
}
