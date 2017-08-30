using UnityEngine;
using System.Collections;

public class ResultsView : View {

	void Start() {

	}

	public void OnRetryClicked() {
		EventBroadcaster.Instance.RemoveAllObservers ();
		LoadManager.LoadScene (SceneNames.IN_GAME_SCENE, false);
	}

	public void OnMainMenuClicked() {
		EventBroadcaster.Instance.RemoveAllObservers ();
		LoadManager.LoadScene (SceneNames.MAIN_MENU_SCENE, true);
	}

	public override void OnShowEvent ()
	{
		base.OnShowEvent ();
		GamePauseHandler.Instance.Pause ();
	}

	public override void OnHideEvent ()
	{
		base.OnHideEvent ();
		GamePauseHandler.Instance.Resume ();
	}
}
