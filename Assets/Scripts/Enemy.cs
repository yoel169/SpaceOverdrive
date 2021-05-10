using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreToGive = 5;

    [Header("Shooting")]
    float shotCounter;
    [SerializeField] float minBetweenShots = 0.2f;
    [SerializeField] float maxBetweenShots = 3f;
    [SerializeField] float projectileSpeed =  - 10f;
    [SerializeField] GameObject laserPrefab;

    [Header("Explosion")]
    [SerializeField] GameObject particleExplosionPrefab;
    [SerializeField] float durationOfExplosion = 1f;

    [Header("Sound")]
    [SerializeField] AudioClip dieSound;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0,10)] float dieSoundVolume = 0.75f;  
    [SerializeField] [Range(0, 10)] float shootVolume = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minBetweenShots, maxBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        shotCounter -= Time.deltaTime;

        if(shotCounter <= 0f)
        {
            GameObject laser = Instantiate(
                laserPrefab, transform.position, 
                Quaternion.identity) as GameObject;

            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootVolume);
            shotCounter = Random.Range(minBetweenShots, maxBetweenShots);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();

        if (!damageDealer) { return; }

        health -= damageDealer.getDamage();

        damageDealer.Hit();

        if (health <= 0)
        {
            Die();
            
        }

    }

    private void Die()
    {

        FindObjectOfType<GameSession>().AddToScore(scoreToGive);

        AudioSource.PlayClipAtPoint(dieSound, Camera.main.transform.position, dieSoundVolume);

        Destroy(gameObject);

        GameObject explosion = Instantiate(particleExplosionPrefab, transform.position, transform.rotation);

        Destroy(explosion, durationOfExplosion);

    }
}
