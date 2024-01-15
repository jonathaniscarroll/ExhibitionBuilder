﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(DropdownPrefabListAttribute))]
public class DropDownPrefabListEditor : PropertyDrawer
{

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (property.propertyType == SerializedPropertyType.String){
			
			EditorGUI.BeginProperty(position, label, property);

			var attribute = this.attribute as DropdownPrefabListAttribute;
		
			List<string> objectList = new List<string>();
			//foreach(SpawnUIElements.NamedUIElements obj in property.serializedObject(nameof(UIElements))){
			//	objectList.Add(obj.name);
			//}

			string propertyString = property.stringValue;
			int index = -1;
			if (propertyString == "")
			{
				//The tag is empty
				index = 0; //first index is the special <notag> entry
			}
			else
			{
				//check if there is an entry that matches the entry and get the index
				//we skip index 0 as that is a special custom case
				for (int i = 1; i < objectList.Count; i++)
				{
					if (objectList[i] == propertyString)
					{
						index = i;
						break;
					}
				}
			}
		
			//Draw the popup box with the current selected index
			index = EditorGUI.Popup(position, label.text, index, objectList.ToArray());

			//Adjust the actual string value of the property based on the selection
			if (index == 0)
			{
				property.stringValue = "";
			}
			else if (index >= 1)
			{
				property.stringValue = objectList[index];
			}
			else
			{
				property.stringValue = "";
			}
		
			EditorGUI.EndProperty();
		}
		else
		{
			EditorGUI.PropertyField(position, property, label);
		}
		
	}
}