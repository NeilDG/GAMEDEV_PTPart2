using UnityEngine;
using System.Collections;

/// <summary>
/// Represents the preparation state for the player. Lights are on and no monsters should appear.
/// </summary>
public class PreparationState : GameState {

	private float randomTime = 0.0f;

	private float currentTime = 0.0f;

	private bool hasTriggeredClosure = false;
	private bool lightsOutEventTriggered = false;

	public override void OnStart ()
	{
		this.currentTime = 0.0f;
		this.randomTime = GameFlowConstants.RandomizePreparationTime();

		EventBroadcaster.Instance.AddObserver(EventNames.ON_LIGHTS_OUT_EVENT_FINISHED, this.OnReceivedLightsOutEvent);

		ObjectiveView.ShowObjective (DialogConstants.OBJECTIVE_1_STRING);

		GameStateMachine.Instance.StartCoroutine (this.SimulateTutorialFeedback ());
	}

	public override void OnUpdate ()
	{
		this.currentTime += Time.deltaTime;

		//Debug.Log("Current time: " +this.currentTime+ " Random time: " +this.randomTime);

		if(this.currentTime >= this.randomTime && this.hasTriggeredClosure == false) {
			this.hasTriggeredClosure = true;
			GameStateMachine.Instance.StartCoroutine(this.InitiateClosure());
		}
	}

	private IEnumerator SimulateTutorialFeedback() {
		yield return new WaitForSeconds (GameFlowConstants.DELAY_BEFORE_SHOW_TUTORIAL);
		ObjectiveView.ShowObjective (DialogConstants.TUTORIAL_1_STRING);

		yield return new WaitForSeconds (GameFlowConstants.DELAY_BEFORE_SHOW_TUTORIAL);
		ObjectiveView.ShowObjective (DialogConstants.TUTORIAL_2_STRING);

		yield return new WaitForSeconds (GameFlowConstants.DELAY_BEFORE_SHOW_TUTORIAL);
		ObjectiveView.ShowObjective (DialogConstants.TUTORIAL_3_STRING);

	}

	private IEnumerator InitiateClosure() {
		//play events prior to moving to main event
		Debug.Log("initiate closure!!");
		EventsInitiator.Instance.InitiateTremorEvent();

		Debug.Log("Tremor event finished!!");
		yield return new WaitForSeconds(GameFlowConstants.RandomizeTrevorDuration());

		CeilingLightsHandler.Instance.InitiateLightsOutEvent();

		while(this.lightsOutEventTriggered == false) {
			yield return null;
		}

		GameStateMachine.Instance.ChangeState(GameStateMachine.StateType.MAIN_EVENT_GAME);
	}

	public override void OnEnd ()
	{
		this.currentTime = 0.0f;
		EventBroadcaster.Instance.RemoveObserver(EventNames.ON_LIGHTS_OUT_EVENT_FINISHED);
	}

	private void OnReceivedLightsOutEvent() {
		this.lightsOutEventTriggered = true;
	}
}
