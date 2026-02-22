using UnityEditor;
using UnityEngine;


namespace WebGameProject.Editor
{
	/// <summary>
	/// 텍스트 애셋 추가.
	/// </summary>
	public class TextAssetCreator
	{
		private static bool IsSelection()
		{
			var selected = Selection.activeObject;
			return selected;
		}

		private static bool IsSelectionWithFolder()
		{
			if (!IsSelection())
				return false;

			var path = AssetDatabase.GetAssetPath(Selection.activeObject);
			var isFolder = AssetDatabase.IsValidFolder(path);
			return isFolder;
		}

		[MenuItem("Assets/Create/Text/TXT")]
		public static void CreateTXT()
		{
		}

		[MenuItem("Assets/Create/Text/JSON")]
		public static void CreateJSON()
		{
		}

		[MenuItem("Assets/Create/Text/CSV")]
		public static void CreateCSV()
		{
		}

		[MenuItem("Assets/Create/Text/XML")]
		public static void CreateXML()
		{
		}

		[MenuItem("Assets/Create/Text/Markdown")]
		public static void CreateMarkdown()
		{
		}

		[MenuItem("Assets/Create/Text/JSLIB")]
		public static void CreateJSLIB()
		{
		}
	}
}
