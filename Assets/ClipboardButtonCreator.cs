using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipboardButtonCreator : MonoBehaviour
{
	public GameObject buttonPrefab; // Prefab for the button
	public Transform buttonContainer; // Container to hold the buttons
	public string StringToPrepend;

	public void Input(List<string> stringList)
	{
		foreach (var str in stringList)
		{
			CreateButton(str);
		}
	}

	void CreateButton(string text)
	{
		GameObject buttonObj = Instantiate(buttonPrefab, buttonContainer);
		buttonObj.SetActive(true);
		Button button = buttonObj.GetComponent<Button>();
		TMPro.TMP_Text buttonText = buttonObj.GetComponentInChildren<TMPro.TMP_Text>();

		if (buttonText != null)
		{
			buttonText.text = text;
		}

		button.onClick.AddListener(() => CopyToClipboard(text));
	}

	void CopyToClipboard(string text)
	{
		GUIUtility.systemCopyBuffer = text;
		Debug.Log("Copied to clipboard: " + text);
	}
}
