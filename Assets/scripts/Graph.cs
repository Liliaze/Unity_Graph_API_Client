using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class Graph : MonoBehaviour {

	public static Graph singleton;
	public GameObject prefabTitle = null;
	public GameObject prefabGreenBar = null;
	public GameObject prefabBlueBar = null;
	public GameObject prefabRedBar = null;
	public GameObject parentTools = null;

	private double yMin = 0;
	private double yMax = 0;
	private double previousAmount = 0;

	private static SharePricesM sharePriceList;
	private static OneNewPriceM oneNewPrice;

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
		//print(url);

		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();

		print(www.downloadHandler.text);

		sharePriceList = JsonUtility.FromJson<SharePricesM>(www.downloadHandler.text);
	}

	private void swapArrayAndInsert (string time, float amount)
	{
		/*
		print("---------BEFORE-----" + sharePriceList.sharePrices.Length);
		foreach (CompanySharePriceM value in sharePriceList.sharePrices)
		{
			print("time = " + value.time + "amount = " + value.amount);
		}
		*/
		for (int i = 0; i < 23; i++)
		{
			sharePriceList.sharePrices[i] = sharePriceList.sharePrices[i + 1];
		}
		sharePriceList.sharePrices[23].time = time;
		sharePriceList.sharePrices[23].amount = amount;
		/*
		print("---------AFTER-----" + sharePriceList.sharePrices.Length);
		foreach (CompanySharePriceM value in sharePriceList.sharePrices)
		{
			print("time = " + value.time + "amount = " + value.amount);
		}
		*/
	}

	private IEnumerator updateList ()
	{
		//CompanySharePriceM newElemList = null;
		DateTime newHours = Convert.ToDateTime(sharePriceList.sharePrices[sharePriceList.sharePrices.Length - 1].time);
		print("newHours:" + newHours);
		newHours.AddHours(1);
		print("newHours + 1 :" + newHours);
		string tmp = newHours.ToString("yyyy-MM-dd hh:mm:ss");

		print("Changement des données du Graph");

		string url = string.Format("{0}?{1}={2}", Config.graph_api_base_path + Config.oneNewPricePath, Config.paramOneName, Config.companyName);
		print(url);
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();
		print(www.downloadHandler.text);
		oneNewPrice = JsonUtility.FromJson<OneNewPriceM>(www.downloadHandler.text);
		swapArrayAndInsert(tmp, oneNewPrice.oneNewPrice);
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

	private GameObject chooseBarPrefab (double amount)
	{
		if (previousAmount < amount)
			return (prefabGreenBar);
		else if (previousAmount == amount)
			return (prefabBlueBar);
		else
			return (prefabRedBar);
	}

	private void DrawGraphic ()
	{
		double posX = -((24/2)*0.06);
		double coef = yMax / 100;
		GameObject prefab = null;

		foreach (CompanySharePriceM value in sharePriceList.sharePrices)
		{
			float yScale = (float)((value.amount / coef) / 300);
			prefab = chooseBarPrefab(value.amount);
			GameObject newElemGraph = (GameObject)Instantiate(prefab, new Vector3((float)posX, (float)(-0.2 + yScale), 2), Quaternion.identity);
			newElemGraph.transform.localScale = new Vector3((float)0.05, yScale , (float)0.05);
			newElemGraph.transform.parent = parentTools.transform;

			//print("heure =" + value.time + ", montant = " + value.amount + " " + Config.currency);
			previousAmount = value.amount;
			posX += 0.06;
		}
	}

	private void DestroyGraphic ()
	{
		GameObject child = null;
		for (int i = 1; i <= 24; i++)
		{
			child = parentTools.transform.GetChild(i).gameObject;
			Destroy(child);
		}
		//yield return ("");
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
		DrawTitle();
		DrawGraphic();

		yield return StartCoroutine("DestroyGraphic");
		yield return StartCoroutine("updateList");
		DrawGraphic();
	}
}
