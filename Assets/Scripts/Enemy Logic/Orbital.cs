using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbital : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreToGive = 5;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int type = 0;
    [SerializeField] int diff = 0;

    [Header("Shooting")]
    float shotCounter;
    //[SerializeField] float minBetweenShots = 0.2f;
    //[SerializeField] float maxBetweenShots = 3f;
    [SerializeField] float timeBetweenShotsSessions = 5f;
    [SerializeField] float timeBetweenShots = 0.05f;
    [SerializeField] float shotDuration = 3f;
    [SerializeField] float projectileSpeed = -10f;
    [SerializeField] GameObject laserPrefab;

    [Header("Explosion")]
    [SerializeField] GameObject particleExplosionPrefab;
    [SerializeField] float durationOfExplosion = 1f;

    [Header("Sound")]
    [SerializeField] AudioClip dieSound;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip powerUpSound;
    [SerializeField] [Range(0, 10)] float dieSoundVolume = 0.75f;
    [SerializeField] [Range(0, 10)] float shootVolume = 0.25f;

    Player player;
    Vector3 vectorToPlayer;
    bool isFiring = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        shotCounter = timeBetweenShotsSessions;
    }

    // Update is called once per frame
    void Update()
    { 

        if (!CompareTag("Green"))
        {
            shotCounter -= Time.deltaTime;

            if(shotCounter <= 0 && !isFiring)
            {
                StartCoroutine(Shoot());
                isFiring = true;
            }
            
            if(!isFiring)
            {
                LookAt2D();
            }   
        }
    }

    private IEnumerator Shoot()
    {
        float counter = 0f;
        AudioSource.PlayClipAtPoint(powerUpSound, Camera.main.transform.position, shootVolume);

        yield return new WaitForSeconds(3);

        while (counter < shotDuration)
        {
            GameObject laser = Instantiate(
               laserPrefab, transform.position,
               transform.rotation) as GameObject;

            //laser.transform.up =  - vectorToPlayer;
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootVolume);

            laser.GetComponent<Rigidbody2D>().velocity = -laser.transform.up *   - projectileSpeed;

            yield return new WaitForSeconds(timeBetweenShots);
            counter += timeBetweenShots;

        }

        shotCounter = timeBetweenShotsSessions;
        isFiring = false;
    }

    private void LookAt2D()
    {
        if(player != null)
        {
            vectorToPlayer = (player.transform.position - transform.position).normalized;
            transform.up = -vectorToPlayer;

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

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}
