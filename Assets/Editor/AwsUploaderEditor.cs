using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AwsUploader))]
public class AwsUploaderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		AwsUploader uploader = (AwsUploader)target;
		if (GUILayout.Button("Upload Video"))
		{
			uploader.StartUpload();
		}
	}
}
