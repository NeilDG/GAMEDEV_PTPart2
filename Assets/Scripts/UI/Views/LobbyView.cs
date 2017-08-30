using UnityEngine;
using System.Collections;

public class LobbyView : View {

	// Use this for initialization
	void Start () {
	
	}

	public void OnClickedBack(){
		ViewHandler.Instance.Show (ViewNames.CHARACTER_PREVIEW_PANEL_STRING);
		this.Hide ();
	}

	public void OnClickedStartGame(){
		Application.LoadLevel ("InGame");
	}
}
