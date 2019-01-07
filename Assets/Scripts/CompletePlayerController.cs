using UnityEngine;
using System.Collections;

//Adding this allows us to access members of the UI namespace including Text.
using UnityEngine.UI;

public class CompletePlayerController : MonoBehaviour {

	public float speed;				//Floating point variable to store the player's movement speed.
	public Text winText;			//Store a reference to the UI Text component which will display the 'You win' message.
    public Text healthText;

    public AudioClip audioWin;      //Store a reference to the win audio clip
    public AudioClip audioHurt;
    public AudioClip audioBlock;

	private Rigidbody2D rb2d;		//Store a reference to the Rigidbody2D component required to use 2D Physics.

    private Animator anim;          //Store a reference to the Animator component to use animations.
    private AudioSource audios;     //Store a reference to the AudioSource component to use audio.

    public GameObject shieldAura;
    private GameObject shieldInst = null;

    private bool isShield;

    public int health;

    private CompleteCameraController cameraC;

    // A function to switch to and play specific audio clips
    void playAudio(AudioClip audioToPlay) {

        // Setting audio source clip to audioToPlay clip
        audios.clip = audioToPlay;

        // Playing the audio clip
        audios.Play(0);
    }

    // Use this for initialization
    void Start()
	{
        healthText.text = "Health: " + health.ToString();
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D> ();

		//Initialze winText to a blank string since we haven't won yet at beginning.
		winText.text = "";

        // Get and store a reference to the Animator component
        anim = GetComponent<Animator> ();

        // Get and store a reference to the AudioSource component
        audios = GetComponent<AudioSource>();

        cameraC = GameObject.Find("Main Camera").GetComponent<CompleteCameraController>();
	}

    void Shield()
    {
        var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float dirX = targetPos.x - transform.position.x;
        float dirY = targetPos.y - transform.position.y;
        float angle = Mathf.Atan2(dirY, dirX) * Mathf.Rad2Deg;
        anim.Play("PlayerAnim_Shielding");
        shieldInst = (GameObject)Instantiate(shieldAura, transform.position, Quaternion.Euler(new Vector3(0, 0, angle - 90)));

        shieldInst.transform.parent = this.transform;

        rb2d.velocity = new Vector2(0,0);
        isShield = true;
    }

    public void ShieldBlocked()
    {
        playAudio(audioBlock);
        isShield = false;
    }

    void StopShield()
    {
        isShield = false;
        if(shieldInst)
            Destroy(shieldInst);
    }

    void TakeDamage()
    {
        health -= 1;
        playAudio(audioHurt);
        healthText.text = "Health: " + health.ToString();
        StopShield();
        if (health <= 0)
        {
            winText.text = "You lose...";
            healthText.text = "";
            cameraC.Lose();
            rb2d.velocity = new Vector2(0, 0);
            this.enabled = false;
        }
    }

    public void Victory()
    {
        playAudio(audioWin);
        winText.text = "You Win!";
        rb2d.velocity = new Vector2(0, 0);
        this.enabled = false;
    }

    //Update is called every frame. Put most non-physics code here.
    void Update()
    {
        if (isShield && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAnim_Shielding"))
        {
            StopShield();
        }
        if (!isShield && Input.GetMouseButtonDown(0))
        {
            Shield();
        }
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
	{
        if (isShield)
            return;

        //Store the current horizontal input in the float moveHorizontal. Using GetAxisRaw makes it return exactly -1, 0, or 1; no smoothing.
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        //Store the current vertical input in the float moveVertical. Using GetAxisRaw makes it return exactly -1, 0, or 1; no smoothing.
        float moveVertical = Input.GetAxisRaw("Vertical");

		//Use the two store floats to create a new Vector2 variable movement.
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

        //Replaced addforce with setting velocity for more precise movement.
		rb2d.velocity = (movement * speed);

        // Checking if player isn't moving and setting animation variables accordingly
        if (rb2d.velocity == new Vector2(0, 0))
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isRun", false);
        }
        else
        {
            anim.SetBool("isRun", true);
            anim.SetBool("isIdle", false);
        }
	}

    //OnTriggerEnter2D is called whenever this object overlaps with a trigger collider.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectiles") )
        {
            Destroy(other.gameObject);
            TakeDamage();
        }
    }
}
