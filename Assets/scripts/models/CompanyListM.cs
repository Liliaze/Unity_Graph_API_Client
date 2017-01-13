using System;

[Serializable]
public class CompanyInfosM
{
	public string name;
	public string businessCountry;
	public int naeCode;
}

[Serializable]
public class CompanyListM
{
	public CompanyInfosM[] companyList;
}
