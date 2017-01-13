using System;

[Serializable]
public class CompanySharePriceM
{
	public string time;
	public float amount;
}

[Serializable]
public class SharePricesM
{
	public CompanySharePriceM[] sharePrices;
}
