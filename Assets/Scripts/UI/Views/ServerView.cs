using UnityEngine;
using System.Collections;

public class ServerView : View {

	// Use this for initialization
	void Start () {
	
	}

	public void OnServerClickedBack(){
		this.Hide ();
	}

	public void OnClickedCreate(){
		ViewHandler.Instance.Show (ViewNames.CHARACTER_SELECTION_PANEL_STRING);
	}
}
