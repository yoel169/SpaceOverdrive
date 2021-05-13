using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    //config
    [Header("Player Movement")]
    [SerializeField] int maxHealth = 300;
    [SerializeField] int startingLives = 3;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringperiod = 0.1f;

    [Header("Sound")]
    [SerializeField] AudioClip dieSound;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 10)] float dieSoundVolume = 0.75f;
    [SerializeField] [Range(0, 10)] float shootVolume = 0.25f;


    int currentHealth;
    int currentLives;

    //
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetupMoveBoundaries();
        currentHealth = maxHealth;
        currentLives = startingLives;
        StartCoroutine(FireContinuously());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //Fire();

        if (Input.GetButtonDown("Cancel") == true)
        {
            Application.Quit();
        }
    }

    private void Fire()
    {
   

        /*if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }

        if (Input.GetButtonUp("Fire1"))
        {
           StopCoroutine(firingCoroutine);
        }*/



    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            if (CompareTag("Fighter"))
            {
                GameObject laser1 = Instantiate(laserPrefab, (transform.position - new Vector3(0.45f, 0.1f, 0f)), Quaternion.identity) as GameObject;
                GameObject laser2 = Instantiate(laserPrefab, (transform.position + new Vector3(0.45f, - 0.1f, 0f)), Quaternion.identity) as GameObject;

                laser1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
                laser2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

                AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootVolume);
                yield return new WaitForSeconds(projectileFiringperiod);
            }
            else
            {
                GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;

                laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

                AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootVolume);
                yield return new WaitForSeconds(projectileFiringperiod);
            }
           
        }
        
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();

        if (!damageDealer) { return; }

        currentHealth -= damageDealer.getDamage();

        damageDealer.Hit();

        if (currentHealth <= 0)
        {
            currentLives--;

            if (currentLives <= 0)
            {
                Die();
            }
            else{

                currentHealth = maxHealth;

            }
        }
 
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    private void Die()
    { 
        FindObjectOfType<SceneSelector>().LoadGameOver();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(dieSound, Camera.main.transform.position, dieSoundVolume);
    }

    private void SetupMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }


    public int GetCurrentLives()
    {
        return currentLives;
    }
}
