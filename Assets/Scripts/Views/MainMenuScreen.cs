using UnityEngine;
using System.Collections;

/// <summary>
/// Represents the main menu view
/// </summary>
public class MainMenuScreen : View {

	// Use this for initialization
	void Start () {
	
	}

	public void OnStartClicked() {
		LoadManager.LoadScene(SceneNames.IN_GAME_SCENE, true);
	}

	public void OnQuitClicked(){
		Application.Quit();
	}
}
