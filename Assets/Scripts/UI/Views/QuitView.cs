using UnityEngine;
using System.Collections;

public class QuitView : View {

	private bool confirmed = true;

	// Use this for initialization
	void Start () {

	}


	public void OnQuitButtonClicked() {
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
			Application.Quit ();
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
