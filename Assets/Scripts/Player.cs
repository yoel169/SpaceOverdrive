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

    [Header("Sound")]
    [SerializeField] AudioClip dieSound;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 10)] float dieSoundVolume = 0.75f;
    [SerializeField] [Range(0, 10)] float shootVolume = 0.25f;

    [Header("Shields")]
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] float shieldDuration = 3f;
    [SerializeField] float rechargeRate = 0.2f;
    [SerializeField] float timeToRecharge = 6f;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringperiod = 0.1f;

    //shields
    float currentShields;
    GameObject shieldObject;
    Coroutine shieldsC;
    Coroutine rechargeShieldsC;

    //health and life
    int currentHealth;
    int currentLives;

    //positions
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    //UI bars
    HealthBar healthBar;
    ShieldBar shieldBar;

    // Start is called before the first frame update
    void Start()
    {
        SetupMoveBoundaries();
        currentHealth = maxHealth;
        currentLives = startingLives;

        healthBar = FindObjectOfType<HealthBar>();
        healthBar.SetMaxValue(maxHealth);

        shieldBar = FindObjectOfType<ShieldBar>();
        shieldBar.SetMaxValue(shieldDuration);

        currentShields = shieldDuration;

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

        if (Input.GetKeyDown(KeyCode.C ) && currentShields > 0){

            if (rechargeShieldsC != null) { StopCoroutine(rechargeShieldsC); }
            shieldsC = StartCoroutine(TurnOnShields());
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            if(currentShields > 0)
            {
                StopCoroutine(shieldsC);
                Destroy(shieldObject);
                StartCoroutine(RechargeShields());
            }
           
        }
    }

    private IEnumerator TurnOnShields ()
    {

        Vector3 pos = transform.position; // + new Vector3(0,60,0);

        shieldObject = Instantiate(shieldPrefab, pos, Quaternion.identity, transform);

        while (currentShields > 0)
        {
           
            currentShields -= 0.2f;
            shieldBar.SetSlider(currentShields);
            yield return new WaitForSeconds(0.2f);
        }

        currentShields = 0;

        Destroy(shieldObject);

        yield return new WaitForSeconds(timeToRecharge);
        StartCoroutine(RechargeShields());
    }

    private IEnumerator RechargeShields()
    {

        while (currentShields < shieldDuration)
        {
            yield return new WaitForSeconds(rechargeRate);
            currentShields += rechargeRate;
            shieldBar.SetSlider(currentShields);

        }

        currentShields = shieldDuration;
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

        healthBar.SetSlider(currentHealth);
 
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
