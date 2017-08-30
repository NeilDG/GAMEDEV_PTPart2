using UnityEngine;
using System.Collections;

/*
 * A separate black overlay for the UI stuffs that adjusts accordingly depending on how many
 * times Show() has been called.
 * 
 */
public class UIBlackOverlay : MonoBehaviour {

	private static UIBlackOverlay sharedInstance = null;

	[SerializeField] private UIPanel panel;

	void Awake() {
		if(sharedInstance == null) {
			sharedInstance = this;
		}
		
	}

	// Use this for initialization
	void Start () {
		sharedInstance.panel.gameObject.SetActive (false);
	}

	/// <summary>
	/// Shows the black overlay. Its depth will be the depth of the calling panel minus 1.
	/// </summary>
	public static void Show(UIPanel callingPanel) {

		if(sharedInstance == null) {
			Debug.LogWarning("Black overlay not found or Overlay is set to false");
			return;
		}
		sharedInstance.panel.gameObject.SetActive (true);
		sharedInstance.panel.depth = callingPanel.depth - 1;
	}

	/// <summary>
	/// Attempts to hide the black overlay.
	/// </summary>
	public static void Hide() {

		if(sharedInstance == null) {
			Debug.LogWarning("Black overlay not found");
			return;
		}

		sharedInstance.panel.gameObject.SetActive (false);
	}

	public void OnOverlayClicked() {
		if(ViewHandler.Instance.GetActiveView().IsCancelable()) {
			ViewHandler.Instance.HideCurrentView();
		}
	}
}
