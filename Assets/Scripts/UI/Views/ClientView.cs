using UnityEngine;
using System.Collections;

public class ClientView : View {

	// Use this for initialization
	void Start () {
	
	}

	public void OnClickedJoin(){
		ViewHandler.Instance.Show (ViewNames.CHARACTER_SELECTION_PANEL_STRING);
		this.Hide ();
	}

	public void OnClickedBack(){
		this.Hide ();
	}

}
