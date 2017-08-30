using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Handles how views are being displayed in stack order.
 * 
 */ 
public class ViewHandler : MonoBehaviour {

	private static ViewHandler sharedInstance = null;
	public static ViewHandler Instance {
		get {
			return sharedInstance;
		}
	}

	[SerializeField] private GameObject uiRoot;
	[SerializeField] string screenPrefabLocation = "UI/Screens";
	[SerializeField] string firstScreen;
	
	[SerializeField] private List<View> activeViews = new List<View>();
	[SerializeField] private List<View> viewPool = new List<View>();	//views that need not be destroyed should be placed here

	private View rootView;

	private bool isQuittingPermitted = true;
	private bool viewHiddenFinished = true;

	void Awake() {
		sharedInstance = this;
	}

	// Use this for initialization
	void Start () {
		this.Show(this.firstScreen);
	}

	void OnDestroy() {

	}

	void Update() {
		if (this.isQuittingPermitted && Input.GetKeyDown(KeyCode.Escape)) {
			this.OnBack();
		}
	}


	public void Show(string screenName) {

		if (screenName == "")
				return;
		//Debug.LogWarning("Show " + screenName);

		View view;
		if(this.IsViewExisting(screenName)) {
			view = this.FindActiveView(screenName);
			this.activeViews.Remove(view);
			this.activeViews.Add(view);
		}
		else if(this.IsViewInPool(screenName)) {
			view = this.FindViewInPool(screenName);
			this.viewPool.Remove(view);
			this.activeViews.Add(view);

		}
		else {
			view = this.InitializeView(screenName);
			this.activeViews.Add(view);
		}

		view.Show();
		view.OnShowEvent();

		if(view.isRootScreen()) {
			this.rootView = view;
		}

		this.RearrangeOverlay();
	}

	/// <summary>
	/// Instantiates the view and then puts it temporarily in a view pool for faster retrieval. Use this for process-intensive
	/// views that consumes quite an amount of frame rate upon instantiating.
	/// 
	/// Automatically hides the view when called.
	/// </summary>
	/// <param name="screenName">Screen name.</param>
	public void PutToPool(string screenName) {
		if(this.IsViewExisting(screenName)){
			View view = this.FindActiveView(screenName);
			this.activeViews.Remove(view);

			if(!this.viewPool.Contains(view)) {
				view.DoNotDestroy();
				this.viewPool.Add(view);
			}

			view.Hide();

		}
		else {
			View view= this.InitializeView(screenName);
			this.viewPool.Add(view);

			view.Hide();
		}
	}

	public void PutToPool(View view) {
		if(!this.viewPool.Contains(view)) {
			this.viewPool.Add(view);
		}
	}

	public bool IsViewInPool(string screenName) {
		for(int i = 0; i < this.viewPool.Count; i++) {
			View view = this.viewPool[i];

			if(view.GetName() == screenName) {
				return true;
			}
		}

		return false;
	}

	public View FindViewInPool(string screenName) {
		for(int i = 0; i < this.viewPool.Count; i++) {
			View view = this.viewPool[i];
			
			if(view.GetName() == screenName) {
				return view;
			}
		}
		
		return null;
	}

	private View InitializeView(string screenName) {
		View view;
		
		GameObject gObj = GameObject.Instantiate(this.GetScreenPrefab(screenName)) as GameObject;
		gObj.transform.parent = uiRoot.transform;
		gObj.transform.localScale = Vector3.one;
		gObj.transform.localPosition = Vector3.zero;
		gObj.transform.localRotation = Quaternion.identity;
		gObj.name = screenName;
		gObj.SetActive(false);
		
		view = gObj.GetComponent<View>();

		return view;
	}

	private void RearrangeOverlay() {
		View activeView = this.GetActiveView();
		if(activeView.isRootScreen()) {
			UIBlackOverlay.Hide();
		}
		else {
			UIPanel assignedPanel = activeView.GetComponent<UIPanel>();
			UIBlackOverlay.Show(assignedPanel);
		}

	}

