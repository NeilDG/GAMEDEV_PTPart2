using UnityEngine;
using System.Collections;

public class GameEscapeState : GameState {

	public override void OnStart ()
	{
		ObjectiveView.ShowObjective (DialogConstants.OBJECTIVE_4_STRING, 8.0f);
		EventBroadcaster.Instance.PostEvent (EventNames.ON_ESCAPE_EVENT_STARTED);
	}	

	public override void OnUpdate ()
	{

	}

	public override void OnEnd ()
	{

	}
}
