using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages the game states properly
/// </summary>
public class GameStateMachine : MonoBehaviour {

	private static GameStateMachine sharedInstance = null;

	public static GameStateMachine Instance {
		get {
			return sharedInstance;
		}
	}

	//define custom game states here
	public enum StateType {
		INITIALIZE,
		PRE_GAME,
		MAIN_EVENT_GAME,
		GAME_ESCAPE_EVENT,
		POST_EVENT_GAME,
		END,
	}

	[SerializeField] private float delayUntilStart = 1.0f;

	private Dictionary<StateType, GameState> stateTable = new Dictionary<StateType, GameState>();
	private GameState currentState;

	void Awake() {
		sharedInstance = this;

		this.InitializeStateMachine ();
	}

	// Use this for initialization
	void Start () {
		this.StartCoroutine (this.DelayStart ());
	}
	
	// Update is called once per frame
	void Update () {
		if (this.currentState != null) {
			this.currentState.OnUpdate();
		}

		if(Input.GetKeyDown(KeyCode.Escape)) {
			ViewHandler.Instance.Show(ViewNames.QUIT_PANEL_STRING);
		}
	}

	void OnDestroy() {
		this.currentState = null;
		this.stateTable.Clear ();
	}

	/// <summary>
	/// Delays the start of the machine, to give chance for other components to properly initialize.
	/// </summary>
	private IEnumerator DelayStart() {
		yield return new WaitForSeconds (this.delayUntilStart);

		//set start state
		this.ChangeState(StateType.PRE_GAME);
	}

	private void InitializeStateMachine() {
		//fill up state table
		this.stateTable.Add(StateType.PRE_GAME, new PreparationState());
		this.stateTable.Add(StateType.MAIN_EVENT_GAME, new MainGameState());
		this.stateTable.Add (StateType.GAME_ESCAPE_EVENT, new GameEscapeState ());
	}

	public void ChangeState(GameStateMachine.StateType newStateType) {
		if (this.currentState != null) {
			this.currentState.OnEnd();
		}

		if (this.stateTable.ContainsKey (newStateType)) {
			this.currentState = this.stateTable [newStateType];
			this.currentState.OnStart();
		} else {
			Debug.LogError(newStateType + " does not exist in state machine. Please make sure you have added it in InitializeStateMachine().");
		}

	}

	public static bool HasProperState() {
		return (sharedInstance != null && sharedInstance.currentState != null);
	}

	public StateType GetGameState() {
		return this.currentState.GetStateType ();
	}
}
