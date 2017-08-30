using UnityEngine;
using System.Collections;

/// <summary>
/// Represents the basic user interface widget. Based from the old AScreen.
/// 
/// </summary>
public abstract class View : MonoBehaviour {
	[SerializeField] private bool asRootScreen = false;

	[SerializeField] protected AnimationClip enterAnim;
	[SerializeField] protected AnimationClip exitAnim;

	private Animation assignedAnimation;
	private bool destroyable = true;

	protected bool isExiting = false;
	protected bool cancelable = true;
	
	protected const string CLIP_ENTER_NAME = "ClipEnter";
	protected const string CLIP_EXIT_NAME = "ClipExit";

	
	// Use this for initialization
	void Start () {
	
	}

	public bool IsCancelable() {
		return this.cancelable;
	}

	public void SetCancelable(bool flag) {
		this.cancelable = flag;
	}

	public void DoNotDestroy() {
		this.destroyable = false;
	}

	public void MakeDestroyable() {
		this.destroyable = true;
	}

	public bool ShouldBeDestroyed() {
		return this.destroyable;
	}

	public string GetName() {
		return this.gameObject.name;
	}

	public void Show() {
		this.Reset();
		if(this.enterAnim != null) {
			this.PlayEnterAnim();
		}
	}

	private void Reset() {
		this.gameObject.SetActive(true);
		this.isExiting = false;
		this.transform.position = Vector3.zero;
		this.transform.localScale = Vector3.one;
		this.transform.rotation = Quaternion.identity;
	}

	public virtual void Hide() {
		if(this.exitAnim != null && this.isExiting == false && this.enabled) {
			StartCoroutine(this.PlayExitAnim());
			this.isExiting = true;
		}
		else if(this.exitAnim == null && this.isExiting == false) {
			this.gameObject.SetActive(false);
			ViewHandler.Instance.OnViewHidden(this);
		}

	}

	public void SetVisibility(bool flag) {
		this.gameObject.SetActive(flag);
	}

	public bool isRootScreen() {
		return this.asRootScreen;
	}

	private void PlayEnterAnim() {
		//attach enter animation on the fly
		this.VerifyAnimationComponent();
		this.AttachClipIfNeeded(this.enterAnim, CLIP_ENTER_NAME);
		this.GetComponent<Animation>().Play(CLIP_ENTER_NAME);
	}
	
	private IEnumerator PlayExitAnim() {
		
		//attach exit animation on the fly
		this.VerifyAnimationComponent();
		this.AttachClipIfNeeded(this.exitAnim, CLIP_EXIT_NAME);
		this.GetComponent<Animation>().Play(CLIP_EXIT_NAME);

		while (this.GetComponent<Animation>().isPlaying) { yield return null; }
		ViewHandler.Instance.OnViewHidden(this);
		
	}
	
	protected void VerifyAnimationComponent() {
		if(this.assignedAnimation == null) {
			this.assignedAnimation = this.gameObject.AddComponent<Animation>();
		}
	}
	
	protected void AttachClipIfNeeded(AnimationClip clip, string clipName) {
		if(this.assignedAnimation.GetClip(clipName) == null) {
			this.assignedAnimation.AddClip(clip, clipName);
		}
	}

	public void CopyProperty(View view) {
		this.enterAnim = view.enterAnim;
		this.exitAnim = view.exitAnim;
		this.assignedAnimation = view.assignedAnimation;
	}

	#region Events Thrown
	public virtual void OnShowEvent() {}
	public virtual void OnBackButtonPressed() {}
	public virtual void OnHideEvent() {}
	#endregion
}
