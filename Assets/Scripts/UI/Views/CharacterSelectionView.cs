using UnityEngine;
using System.Collections;

public class CharacterSelectionView : View {

	// Use this for initialization
	void Start () {
	
	}

	public void OnClickedWarrior(){
		ViewHandler.Instance.Show (ViewNames.CHARACTER_PREVIEW_PANEL_STRING);
		this.Hide ();
	}

	public void OnClickedRouge(){
		ViewHandler.Instance.Show (ViewNames.CHARACTER_PREVIEW_PANEL_STRING);
		this.Hide ();
	}

	public void OnClickedMage(){
		ViewHandler.Instance.Show (ViewNames.CHARACTER_PREVIEW_PANEL_STRING);
		this.Hide ();
	}

	public void OnClickedBack(){
		this.Hide ();
	}
}
