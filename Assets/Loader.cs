using UnityEngine;

public class Loader : MonoBehaviour
{
	//public Importer importer;
	private GameObject selectedPrefab;
	public Texture2DEvent OutputTexture2D;
	public SpriteRenderer spriteRenderer;

	public void ApplyAsset(string path)
	{
		//foreach (var path in importer.loadedAssetPaths)
		//{
			// Example: Load texture and apply to prefab
			if (path.EndsWith(".png") || path.EndsWith(".jpg"))
			{
				Texture2D texture = LoadTextureFromFile(path);
				//ApplyTextureToPrefab(texture);
				OutputTexture2D.Invoke(texture);
			}
			// Add handling for other asset types (videos, 3D objects, sounds)
		//}
	}

	private Texture2D LoadTextureFromFile(string path)
	{
		byte[] fileData = System.IO.File.ReadAllBytes(path);
		Texture2D texture = new Texture2D(2, 2);
		texture.LoadImage(fileData);
		return texture;
	}

	private void ApplyTextureToPrefab(Texture2D texture)
	{
		if (selectedPrefab != null)
		{
			Renderer renderer = selectedPrefab.GetComponent<Renderer>();
			if (renderer != null)
			{
				renderer.material.mainTexture = texture;
			}
		}
	}
}
