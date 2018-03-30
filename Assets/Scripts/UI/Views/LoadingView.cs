using UnityEngine;
using System.Collections;

/// <summary>
/// Loading view
/// </summary>
public class LoadingView : View {

	private LoadManager loadManager;

	// Use this for initialization
	void Start () {
	
	}

	public void SetLoadManager(LoadManager loadManager) {
		this.loadManager = loadManager;
	}
		

	public override void OnHideCompleted ()
	{
		base.OnHideCompleted ();
		this.SetVisibility (false);
		this.loadManager.Cleanup ();
	}
}
