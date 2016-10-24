using UnityEngine;
using UnityEditor;

	public class ExportAssetBundle {
		[MenuItem("MyMenu/Build AssetBundle From Selection - Track dependencies")]
		static void ExportResource () {
			// Bring up save panel
			string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
			if (path.Length != 0) {
				// Build the resource file from the active selection.
				Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
				BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, 
				                               BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets);
				Selection.objects = selection;
			}
		}
	[MenuItem("MyMenu/Build AssetBundle From Selection - No dependency tracking")]
		static void ExportResourceNoTrack () {
			// Bring up save panel
			string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
			if (path.Length != 0) {
				// Build the resource file from the active selection.
				BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path);
			}
		}
		
		
	[MenuItem("MyMenu/Build AssetBundle From Selection - ToAndroid")]
	static void ExportToAndroid () {
		string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
		if (path.Length != 0) {
			// Build the resource file from the active selection.
			Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, 
			                               BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets ,BuildTarget.Android);
			Selection.objects = selection;
		}
		}
		
		
	[MenuItem("MyMenu/Build AssetBundle From Selection - iPhone")]
	static void ExportToiPhone () {
		string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
		if (path.Length != 0) {
			// Build the resource file from the active selection.
			Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, 
			                               BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets ,BuildTarget.iPhone);
			Selection.objects = selection;
		}
	}
	
	[MenuItem("MyMenu/Build AssetBundle StreamScene - To Android")]
	static void ExportToScene () {
		//string path1= EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
		string[] path = {"Assets/00.Scenes/Track1.unity","Assets/00.Scenes/Track2.unity","Assets/00.Scenes/Track3.unity"
			,"Assets/00.Scenes/Track4.unity","Assets/00.Scenes/Track5.unity","Assets/00.Scenes/Track6.unity","Assets/00.Scenes/Drag_Track1.unity"
			,"Assets/00.Scenes/Event_Track1.unity","Assets/00.Scenes/Weekly.unity"};
		if (path.Length != 0) {
			// Build the resource file from the active selection.
			BuildPipeline.BuildStreamedSceneAssetBundle( path, "GameRace.unity3d", BuildTarget.Android); 
		}
	}
	
	
	
	
	}
