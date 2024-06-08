using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuGenerator : MonoBehaviour
{
	public Importer importer; // Reference to AssetLoader
	public GameObject buttonPrefab; // Prefab for the button
	public Transform menuParent; // Parent transform for the menu buttons
	public StringEvent OutputButtonClicked;

	void Start()
	{
		GenerateMenu();
	}

	public void GenerateMenu()
	{
		// Clear any existing buttons
		foreach (Transform child in menuParent)
		{
			Destroy(child.gameObject);
		}

		// Create buttons for each asset path
		foreach (string path in importer.loadedAssetPaths)
		{
			GameObject button = Instantiate(buttonPrefab, menuParent);
			button.GetComponentInChildren<Text>().text = System.IO.Path.GetFileName(path);
			button.GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(path));
		}
	}

	void OnButtonClicked(string path)
	{
		Debug.Log("Button clicked: " + path);
		OutputButtonClicked.Invoke(path);
	}
}
