using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float enemySpeed = 5.0f;
    [SerializeField]
    private float maxTime = 60.0f;
    private float timer = 0.0f;
    private int score;
    private int round;
    [Range(0, 10)]
    public int numOfEnemiesRequired;
    private int numOfEnemiesHit;
    private EnemySpawner enemySpawner;
    private int updatedScore;

    // Dog Animator
    private Animator dogAnimator;

    // UI
    public GameObject btnPause;
    public GameObject btnPlay;
    public GameObject unpauseScreen;
    public Text gameOverScoreText;
    public GameObject GameOverScreen;
    public Text yourNameText;
    public Image[] skullImages;
    public Sprite blackSkull;
    public Sprite redSkull;
    public Text scoreText;
    private string formatScore = "0000000000";
    public Text roundText;
    private string formatRound = "00";
    public Image[] timerImages;
    public Sprite tick;

    // Reference to audio sources
    AudioSource audioBackgroundMusic;
    AudioSource audioSplat;

    // High Score Management
    public string[] highScoreNames;
    public int[] highScores;

    // Variant
    private bool variantToggle = false;
    private float variantTimer = 0.0f;
    public float variantActiveTime = 5.0f;
    public float variantShootSpeed = 0.63f;
    private float autoShootTimer = 0.0f;
    private bool autoShoot = false;
    private float intervalMultiplierTemp;

    private void Awake()
    {
        highScores = new int[6];
        highScoreNames = new string[6];

        for (int i = 0; i < highScores.Length; i++)
        {
            highScores[i] = PlayerPrefs.GetInt("Highscores" + i);
            highScoreNames[i] = PlayerPrefs.GetString("HighScoreNames" + i);
        }
    }

    private void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        intervalMultiplierTemp = enemySpawner.GetIntervalMultiplier();

        dogAnimator = GameObject.Find("Dog").GetComponent<Animator>();

        // Get audio references
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioBackgroundMusic = audioSources[0];
        audioSplat = audioSources[1];
    }

    // Update is called once per frame
    void Update()
    {
        // Variant-Special
        if (Input.GetKeyUp(KeyCode.Space))
        {
            variantToggle = true;
        }

        if (variantToggle)
        {
            enemySpawner.SetIntervalMuliplier(intervalMultiplierTemp / 2);
            variantTimer += Time.deltaTime;
            autoShootTimer += Time.deltaTime;

            if(variantTimer > variantActiveTime)
            {
                variantTimer = 0.0f;
                variantToggle = false;
            }else if(autoShootTimer > variantShootSpeed)
            {
                autoShoot = true;
                autoShootTimer = 0.0f;
            }
        }
        else
        {
            enemySpawner.SetIntervalMuliplier(intervalMultiplierTemp);
        }

        // UI
        scoreText.text = score.ToString(formatScore);
        roundText.text = round.ToString(formatRound);

        if (dogAnimator.GetCurrentAnimatorStateInfo(0).IsName("Laugh"))
        {
            dogAnimator.SetBool("New Bool", false); // Have to use name "New Bool" - there is some glitch in Unity that will not let me save parameter names.
        }

        if (!GameOverScreen.activeInHierarchy)
        {
            HandleUISkulls();
            HandleUITimer();
            HandleShooting();
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) || autoShoot)
        {
            if (autoShoot)
            {
                autoShoot = false;
            }

            audioSplat.Play();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);
            
            int enemiesHit = 0;

            if (hits == null || hits.Length == 0)
            {
                dogAnimator.SetBool("New Bool", true);

                updatedScore = variantToggle ? -2 : -1;
                UpdateScore(updatedScore);
            }

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.tag == "Enemy")
                {
                    enemiesHit += 1;
                    Destroy(hit.collider.gameObject);
                }else if(hit.collider.tag == "Witch")
                {
                    hit.collider.GetComponentInParent<Witch>().DecrementHealth(1);
                }
            }

            UpdateEnemiesHit(enemiesHit);
            
            switch (enemiesHit)
            {
                case 0: // handled above
                    break;
                case 1:
                    updatedScore = variantToggle ? 1 : 3;
                    UpdateScore(updatedScore);
                    break;
                default:
                    updatedScore = variantToggle ? 1 : 3;
                    UpdateScore(enemiesHit * updatedScore + 5);
                    break;
            }

            if(numOfEnemiesHit >= numOfEnemiesRequired)
            {
                NextRound();
            }
        }
    }

    private void NextRound()
    {
        round += 1;
        numOfEnemiesHit = 0;
        maxTime *= 0.9f;
        timer = 0.0f;
        enemySpeed *= 1.1f;
        GetComponent<EnemySpawner>().SetUp();
    }

    private void HandleUISkulls()
    {
        // Credit: Adapted health system from https://www.youtube.com/watch?v=3uyolYVsiWc
        for (int i = 0; i < skullImages.Length; i++)
        {
            if (i < numOfEnemiesHit)
            {
                skullImages[i].sprite = redSkull;
            }
            else
            {
                skullImages[i].sprite = blackSkull;
            }

            if (i < numOfEnemiesRequired)
            {
                skullImages[i].enabled = true;
            }
            else
            {
                skullImages[i].enabled = false;
            }
        }
    }

    private void HandleUITimer()
    {
        timer += Time.deltaTime;

        if(timer > maxTime)
        {
            GameOver();
        }

        // Credit: Adapted health system from https://www.youtube.com/watch?v=3uyolYVsiWc
        for (int i = 0; i < timerImages.Length; i++)
        {
            if (i < (timerImages.Length - (int)(timer * (float)timerImages.Length / maxTime)))
            {
                timerImages[i].enabled = true;
            }
            else
            {
                timerImages[i].enabled = false;
            }
        }
    }

    public void GameOver()
    {
        gameOverScoreText.text = score.ToString(formatScore);
        GameOverScreen.SetActive(true);
    }

    public void CloseButton()
    {
        Application.Quit();
    }

    public void HomeButton()
    {
        SceneManager.LoadScene("MainMenu");
        UpdateHighScores(yourNameText.text, score);
    }

    public void UpdateHighScores(string name, int score)
    {
        string replaceName;
        int replaceScore;

        for (int i = 0; i < highScores.Length - 1; i++)
        {
            if(score >= highScores[i])
            {
                replaceName = highScoreNames[i];
                replaceScore = highScores[i];

                highScoreNames[i] = name;
                highScores[i] = score;

                name = replaceName;
                score = replaceScore;
            }
        }

        for (int i = 0; i < highScores.Length; i++)
        {
            PlayerPrefs.SetInt("Highscores" + i, highScores[i]);
            PlayerPrefs.SetString("HighScoreNames" + i, highScoreNames[i]);
        }
    }

    public void UpdateEnemiesHit(int numberHit)
    {
        numOfEnemiesHit += numberHit;
    }

    public int GetNumOfEnemiesHit()
    {
        return numOfEnemiesHit;
    }

    public void UpdateScore(int points)
    {
        score += points * 100;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetRound()
    {
        return round;
    }

    public float GetEnemySpeed()
    {
        return enemySpeed;
    }

    public float GetMaxTime()
    {
        return maxTime;
    }

    public void PauseGame()
    {
        btnPause.SetActive(false);
        btnPlay.SetActive(true);
        unpauseScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void UnPauseGame()
    {
        btnPause.SetActive(true);
        btnPlay.SetActive(false);
        unpauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
