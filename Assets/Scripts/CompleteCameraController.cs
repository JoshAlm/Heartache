using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class CompleteCameraController : MonoBehaviour {

	public GameObject player;		//Public variable to store a reference to the player game object
    private CompletePlayerController playerC;
    private RangerController rangerC;

    private Vector3 offset;			//Private variable to store the offset distance between the player and camera

    public float time;
    public Text timerText;

    private AudioSource audioS;
    // Use this for initialization
    void Start () 
	{
        audioS = GetComponent<AudioSource>();
        playerC = player.GetComponent<CompletePlayerController>();
        rangerC = GameObject.Find("Ranger").GetComponent<RangerController>();
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = transform.position - player.transform.position;
	}

    public void Lose()
    {
        rangerC.GameStopped();
        audioS.Stop();
        timerText.text = "";
        transform.position = new Vector3(0, 0, 0);
        this.enabled = false;
    }

    void Update()
    {
        timerText.text = "Time: " + (Mathf.Round(time*100)/100).ToString();
        if (time <= 0)
        {
            playerC.Victory();
            rangerC.GameStopped();
            audioS.Stop();
            this.enabled = false;
        }
        time -= Time.deltaTime;
        if (time < 0)
            time = 0;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate () 
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		transform.position = player.transform.position + offset;
	}
}
