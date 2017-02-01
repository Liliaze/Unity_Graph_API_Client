using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class Graph : MonoBehaviour {

	public static Graph singleton;
	public GameObject prefabTitle = null;
	public GameObject prefab = null;
	public GameObject parentTools = null;

	public static bool activeColors = false;
	public static bool activeDetails = false;

	private GameObject[] newElemGraph = new GameObject[24]; 

	private float yMin = 0;
	private float yMax = 0;
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

		posX = -((Config.companyName.Length / 2) * 0.01);
		GameObject titleCompany = (GameObject)Instantiate(prefabTitle, new Vector3((float)posX, (float)-0.21, (float)1.9), Quaternion.identity);
		titleCompany.GetComponent<TextMesh>().text = Config.companyName;
		titleCompany.transform.parent = parentTools.transform;
	}

	private void SearchMinandMaxY (float amount)
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
		if (activeColors)
		{
			if (previousAmount < amount)
				prefab.GetComponent<Renderer>().material.color = Color.green;
			else if (previousAmount == amount)
				prefab.GetComponent<Renderer>().material.color = Color.blue;
			else
				prefab.GetComponent<Renderer>().material.color = Color.red;
		}
		else
			prefab.GetComponent<Renderer>().material.color = Color.yellow;
	}

	private void DrawFirstGraphic ()
	{
		float sizeXY = 0.02f;
		float posX = parentTools.transform.position.x -((24.0f / 2.0f) * sizeXY);
		float posZ = parentTools.transform.position.z;
		float posY = parentTools.transform.position.y;
		float coef = yMax / 100.0f;
		int index = 0;

		foreach (CompanySharePriceM value in sharePriceList.sharePrices)
		{
			float yScale = (float)((value.amount / coef) / 600);


			newElemGraph[index] = (GameObject)Instantiate(prefab, new Vector3(posX, (posY + yScale), posZ), Quaternion.identity);
			changeColorPrefab(value.amount, newElemGraph[index]);
			newElemGraph[index].transform.localScale = new Vector3(sizeXY, yScale, sizeXY);
			newElemGraph[index].transform.parent = parentTools.transform;
			displayAmountDetails(newElemGraph[index], value.amount);

			index++;
			previousAmount = value.amount;
			posX += sizeXY * 1.25f;
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

	public void reDrawGraphic ()
	{
		float sizeXY = (float)0.02;
		double coef = yMax / 100;
		float yScale = 0;
		int index = 0;

		foreach (CompanySharePriceM value in sharePriceList.sharePrices)
		{
			yScale = (float)((value.amount / coef) / 600);

			changeColorPrefab(value.amount, newElemGraph[index]);
			newElemGraph[index].transform.localPosition = new Vector3(newElemGraph[index].transform.localPosition.x, yScale, newElemGraph[index].transform.localPosition.z);
			newElemGraph[index].transform.localScale = new Vector3(sizeXY, yScale, sizeXY);
			displayAmountDetails(newElemGraph[index], value.amount);
			previousAmount = value.amount;
			index++;
		}
	}

	private void displayAmountDetails (GameObject elem, float amount)
	{
		if (activeDetails)
		{
			elem.transform.GetChild(0).GetComponent<TextMesh>().text = Math.Round(amount, 0).ToString();
			//A FAIRE !!!!!!!!!!!!!!
			//ajouter ligne de code pour aligner le display des currency.
		}

		else
		{
			elem.transform.GetChild(0).GetComponent<TextMesh>().text = "";
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

