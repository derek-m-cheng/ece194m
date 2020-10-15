using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    private float turnSpeed = 180;
    private float thrust = 0.000015f;
    private float bulletSpeed = 20f;

    private Vector3 shipDirection = new Vector3(0, 1, 0);

    private Rigidbody2D rb;

    private float maxX = 9.2f;
    private float maxY = 5.2f;
    private float maxSpeed = 2.0f;

    private GameController gameController;

    //audio stuff
    private AudioSource audioSource;
    public AudioClip shootingSoundFX;
    public AudioClip thrustersFX;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();



        gameObject.tag = "Spaceship";
        gameObject.name = "Spaceship";
        //random position
        transform.position = new Vector3(Random.Range(-maxX + 2f, maxX - 2f), Random.Range(-maxY + 2f, maxY -2f), 0);
    }

    public void setGameController(GameController _gameController)
    {
        gameController = _gameController;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float turnAngle;
        //keyboard

        //ship motion
        if (Input.GetKey("a"))
        {
            //turn left
            turnAngle = turnSpeed * Time.deltaTime;
            transform.Rotate(0, 0, turnAngle);
            shipDirection = Quaternion.Euler(0, 0, turnAngle) * shipDirection;

        }

        if (Input.GetKey("d"))
        {
            //turn right
            turnAngle = -turnSpeed * Time.deltaTime;
            transform.Rotate(0, 0, turnAngle);
            shipDirection = Quaternion.Euler(0, 0, turnAngle) * shipDirection;
        }

        if (Input.GetKey("w"))
        {
            //thrust
            rb.AddForce(shipDirection * thrust * 2);
        }

        if (Input.GetKeyDown("w"))
        {
            //keep playing sound
            audioSource.clip = thrustersFX;
            audioSource.Play();

        }

        if (Input.GetKeyUp("w"))
        {
            //stop playing sound
            audioSource.clip = thrustersFX;
            audioSource.Stop();


        }

        //ship firing
        if (Input.GetKeyDown("space"))
        {
            audioSource.PlayOneShot(shootingSoundFX);
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation * Quaternion.Euler(0,0,90);
            bullet.GetComponent<Rigidbody2D>().velocity = shipDirection * bulletSpeed;
        }

        //throttle max speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        //if ship goes off edge, wrap around
        if (transform.position.x < -maxX)
        {
            transform.position = new Vector2(maxX, transform.position.y);
        }
        else if (transform.position.x > maxX)
        {
            transform.position = new Vector2(-maxX, transform.position.y);
        }
        if (transform.position.y < -maxY)
        {
            transform.position = new Vector2(transform.position.x, maxY);
        }
        else if (transform.position.y > maxY)
        {
            transform.position = new Vector2(transform.position.x, -maxY);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            gameController.timeDied = Time.time;
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
