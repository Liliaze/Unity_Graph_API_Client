using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class OnSelectDate : MonoBehaviour {
	TouchScreenKeyboard keyboard;
	public static string keyboardText = "";


	public void OnEnterDateText (Text t)
	{
		keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
	}

	void update ()
	{
		if (TouchScreenKeyboard.visible == false && keyboard != null)
		{
			if (keyboard.done == true)
			{
				keyboardText = keyboard.text;
				keyboard = null;
			}
		}
	}

	public void OnSelectDateText (Text t)
	{
		string pattern = @"^((19\d{2})|(20\d{2}))-(((02)-(0[1-9]|[1-2][0-9]))|(((0(1|[3-9]))|(1[0-2]))-(0[1-9]|[1-2][0-9]|30))|((01|03|05|07|08|10|12)-(31)))$";

		Match match = Regex.Match(t.text, pattern);
		if (match.Value != "")
		{
			Config.date = t.text;
			print(Config.date);
			t.color = Color.green;
			print("ok date = " + Config.date);
		}
		else
		{
			t.color = Color.red;
			print(t.text + "= bad format");
		}
	}
}
