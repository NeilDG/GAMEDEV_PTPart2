using UnityEngine;
using System.Collections;
using UnityEngine.AI;

/// <summary>
/// Spawns the enemy after some time.
/// </summary>
public class EnemySpawner : MonoBehaviour {

	[SerializeField] private AudioClip[] spawnSoundList;
	[SerializeField] private AudioSource monsterSpawnSource;
	//[SerializeField] private GameObject enemyPrefab;

	[SerializeField] private GameObject[] monsterPool;
	private int numActiveMonsters = 0;

	private const float Y_OFFSET = 0.00f;

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver(EventNames.ON_MAIN_EVENT_GAME_STARTED, this.OnMainEventStarted);
		this.HideMonsters ();
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_MAIN_EVENT_GAME_STARTED, this.OnMainEventStarted);
	}

	/// <summary>
	/// Temporarily hides the monsters and spawns them periodically
	/// </summary>
	private void HideMonsters() {
		for (int i = 0; i < this.monsterPool.Length; i++) {
			//set a random start position
			Vector3 position = EnemyPatrolPointDirectory.Instance.GetRandomPatrolPoint().position;
			this.monsterPool[i].GetComponent<NavMeshAgent> ().Warp (position);
			this.monsterPool[i].SetActive (false);
		}
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
		if (this.numActiveMonsters < this.monsterPool.Length) {
			this.monsterPool [this.numActiveMonsters].SetActive (true);
			this.monsterSpawnSource.clip = this.spawnSoundList [Random.Range (0, this.spawnSoundList.Length)];
			this.monsterSpawnSource.Play ();

			this.numActiveMonsters++;

			this.StartCoroutine (this.WaitForSpawn ());
		}

	}

	/*private void SpawnEnemy() {
		GameObject spawnedEnemy = GameObject.Instantiate (this.enemyPrefab) as GameObject;

		Vector3 position = EnemyPatrolPointDirectory.Instance.GetRandomPatrolPoint().position;
		position.y = Y_OFFSET;

		//spawnedEnemy.transform.position = position;
		spawnedEnemy.transform.parent = this.transform;
		spawnedEnemy.GetComponent<NavMeshAgent> ().Warp (position);

		this.monsterSpawnSource.clip = this.spawnSoundList [Random.Range (0, this.spawnSoundList.Length)];
		this.monsterSpawnSource.Play ();

		this.StartCoroutine (this.WaitForSpawn ());
	}*/
}
