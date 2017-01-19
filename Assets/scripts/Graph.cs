﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class Graph : MonoBehaviour {

	public static Graph singleton;
	public GameObject prefabTitle = null;
	public GameObject prefab = null;
	//public GameObject prefabBlueBar = null;
	//public GameObject prefabRedBar = null;
	public GameObject parentTools = null;

	private GameObject[] newElemGraph = new GameObject[24]; 

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

	public IEnumerator DrawGraph ()
	{
		yield return StartCoroutine("getSharePriceList");

		SearchMinandMaxYinList();
		DrawTitle();
		DrawFirstGraphic();

		yield return StartCoroutine("updateList");
		yield return new WaitForSeconds(3);
		reDrawGraphic();
		yield return StartCoroutine("updateList");
		yield return new WaitForSeconds(3);
		reDrawGraphic();
		yield return StartCoroutine("updateList");
		yield return new WaitForSeconds(3);
		reDrawGraphic();
	}

	private IEnumerator getSharePriceList ()
	{
		print("Récupération des données du Graph");

		string url = string.Format("{0}?{1}={2}&{3}={4}&{5}={6}", Config.graph_api_base_path + Config.companySharePricePath, Config.paramOneName, Config.companyName, Config.paramTwoDate, Config.date, Config.paramThreeCurrency, Config.currency);

		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();

		sharePriceList = JsonUtility.FromJson<SharePricesM>(www.downloadHandler.text);
	}

	private void DrawTitle ()
	{
		double posX;

		posX = -((Config.companyName.Length / 2) * 0.035);
		GameObject titleCompany = (GameObject)Instantiate(prefabTitle, new Vector3((float)posX, (float)-0.15, (float)1.9), Quaternion.identity);
		titleCompany.GetComponent<TextMesh>().text = Config.companyName;
		titleCompany.transform.parent = parentTools.transform;
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

	private void changeColorPrefab (double amount, GameObject prefab)
	{
		if (previousAmount < amount)
			prefab.GetComponent<Renderer>().material.color = Color.green;
		else if (previousAmount == amount)
			prefab.GetComponent<Renderer>().material.color = Color.blue;
		else
			prefab.GetComponent<Renderer>().material.color = Color.red;
	}

	private void DrawFirstGraphic ()
	{
		double posX = -((24/2)*0.05);
		double coef = yMax / 100;
		int index = 0;

		foreach (CompanySharePriceM value in sharePriceList.sharePrices)
		{
			float yScale = (float)((value.amount / coef) / 300);

			newElemGraph[index] = (GameObject)Instantiate(prefab, new Vector3((float)posX, (float)(-0.2 + yScale), 2), Quaternion.identity);
			changeColorPrefab(value.amount, newElemGraph[index]);
			newElemGraph[index].transform.localScale = new Vector3((float)0.04, yScale, (float)0.04);
			newElemGraph[index].transform.parent = parentTools.transform;

			previousAmount = value.amount;
			posX += 0.05;
			index++;
		}
	}

	private void swapArrayAndInsert (string date, float price)
	{
		for (int i = 0; i < 23; i++)
		{
			sharePriceList.sharePrices[i].time = sharePriceList.sharePrices[i + 1].time;
			sharePriceList.sharePrices[i].amount = sharePriceList.sharePrices[i + 1].amount;
		}
		sharePriceList.sharePrices[23].time = date;
		sharePriceList.sharePrices[23].amount = price;
	}

	/*Bouton à créer voir update permanent si sélectionné, arrêt si décoché
	private void onSelectUpdateGraphic()
	{
		yield return StartCoroutine("updateList");
		yield return new WaitForSeconds(3);
		reDrawGraphic();
	}
	*/

	private IEnumerator updateList ()
	{
		string newDate = null;
		float newPrice = 0;

		//Requête pour récupérer un nouveau montant
		yield return StartCoroutine("getNewPrice");
		newPrice = oneNewPrice.oneNewPrice;

		//Mise à jour de la date correspondante
		newDate = getNewDateAddHours(sharePriceList.sharePrices[sharePriceList.sharePrices.Length - 1].time);

		//Mise à jour du tableau de valeur et insertion des nouvelles données
		swapArrayAndInsert(newDate, newPrice);
		SearchMinandMaxYinList();
		yield return null;
	}

	private IEnumerator getNewPrice ()
	{
		string url = string.Format("{0}?{1}={2}", Config.graph_api_base_path + Config.oneNewPricePath, Config.paramOneName, Config.companyName);

		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();

		oneNewPrice = JsonUtility.FromJson<OneNewPriceM>(www.downloadHandler.text);
		yield return null;
	}

	private string getNewDateAddHours (string date)
	{
		string newDate = null;

		//print("date à convertir" + date);
		DateTime newHours = Convert.ToDateTime(date);
		//print("date convertit :" + newHours);
		newHours.AddHours(10);
		newDate = newHours.ToString("yyyy-MM-dd hh:mm:ss");
		//print("newHours + 1 :" + newDate);
		return (newDate);
	}

	private void reDrawGraphic ()
	{
		double coef = yMax / 100;
		float yScale = 0;
		int index = 0;

		foreach (CompanySharePriceM value in sharePriceList.sharePrices)
		{
			yScale = (float)((value.amount / coef) / 300);

			changeColorPrefab(value.amount, newElemGraph[index]);
			newElemGraph[index].transform.localPosition = new Vector3(newElemGraph[index].transform.localPosition.x, (float)(-0.2 + yScale), newElemGraph[index].transform.localPosition.z);
			newElemGraph[index].transform.localScale = new Vector3((float)0.04, yScale, (float)0.04);

			previousAmount = value.amount;
			index++;
		}
	}

	/*
		print ("---------IMPRESSION LISTE DES PRICES -----" + sharePriceList.sharePrices.Length);
		foreach (CompanySharePriceM value in sharePriceList.sharePrices)
			print ("time = " + value.time + "amount = " + value.amount);
	*/


	/*
 * A compléter et revoir : 
private void ZoomInOneHour (double Hour)
{
	double posX = -((60/2)*0.012);

	for (int i = 0; i < 60; i++)
	{
		GameObject newElemGraph = (GameObject)Instantiate(prefabBar, new Vector3((float)posX, (float)(-0.2 + (value.amount/100)), 2), Quaternion.identity);
		newElemGraph.transform.localScale = new Vector3((float)0.01, value.amount / 100, (float)0.01);
		newElemGraph.transform.parent = parentTools.transform;

		posX += 0.012;
	}
}
*/
}

