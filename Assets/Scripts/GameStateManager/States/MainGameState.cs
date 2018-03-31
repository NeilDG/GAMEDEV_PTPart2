using UnityEngine;
using System.Collections;

/// <summary>
/// This is the main game state where lights are out and monster should spawn!
/// </summary>
public class MainGameState : GameState {

	public override void OnStart ()
	{
		EventBroadcaster.Instance.PostEvent(EventNames.ON_MAIN_EVENT_GAME_STARTED);
		Debug.Log("Main event started!");

		ObjectiveView.ShowObjective (DialogConstants.OBJECTIVE_2_STRING);
	}

	public override void OnUpdate ()
	{
		
	}

	public override void OnEnd ()
	{

	}
}
