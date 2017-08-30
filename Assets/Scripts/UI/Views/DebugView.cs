using UnityEngine;
using System.Collections;

public class DebugView : View {

	// Use this for initialization
	void Start () {
	
	}

	public void OnMainMenuClicked() {
		ViewHandler.Instance.ShowViewWithNullProperty (ViewNames.MAIN_MENU_PANEL_STRING);
	}

	public void OnServerClicked() {
		ViewHandler.Instance.ShowViewWithNullProperty (ViewNames.SERVER_PANEL_STRING);
	}

	public void OnClientClicked() {
		ViewHandler.Instance.ShowViewWithNullProperty (ViewNames.CLIENT_PANEL_STRING);
	}

	public void OnCharacterSelectionClicked() {
		ViewHandler.Instance.ShowViewWithNullProperty (ViewNames.CHARACTER_SELECTION_PANEL_STRING);
	}

	public void OnCharacterPreviewClicked() {
		ViewHandler.Instance.ShowViewWithNullProperty (ViewNames.CHARACTER_PREVIEW_PANEL_STRING);
	}

	public void OnLobbyClicked() {
		ViewHandler.Instance.ShowViewWithNullProperty (ViewNames.LOBBY_PANEL_STRING);
	}
}
