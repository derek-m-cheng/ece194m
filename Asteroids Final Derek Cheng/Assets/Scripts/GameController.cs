using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public GameObject spaceshipPrefab;
    public GameObject[] lifeIcons;

    public int maxAsteroids = 2;
    public int numAsteroids;

    public float timeDied;

    private int numLivesLeft;
    private int maxLives = 4;
    private float respawnTime = 3f;

    private int myScore = 0;
    private Score scoreText;
    private GameObject spaceship;

    private GameObject gameOverSign;
    private GameObject levelClearedSign;
    private float minCollisionRadius = 2.0f;

    private bool gameFinished = false;
    private bool gameWon = false;
    private float finishTime;

    private void Awake()
    {
        numLivesLeft = maxLives;
        myScore = 0;
        scoreText = FindObjectOfType<Score>();
        scoreText.UpdateScore(myScore);
        gameOverSign = GameObject.Find("GameOver");
        levelClearedSign = GameObject.Find("LevelCleared");
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        numAsteroids = maxAsteroids;
        //spawn asteroids
        for (int i = 0; i < numAsteroids; i++)
        {
            spawnAsteroid();
        }
        //spwn the ship
        spawnSpaceship();

        //hide game over sign
        Assert.IsNotNull(gameOverSign);
        gameOverSign.SetActive(false);

        //hide level cleared sign
        Assert.IsNotNull(levelClearedSign);
        levelClearedSign.SetActive(false);

        gameFinished = false;
        gameWon = false;

    }

    private void spawnAsteroid()
    {
        bool valid;
        GameObject newAsteroid;
        do
        {
            newAsteroid = Instantiate(asteroidPrefab);
            newAsteroid.GetComponent<Asteroid>().setGameController(this);
            valid = CheckTooCloseToAsteroid(newAsteroid);
        } while (valid == false);

        return;

    }

    private void spawnSpaceship()
    {
        bool valid;

        Assert.IsNull(spaceship);
        do
        {
            spaceship = Instantiate(spaceshipPrefab);
            valid = CheckTooCloseToAsteroid(spaceship);
        } while (valid == false);

        spaceship.GetComponent<Spaceship>().setGameController(this);

        numLivesLeft -= 1;

        return;

    }

    public void IncreaseScore()
    {
        myScore += 10;
        scoreText.UpdateScore(myScore);
    }


    private bool CheckTooCloseToAsteroid(GameObject testObject)
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        foreach(GameObject asteroid in asteroids)
        {
            if (asteroid != testObject)
            {
                //check if too close
                if (Vector3.Distance(testObject.transform.position, asteroid.transform.position) < minCollisionRadius)
                {
                    Destroy(testObject);
                    return false;
                }
            }
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check to see if spaceship has died
        if (spaceship == null)
        {
            if (Time.time - timeDied > respawnTime)
            {
                //check to see if lives left
                if (numLivesLeft > 0)
                {
                    spawnSpaceship();

                    //update life icons
                    Destroy(lifeIcons[numLivesLeft]);
                }
                else
                {
                    gameOverSign.SetActive(true);
                }
            }
            
        }
        

        //check to see if I won
        if ((numAsteroids == 0) && (gameWon == false))
        {
            if (gameFinished)
            {
                if (Time.time - finishTime > respawnTime)
                {
                    levelClearedSign.SetActive(true);
                    gameFinished = false;
                    gameWon = true;
                    StartCoroutine(Pause());
                }
            }
            else
            {
                gameFinished = true;
                finishTime = Time.time;
            }
     
        }

        Assert.IsTrue(numAsteroids >= 0);
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(3f);
        maxAsteroids = maxAsteroids * 2;

        if (maxAsteroids > 16)
        {
            maxAsteroids = 16;
        }
        Destroy(spaceship);
        spaceship = null;
        numLivesLeft++;
        InitializeLevel();
    }
}
