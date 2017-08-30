using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Properly handles the pausing of the game
/// </summary>
public class GamePauseHandler {
	private static GamePauseHandler sharedInstance = null;

	public static GamePauseHandler Instance {
		get {
			if(sharedInstance == null) {
				sharedInstance = new GamePauseHandler();
			}
			return sharedInstance;
		}
	}

	private List<IPauseCommand> pauseCommandList = new List<IPauseCommand>();
	private List<IResumeCommand> resumeCommandList = new List<IResumeCommand>();

	private GamePauseHandler() {

	}

	/// <summary>
	/// Attachs a class that implements a pause and a resume command. This will be called upon pause of the game.
	/// Set the parameter to null on any one of this commands if in case the class does not implement both.
	/// </summary>
	public void AttachClassToVisit(IPauseCommand pauseCommand, IResumeCommand resumeCommand) {
		if (pauseCommand != null) {
			this.pauseCommandList.Add(pauseCommand);
		}

		if (resumeCommand != null) {
			this.resumeCommandList.Add(resumeCommand);
		}
	}

	public void Cleanup() {
		this.pauseCommandList.Clear ();
		this.resumeCommandList.Clear ();
	}

	public void Pause() {
		foreach (IPauseCommand pauseCommand in this.pauseCommandList) {
			pauseCommand.Pause();
		}
	}

	public void Resume() {
		foreach (IResumeCommand resumeCommand in this.resumeCommandList) {
			resumeCommand.Resume();
		}
	}
}
