using UnityEngine;
using System.Collections;

public class CharacterPreviewView : View {

	// Use this for initialization
	void Start () {
	
	}

	public void OnClickedBack(){
		ViewHandler.Instance.Show (ViewNames.CHARACTER_SELECTION_PANEL_STRING);
		this.Hide ();
	}

	public void OnClickedFinish(){
		ViewHandler.Instance.Show (ViewNames.LOBBY_PANEL_STRING);
		this.Hide ();
	}
}
