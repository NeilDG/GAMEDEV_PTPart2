using UnityEngine;
using System.Collections;

/// <summary>
/// Representation of a game state
/// </summary>
public abstract class GameState {

	private GameStateMachine.StateType stateType;

	public abstract void OnStart();
	public abstract void OnUpdate(); //initialize and end game are states that may not need this
	public abstract void OnEnd();

	public void SetStateType(GameStateMachine.StateType stateType) {
		this.stateType = stateType;
	}

	public GameStateMachine.StateType GetStateType() {
		return this.stateType;
	}
}
