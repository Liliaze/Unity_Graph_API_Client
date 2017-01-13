using System;

public static class Config
{
	public static string graph_api_base_path = "https://graph-api-liliaze.c9users.io/";
	public static string companyListPath = "companyList";
	public static string companySharePricePath = "companySharePrice";

	public static string paramOneName = "companyName";
	public static string paramTwoDate = "date";
	public static string paramThreeCurrency = "currencyName";


	public static string companyName = "";
	public static char currency = '€';
	public static string date = DateTime.Now.ToString("yyyy-MM-dd");
}
