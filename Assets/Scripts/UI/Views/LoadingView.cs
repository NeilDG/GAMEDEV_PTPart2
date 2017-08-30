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

	public override void Hide() {
		if(this.exitAnim != null && this.isExiting == false && this.enabled) {
			StartCoroutine(this.PlayExitAnim());
			this.isExiting = true;
		}
		else if(this.exitAnim == null && this.isExiting == false) {
			this.gameObject.SetActive(false);
			this.OnHideEvent();
		}
		
	}

	public override void OnHideEvent ()
	{
		base.OnHideEvent ();
		this.SetVisibility (false);
		this.loadManager.Cleanup ();
	}

	private IEnumerator PlayExitAnim() {
		
		//attach exit animation on the fly
		this.VerifyAnimationComponent();
		this.AttachClipIfNeeded(this.exitAnim, CLIP_EXIT_NAME);
		this.GetComponent<Animation>().Play(CLIP_EXIT_NAME);
		
		while (this.GetComponent<Animation>().isPlaying) { yield return null; }
		this.OnHideEvent ();
		
	}
}
