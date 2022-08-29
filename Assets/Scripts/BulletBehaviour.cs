using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public AudioClip ShootSound;
    private void Start()
    {
        AudioSource.PlayClipAtPoint(ShootSound, new Vector3(0.0f, 0.0f, 0.0f));
    }
    void Update()
    {
        //move quickly to the right
        Vector3 pos = transform.position;
        pos.x += 8 * Time.deltaTime;
        transform.position = pos;

        //destroy when offscreen
        if (transform.position.x > 10.5)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //death when it collides
        Destroy(gameObject);
    }
}
