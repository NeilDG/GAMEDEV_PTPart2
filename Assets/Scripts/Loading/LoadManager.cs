using UnityEngine;
using System.Collections;

public class LoadManager : MonoBehaviour {
	private static LoadManager sharedInstance = null;

	[SerializeField] private LoadingView loadingView;
	[SerializeField] private GameObject overlay;

	private bool dismissAutomatic = true;

	void Awake() {
		sharedInstance = this;
	}

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
		this.loadingView.SetVisibility (false);
		this.overlay.SetActive (false);
		this.loadingView.SetLoadManager (sharedInstance);
	}

	/// <summary>
	/// Loads the scene. If dismissAutomatically is set to true, the loading overlay will disappear immediately once the loading is complete.
	/// Otherwise, you would have to call ReportLoadComplete() to hide the overlay.
	/// </summary>
	/// <param name="sceneName">Scene name.</param>
	public static void LoadScene(string sceneName, bool dismissAutomatically) {
		sharedInstance.dismissAutomatic = dismissAutomatically;
		sharedInstance.StartCoroutine (sharedInstance.StartLoadSequence (sceneName));
	}

	/// <summary>
	/// Call this function to signify that loading is complete and will hide the loading overlay. If DismissAutomatically(false) was called,
	/// you need to call this method to hide the overlay manually.
	/// </summary>
	public static void ReportLoadComplete() {
		sharedInstance.loadingView.Hide ();
		sharedInstance.dismissAutomatic = true;
	}

	private IEnumerator StartLoadSequence(string sceneName) {
		this.loadingView.Show ();
		this.overlay.SetActive (true);
		yield return new WaitForSeconds (1.0f);

		Application.LoadLevel (sceneName);

		if (this.dismissAutomatic == true) {
			LoadManager.ReportLoadComplete();
		}
	}

	public void Cleanup() {
		this.overlay.SetActive (false);
	}
	
}
