using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerController : MonoBehaviour {

    public GameObject arrow;

    public int projSpeed;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private Rigidbody2D rb2d;
    private Animator anim;
    private AudioSource audioSO;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSO = GetComponent<AudioSource>();
    }

    void Shoot()
    {
        var targetPos = GameObject.Find("Player").transform.position;
        float dirX = targetPos.x - transform.position.x;
        float dirY = targetPos.y - transform.position.y;
        float angle = Mathf.Atan2(dirY, dirX) * Mathf.Rad2Deg;
        var proj = (GameObject)Instantiate(arrow, transform.position, Quaternion.Euler(new Vector3(0, 0, angle - 180)));
        float uvX = dirX / Mathf.Sqrt(Mathf.Pow(dirX, 2) + Mathf.Pow(dirY, 2));
        float uvY = dirY / Mathf.Sqrt(Mathf.Pow(dirX, 2) + Mathf.Pow(dirY, 2));
        Physics2D.IgnoreCollision(proj.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        proj.GetComponent<Rigidbody2D>().velocity = new Vector2(uvX * projSpeed, uvY * projSpeed);
        Destroy(proj, 5);
        anim.SetTrigger("shoot");
        audioSO.Play(0);
    }

    void Move()
    {
        float xDir = Random.Range(-30f, 30f);
        float yDir = Random.Range(-30f, 30f);
        if (Mathf.Abs(xDir) < 10f || Mathf.Abs(yDir) < 10f)
        {
            Move();
        }
        else if (xDir > 0 && transform.position.x >= maxX)
        {
            Move();
        }
        else if (xDir < 0 && transform.position.x <= minX)
        {
            Move();
        }
        else if (yDir > 0 && transform.position.y >= maxY)
        {
            Move();
        }
        else if (yDir < 0 && transform.position.y <= minY)
        {
            Move();
        }
        else
        {
            rb2d.velocity = new Vector2(xDir, yDir);
        }
    }

    public void GameStopped()
    {
        rb2d.velocity = new Vector2(0, 0);
        anim.SetTrigger("dies");
        this.enabled = false;
    }

	// Update is called once per frame
	void Update () {
        if(rb2d.velocity != new Vector2(0,0))
        {
            if (rb2d.velocity.x > 0 && transform.position.x >= maxX)
                rb2d.velocity = new Vector2(0, 0);
            if (rb2d.velocity.x < 0 && transform.position.x <= minX)
                rb2d.velocity = new Vector2(0, 0);
            if (rb2d.velocity.y > 0 && transform.position.y >= maxY)
                rb2d.velocity = new Vector2(0, 0);
            if (rb2d.velocity.y < 0 && transform.position.y <= minY)
                rb2d.velocity = new Vector2(0, 0);
            if(Random.Range(0,1000f) <= 30f)
                rb2d.velocity = new Vector2(0, 0);
        }
        if (rb2d.velocity == new Vector2(0, 0))
            anim.SetBool("isRunning", false);
        else
            anim.SetBool("isRunning", true);
        float r = Random.Range(0, 1000f);
        if (r <= 20f)
        {
            Shoot();
        }
        if (r > 20f && r <= 65f && rb2d.velocity == new Vector2(0, 0))
        {
            Move();
        }
	}
}
