using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public int MovementSpeed = 3;
    public int CurrentHp = 5;
    public int Score;
    public GameObject PlayerReference;
    public GameObject EnemyReference;
    public GameObject ExplosionReference;
    public AudioClip DestroySound;
    private void Start()
    {
        if (gameObject.name == "bhinfantry(Clone)")
        {
            MovementSpeed = 3;
            CurrentHp = 1;
            Score = 1;
        }
        else if (gameObject.name == "bhtank(Clone)")
        {
            MovementSpeed = 2;
            CurrentHp = 3;
            Score = 3;
        }
        else if (gameObject.name == "bhmdtank(Clone)")
        {
            MovementSpeed = 1;
            CurrentHp = 5;
            Score = 5;
        }
        //check what enemy type, adjust speed and hp accordingly
    }
    void Update()
    {
        GameController gc = GameObject.FindObjectOfType<GameController>();
        //move slowly to the left
        Vector3 pos = transform.position;
        pos.x -= MovementSpeed * Time.deltaTime;
        if (transform.position.x < -10.5)
        {
            //lose a life
            gc.PlayerLives--;
            Destroy(gameObject);
        }
        transform.position = pos;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameController gc = GameObject.FindObjectOfType<GameController>();
        if (collision.gameObject.name == "shot(Clone)")
        {
            CurrentHp--;
            StartCoroutine(StunTimer());
        }
        else if (collision.gameObject.name == "Player")
        {
            Destroy(gameObject);
        }
        if (CurrentHp <= 0)
        {
            //death
            gc.PlayerScore += Score;
            AudioSource.PlayClipAtPoint(DestroySound, new Vector3(0.0f, 0.0f, 0.0f));
            Instantiate(ExplosionReference, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    IEnumerator StunTimer()
    {
        MovementSpeed = 0;
        EnemyReference.GetComponent<SpriteRenderer>().color = new Color(30, 30, 30);
        yield return new WaitForSecondsRealtime(0.1f);
        if (gameObject.name == "bhinfantry(Clone)")
        {
            MovementSpeed = 3;
        }
        else if (gameObject.name == "bhtank(Clone)")
        {
            MovementSpeed = 2;
        }
        else if (gameObject.name == "bhmdtank(Clone)")
        {
            MovementSpeed = 1;
        }
        yield return 0;
    }
}