	public void HideCurrentView() {
		this.OnBack();
	}

	public View GetActiveView() {
		if(this.activeViews.Count == 0) {
			return null;
		}
		else {
			return this.activeViews[this.activeViews.Count - 1];
		}
	}

	/// <summary>
	/// Shows the view with null property. This removes the functionality of the view
	/// Only to be used for debugging.
	/// </summary>
	public void ShowViewWithNullProperty(string screenName) {
		Debug.LogWarning("Show " + screenName);
		
		View view;
		if(this.IsViewExisting(screenName)) {
			view = this.FindActiveView(screenName);
			this.activeViews.Remove(view);
			this.activeViews.Add(view);
		}
		else {
			view = this.InitializeView(screenName);
			this.activeViews.Add(view);
		}

		this.ReplaceActiveViewWithNull();
		View activeView = this.GetActiveView();
		activeView.Show();
		activeView.OnShowEvent();
		
		if(view.isRootScreen()) {
			this.rootView = view;
		}
		
		this.RearrangeOverlay();
	}
	
	private void ReplaceActiveViewWithNull() {
		GameObject viewObject = this.activeViews[this.activeViews.Count - 1].gameObject;
		NullScreen nullScreen = viewObject.AddComponent<NullScreen>();
		View originalView = this.activeViews[this.activeViews.Count - 1];
		nullScreen.CopyProperty(originalView);

		GameObject.Destroy(viewObject.GetComponent<Animation>());
		GameObject.Destroy(this.activeViews[this.activeViews.Count - 1]);
		this.activeViews[this.activeViews.Count - 1] = nullScreen;
	}

	public View FindActiveView(string screenName) {
		foreach(View view in this.activeViews) {
			if(view.GetName() == screenName) {
				return view;
			}
		}

		Debug.LogError(screenName + " not found in view list!");
		return null;
	}

	public bool IsViewExisting(string screenName) {
		foreach(View view in this.activeViews) {
			if(view.GetName() == screenName) {
				return true;
			}
		}

		return false;
	}

	public void ClearPopupViews() {
		foreach(View view in this.activeViews) {
			if(!view.isRootScreen()) {
				view.OnHideEvent();
				GameObject.Destroy(view.gameObject);
			}
		}

		this.activeViews.Clear();
		this.activeViews.Add(this.rootView);
	}

	private void OnBack() {
		if(this.viewHiddenFinished == false) {
			return;
		}

		this.viewHiddenFinished = false;
		View activeView = this.GetActiveView();

		activeView.OnBackButtonPressed();
		if (activeView.isRootScreen () == false) {
			activeView.Hide ();
		} else {
			this.Show(ViewNames.QUIT_PANEL_STRING);
		}
	}

	private GameObject GetScreenPrefab(string screenName)
	{
		
		UnityEngine.Object obj = Resources.Load(screenPrefabLocation + "/" + screenName);
		if (obj == null) return null;
		
		GameObject gObj = obj as GameObject;
		return gObj;
	}

	public void OnViewHidden(View view) {

		this.activeViews.Remove(view);
		view.OnHideEvent();

		if(view.ShouldBeDestroyed()) {
			GameObject.Destroy(view.gameObject);
			Resources.UnloadUnusedAssets();
			System.GC.Collect();
		}
		else {
			this.PutToPool(view);
			view.SetVisibility(false);
		}


		//this.RearrangeOverlay();
		this.StartCoroutine(this.DelayViewHiddenFinished());

		View newActiveView = this.GetActiveView();
		if(newActiveView != null) {
			newActiveView.OnShowEvent();
		}
	}

	private IEnumerator DelayViewHiddenFinished() {
		yield return new WaitForSeconds(0.3f);
		this.viewHiddenFinished = true;
	}

	public void ToggleAllViews(bool active) {
		foreach(View view in this.activeViews) {
			view.gameObject.SetActive(active);
		}
	}
}
