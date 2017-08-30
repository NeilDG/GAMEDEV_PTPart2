using UnityEngine;
using System.Collections;

/// <summary>
/// Represents the main menu view
/// </summary>
public class MainMenuView : View {

	// Use this for initialization
	void Start () {
	
	}

	public void OnStartClicked() {
		Application.LoadLevel(SceneNames.BRIGHTNESS_ADJUSTMENT_SCENE);
	}

	public void OnQuitClicked(){
		Application.Quit();
	}
}
