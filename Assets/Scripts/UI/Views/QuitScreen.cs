using UnityEngine;
using System.Collections;

public class QuitScreen : View {

	private bool confirmed = true;

	// Use this for initialization
	void Start () {

	}


	public void OnQuitButtonClicked() {
		this.confirmed = true;
		this.Hide ();
	}

	public void OnCancelButtonClicked() {
		this.confirmed = false;
		this.Hide ();
	}

	public override void OnHideCompleted ()
	{
		base.OnHideCompleted();

		if (this.confirmed) {
			LoadManager.LoadScene (SceneNames.MAIN_MENU_SCENE, true);
		} else {
			GamePauseHandler.Instance.Resume();
		}

	}

	public override void OnShowStarted ()
	{
		base.OnShowStarted ();
		GamePauseHandler.Instance.Pause ();
	}
}
