using System.Collections;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    InputActions Controls;
    public bool Restart = false;
    public int PlayerScore = 0;
    public static int PlayerHighScore = 0;
    public int PlayerLives = 3;
    public GameObject InfantryReference;
    public GameObject TankReference;
    public GameObject MdTankReference;

    public TMP_Text LivesText;
    public TMP_Text ScoreText;
    public TMP_Text HighScoreText;
    public TMP_Text GameOverText;

    string highScoreKey = "HighScore";

    public AudioClip GameOver;

    private void Awake()
    {
        Controls = new InputActions();
        Controls.Gameplay.Restart.performed += ctx => Restart = true;
    }
    private void Start()
    {
        Invoke("SpawnEnemy", 0f);
        PlayerHighScore = PlayerPrefs.GetInt(highScoreKey, 0);
    }
    void Update()
    {
        //update ui
        if (PlayerScore >= PlayerHighScore)
        {
            PlayerHighScore = PlayerScore;
            PlayerPrefs.SetInt(highScoreKey, PlayerScore);
            PlayerPrefs.Save();
        }
        ScoreText.text = "Score: " + PlayerScore.ToString();
        HighScoreText.text = "High Score: " + PlayerHighScore.ToString();
        LivesText.text = "Lives: " + PlayerLives.ToString();


        if (Restart == true)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        if (PlayerLives <= 0)
        {
            //game over
            StartCoroutine(Death());
        }
    }
    void SpawnEnemy()
    {
        int random = Random.Range(1, 5);
        if (random == 1)
        {
            //spawn infantry in a random lane
            random = Random.Range(-2, 3);
            Instantiate(InfantryReference, new Vector3(8, random * 2, 0), Quaternion.identity);
        }
        else if (random == 2)
        {
            //spawn tank in a random lane
            random = Random.Range(-2, 3);
            Instantiate(TankReference, new Vector3(8, random * 2, 0), Quaternion.identity);
        }
        else if (random == 3)
        {
            //spawn mdtank in a random lane
            random = Random.Range(-2, 3);
            Instantiate(MdTankReference, new Vector3(8, random * 2, 0), Quaternion.identity);
        }
        Invoke("SpawnEnemy", 1f);
    }
    IEnumerator Death()
    {
        //turn off music
        //This.SetActive(false);

        //wait for explosion to finish
        yield return new WaitForSecondsRealtime(0.75f);

        //game over
        AudioSource.PlayClipAtPoint(GameOver, new Vector3(0.0f, 0.0f, 0.0f));
        GameOverText.gameObject.SetActive(true);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(3f);

        //restart
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    private void OnEnable()
    {
        Controls.Gameplay.Enable();
    }
    private void OnDisable()
    {
        Controls.Gameplay.Disable();
    }
}
