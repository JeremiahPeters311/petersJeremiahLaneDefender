using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerControls : MonoBehaviour
{
    //player controls
    InputActions Controls;
    public int Move;
    public bool Fire;

    public bool CanShoot = true;

    public GameObject Player;
    public GameObject ShotReference;
    public GameObject ExplosionReference;
    public GameObject BlastReference;

    public AudioClip DestroySound;

    public TMP_Text LivesText;

    private void Awake()
    {
        //player controls
        Controls = new InputActions();

        Controls.Gameplay.Up.performed += ctx => Move += 1;
        Controls.Gameplay.Up.canceled += ctx => Move -= 1;

        Controls.Gameplay.Down.performed += ctx => Move -= 1;
        Controls.Gameplay.Down.canceled += ctx => Move += 1;

        Controls.Gameplay.Fire.performed += ctx => Fire = true;
        Controls.Gameplay.Fire.canceled += ctx => Fire = false;

        CanShoot = true;

    }
    void Update()
    {
        //allow movement up and down
        Vector3 newPos = transform.position;
        newPos.y += 8 * Move * Time.deltaTime;

        //prevent player from going out of bounds
        newPos.y = Mathf.Clamp(newPos.y, -4, 4);

        //update the position
        transform.position = newPos;

        //allow shooting
        if (Fire == true)
        {
            //prevent rapid fire
            if (CanShoot == true)
            {
                //shoot
                CanShoot = false;
                StartCoroutine(ShotTimer());
                Vector3 BulletStart = transform.position;
                BulletStart.x = transform.position.x + 1.35f;
                BlastReference.SetActive(true);
                Instantiate(ShotReference, BulletStart, Quaternion.identity);
            }
        }
    }
    private IEnumerator ShotTimer()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        BlastReference.SetActive(false);
        yield return new WaitForSecondsRealtime(0.15f);
        CanShoot = true;
        yield return 0;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameController gc = GameObject.FindObjectOfType<GameController>();
        //death
        gc.PlayerLives--;
        LivesText.text = "Lives: " + gc.PlayerLives.ToString();
        Instantiate(ExplosionReference, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(DestroySound, new Vector3(0.0f, 0.0f, 0.0f));
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
