using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple represention of player HP measured by number of hits.
/// </summary>
public class PlayerHP : MonoBehaviour {

	private static PlayerHP sharedInstance = null;
	public static PlayerHP Instance {
		get {
			return sharedInstance;
		}
	}

	[SerializeField] private AudioClip[] hurtClipList;
	[SerializeField] private AudioClip dieAudioClip;
	[SerializeField] private AudioSource playerSource;

    //[SerializeField] private MouseLook mouseLookX;
    //[SerializeField] private MouseLook mouseLookY;
    private float rotationY = 0.0f;

	private const int MAX_PLAYER_HIT = 10;
	private int currentNumHits = 0;

	private bool deathAnimationPlaying = false;

	void Awake() {
		sharedInstance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(this.deathAnimationPlaying == true) {
			float xRotation = Time.deltaTime * 2;
			float zRotation = Time.deltaTime * 2;

			Quaternion quaternion = this.transform.rotation;
			quaternion.x += xRotation;
			quaternion.z += zRotation;

			if(this.transform.rotation.z <= 50) {
				this.transform.rotation = quaternion;
			}
		}
	}

	public void AttackHit() {
		if(this.currentNumHits < MAX_PLAYER_HIT) {
			this.currentNumHits++;
			
			this.playerSource.clip = this.hurtClipList [Random.Range (0, this.hurtClipList.Length)];
			this.playerSource.Play ();

            //simulate hit effect by moving the camera
            this.SimulateHit();
		}
		else {
			CharacterController characterControl = this.GetComponent<CharacterController>();
			characterControl.enabled = false;

			this.deathAnimationPlaying = true;

			this.playerSource.clip = this.dieAudioClip;
			this.playerSource.Play ();
			this.StartCoroutine(this.DelayRestartLevel());
		}
	}

	private IEnumerator DelayRestartLevel() {
		yield return new WaitForSeconds(this.playerSource.clip.length + 2.0f);
		Cursor.visible = true;
		SceneManager.LoadScene (SceneNames.MAIN_MENU_SCENE);
	}

    public void SimulateHit() {
        //TODO
        float rotationX = this.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 15.0f;
        transform.localEulerAngles = new Vector3(0, rotationX, 0);
    }
}
