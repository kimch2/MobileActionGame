using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateMyScriptableObject : MonoBehaviour
{
	// ScriptableObject生成コマンド用テンプレート
	public static void CreateAssets<T>(string folderName) where T : ScriptableObject
	{
		// オブジェクト生成
		T obj = ScriptableObject.CreateInstance<T>();

		Debug.Log("保存フォルダ名:" + folderName);

		if (folderName != "") folderName += "/";
		
		// オブジェクトを保存するパス
		string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/" + folderName + typeof(T) + ".asset");

		Debug.Log(path);
		if (string.IsNullOrEmpty(path))
		{
			Debug.LogError("パスが違うか保存先のフォルダが見つかりません、フォルダがない場合は予め生成してから再度Createして下さい");
			return;
		}

		AssetDatabase.CreateAsset(obj, path);
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();
		Selection.activeObject = obj;
	}

	[MenuItem("Assets/Create/MyAssets/BGMListObject")]
	public static void CreateBGMListObject()
	{
		CreateAssets<BGMListObject>("BGMData");
	}

	[MenuItem("Assets/Create/MyAssets/ChildObject")]
	public static void CreateChildObject()
	{
		CreateAssets<ChildObject>("ChildData");
	}

	[MenuItem("Assets/Create/MyAssets/ContinuosHitObjectList")]
	public static void CreateContinuosHitObjectList()
	{
		CreateAssets<ContinuosHitObjectList>("ContinuosHitObjectList");
	}
}
