using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float checkRadius;
    public Text score;
    public Text livesText;
    public GameObject winTextObject;
    public GameObject livesTextObject;
    public Transform groundcheck;
    public LayerMask allGround;
    public AudioClip musicClipOne;
    public AudioSource musicSource;

    private Rigidbody2D rd2d;
    private int scoreValue;
    private int lives;
    private bool facingRight = true;
    private bool isOnGround;
    private bool jumping = false;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Coins: " + scoreValue.ToString();
        livesText.text = "Lives: " + lives.ToString();
        lives = 3;

        winTextObject.SetActive(false);
        livesTextObject.SetActive(false);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    // Update is called once per frame
    void FixedUpdate()

    {
       float hozMovement = Input.GetAxis("Horizontal");
       float verMovement = Input.GetAxis("Vertical"); 
       rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
       isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

       if (facingRight == false && hozMovement > 0)
       {
           Flip();
       }
       else if (facingRight == true && hozMovement < 0)
       {
           Flip();
       }

       if (Input.GetKeyDown(KeyCode.D))
       {
           anim.SetInteger("State", 1);
       }

       if (Input.GetKeyUp(KeyCode.D))
       {
           anim.SetInteger("State", 0);
       }

        if (Input.GetKeyDown(KeyCode.A))
       {
           anim.SetInteger("State", 1);
       }

        if (Input.GetKeyUp(KeyCode.A))
       {
           anim.SetInteger("State", 0);
       }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            if(scoreValue == 4)
            {
                transform.position = new Vector3(42.0f, 4.0f, 0.0f);
                lives = 3;
            }
            if(scoreValue == 8)
            {
                musicSource.clip = musicClipOne;
                musicSource.Play();
                musicSource.loop = false;

                winTextObject.SetActive(true);
            }

            score.text = "Coins: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if(collision.collider.tag == "Enemy")
        {
            livesText.text = "Lives: " + lives.ToString();
            lives -= 1;

            Destroy(collision.collider.gameObject);
        }
        
        livesText.text = "Lives: " + lives.ToString();
        if(lives == 0)
        {
            livesTextObject.SetActive(true);
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }

            if (jumping)
            {
                jumping = false;
                anim.SetBool("IsJumping", jumping);           
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                jumping = true;
                anim.SetBool("IsJumping", jumping);
            }
            
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        }
    }
}
