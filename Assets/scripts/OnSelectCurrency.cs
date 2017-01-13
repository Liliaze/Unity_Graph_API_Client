using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnSelectCurrency : MonoBehaviour {

	public void OnSelectCurrencyText (Text t)
	{
		Config.currency = t.text[t.text.Length - 2];
		print(Config.currency);
	}
}
