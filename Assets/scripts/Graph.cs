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

	private double yMin = 0;
	private double yMax = 0;

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

	private void SearchMinandMaxY (double amount)
	{
		if (amount < yMin)
			yMin = amount;
		else if (amount > yMax)
			yMax = amount;
	}

	private void SearchMinandMaxYinList ()
	{
		foreach (CompanySharePriceM value in sharePriceList.sharePrices)
			SearchMinandMaxY(value.amount);
	}

	private void DrawTitle ()
	{
		double posX;

		posX = -((Config.companyName.Length / 2) * 0.035); 
		GameObject titleCompany = (GameObject)Instantiate(prefabTitle, new Vector3((float)posX, (float)-0.15, (float)1.9), Quaternion.identity);
		titleCompany.GetComponent<TextMesh>().text = Config.companyName;
	}

	private void DrawFirstGraphic ()
	{
		double posX = -((24/2)*0.06);

		foreach (CompanySharePriceM value in sharePriceList.sharePrices)
		{
			GameObject newElemGraph = (GameObject)Instantiate(prefabBar, new Vector3((float)posX, (float)(-0.2 + (value.amount/100)), 2), Quaternion.identity);
			newElemGraph.transform.localScale = new Vector3((float)0.05, value.amount / 100, (float)0.05);
			newElemGraph.transform.parent = parentTools.transform;

			//print("heure =" + value.time + ", montant = " + value.amount + " " + Config.currency);
			posX += 0.06;
		}
	}

	private void ZoomInOneHour (double Hour)
	{
		//A compléter et revoir :
		/*
		double posX = -((60/2)*0.012);

		for (int i = 0; i < 60; i++)
		{
			GameObject newElemGraph = (GameObject)Instantiate(prefabBar, new Vector3((float)posX, (float)(-0.2 + (value.amount/100)), 2), Quaternion.identity);
			newElemGraph.transform.localScale = new Vector3((float)0.01, value.amount / 100, (float)0.01);
			newElemGraph.transform.parent = parentTools.transform;

			posX += 0.012;
		}
		*/
	}

	public IEnumerator DrawGraph ()
	{
		print("DrawGraph");
		yield return StartCoroutine("getSharePriceList");

		SearchMinandMaxYinList();
		print("yMin = " + yMin + ", yMax = " + yMax);
		DrawTitle();
		DrawFirstGraphic();

		print("list = " + sharePriceList);
		print ("Lancement du Graph");
	}
}
