using UnityEngine;
using System.Collections;

/// <summary>
/// Spawns the enemy after some time.
/// </summary>
public class EnemySpawner : MonoBehaviour {

	[SerializeField] private AudioClip[] spawnSoundList;
	[SerializeField] private AudioSource monsterSpawnSource;
	[SerializeField] private GameObject enemyPrefab;
	
	private const float Y_OFFSET = 0.00f;

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver(EventNames.ON_MAIN_EVENT_GAME_STARTED, this.OnMainEventStarted);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_MAIN_EVENT_GAME_STARTED, this.OnMainEventStarted);
	}

	public void OnMainEventStarted() {
		this.StartCoroutine(this.WaitForSpawn());
		Debug.Log("Monster spawning initiated");
	}

	private IEnumerator WaitForSpawn() {
		yield return new WaitForSeconds(GameFlowConstants.RandomizeMonsterDelay());
		this.SpawnEnemy ();

		EventsInitiator.Instance.ActivateGameEvent (GameEventNames.MACHINE_ROOM_EVENT_NAME);
	}

	private void SpawnEnemy() {
		GameObject spawnedEnemy = GameObject.Instantiate (this.enemyPrefab) as GameObject;

		Vector3 position = EnemyPatrolPointDirectory.Instance.GetRandomPatrolPoint().position;
		position.y = Y_OFFSET;

		spawnedEnemy.transform.position = position;
		spawnedEnemy.transform.parent = this.transform;

		this.monsterSpawnSource.clip = this.spawnSoundList [Random.Range (0, this.spawnSoundList.Length)];
		this.monsterSpawnSource.Play ();

		this.StartCoroutine (this.WaitForSpawn ());
	}
}
