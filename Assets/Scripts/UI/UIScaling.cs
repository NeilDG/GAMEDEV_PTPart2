using UnityEngine;
using System.Collections;

/// <summary>
/// Simple class to handle transform scaling with a resolution policy classified as SHOW ALL
/// Created By: NeilDG
/// </summary>
public class UIScaling : MonoBehaviour {

	[SerializeField] private bool manualSetup; //if checked, developer would have to call ForceSetup manually.

	private const string GAME_ROOT_NAME = "GameRoot";
	private const string BASE_OBJECT_NAME = "Base";

	private const int MAX_WIDTH = 1598;
	private const int MAX_HEIGHT = 900;

	private static Vector3 computedScale = Vector3.one;

	private UIWidget baseWidget;
	private int baseWidgetWidth = 0;
	private int baseWidgetHeight = 0;



	void Awake () {
		this.baseWidget = this.GetComponent<UIWidget>();
		//Debug.Log("Awake width n height : " +  baseWidget.width + ", " + baseWidget.height);
	}


	void Start () {

		if(!manualSetup) {
			Debug.Log("Start width n height : " +  baseWidget.width + ", " + baseWidget.height);
			ForceSetup();
		}
	}

	public void UpdateAllAnchors() {
		
		this.baseWidget.UpdateAnchors();

		UIPanel[] panelChildren = this.baseWidget.GetComponentsInChildren<UIPanel>();
		
		foreach(UIPanel childPanel in panelChildren) {
			childPanel.UpdateAnchors();
		}

		UIWidget[] widgetChildren = this.baseWidget.GetComponentsInChildren<UIWidget>();
		
		foreach(UIWidget widgetChild in widgetChildren) {
			widgetChild.UpdateAnchors();
		}
	}


	public void ForceSetup()
	{


		this.baseWidgetWidth = this.baseWidget.width;
		this.baseWidgetHeight = this.baseWidget.height;

		//Debug.Log("Force setup width n height : " +  baseWidget.width + ", " + baseWidget.height);

		computedScale.x = this.baseWidgetWidth * 1.0f / MAX_WIDTH;
		computedScale.y = this.baseWidgetHeight * 1.0f / MAX_HEIGHT;
		computedScale.z = computedScale.x;
		this.transform.localScale = computedScale;

		this.UpdateAllAnchors();
	}


	public static Vector3 GetComputedScale() {
		return computedScale;
	}


	void Update () 
	{
		if(Input.GetMouseButtonDown(1)) {
			Debug.Log("width n height : " +  baseWidget.width + ", " + baseWidget.height);
			Debug.Log("Scale: " + this.transform.localScale);
		}
	}

}
