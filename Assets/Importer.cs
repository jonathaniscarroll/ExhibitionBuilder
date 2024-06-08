using UnityEngine;
using SimpleFileBrowser;
using System.Collections.Generic;

public class Importer : MonoBehaviour
{
	public List<string> loadedAssetPaths = new List<string>();
	public StringEvent OnImported;

	public void OpenFileBrowser()
	{
		FileBrowser.ShowLoadDialog(
			(paths) => { LoadAssets(paths); },
			null,
			FileBrowser.PickMode.Files,
			false,
			null,
			"Select Assets",
			"Load"
		);
	}

	private void LoadAssets(string[] paths)
	{
		foreach (var path in paths)
		{
			if (!loadedAssetPaths.Contains(path))
			{
				loadedAssetPaths.Add(path);
				Debug.Log("Loaded: " + path);
				OnImported.Invoke(path);
			}
		}
	}
}
