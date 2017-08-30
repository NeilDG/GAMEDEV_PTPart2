using UnityEngine;
using System.Collections;

/// <summary>
/// utility class that points to a custom resource location of the localization CSV file.
/// This loads that file instead. This is inserted in Localization.cs of NGUI. Will have to reinsert
/// the method function every time NGUI is updated.
/// 
/// Created By: NeilDG
/// </summary>
public static class CustomLocalizationLoader {

	public static TextAsset LoadLocalizationCSV() {
		// Try to load the Localization CSV
		TextAsset txt =  Resources.Load("Localization/Localization", typeof(TextAsset)) as TextAsset;
		return txt;
	}

}
