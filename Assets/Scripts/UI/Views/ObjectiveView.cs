using UnityEngine;
using System.Collections;

public class ObjectiveView : View {
	[SerializeField] private UILabel label;

	private const float DURATION = 4.0f;

	private void SetMessage(string message) {
		this.label.text = message;
	}

	public override void OnShowEvent ()
	{
		base.OnShowEvent ();

		this.StartCoroutine (this.DelayHide ());
	}

	private IEnumerator DelayHide() {
		yield return new WaitForSeconds (DURATION);

		this.Hide ();
	}

	public static void ShowObjective(string message, float duration = DURATION) {
		ViewHandler.Instance.Show (ViewNames.OBJECTIVE_PANEL_STRING);

		ObjectiveView objectiveView = (ObjectiveView)ViewHandler.Instance.FindActiveView (ViewNames.OBJECTIVE_PANEL_STRING);
		objectiveView.SetMessage (message);
	}
}
