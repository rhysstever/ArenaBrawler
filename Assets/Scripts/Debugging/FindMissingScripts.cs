using UnityEngine;
using UnityEditor;
using System.Linq;

// Not my code, found here: https://www.youtube.com/watch?v=kSVVQbMVhfA&t=355s
// Thanks Warped Imagination
public static class FindMissingScripts
{
    [MenuItem("MyMenu/Find Missing Scripts in Project")]

    static void FindMissingScriptsInProjectMenuItem()
	{
		string[] prefabPaths = AssetDatabase.GetAllAssetPaths().Where(path => path.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase)).ToArray();

		foreach(string path in prefabPaths)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			foreach(Component component in prefab.GetComponentsInChildren<Component>())
			{
				if(component == null)
				{
					Debug.Log("Prefab found with missing script " + path, prefab);
					break;
				}
			}
		}
	}
}
