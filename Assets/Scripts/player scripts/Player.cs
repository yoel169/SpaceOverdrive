using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //config
    [Header("Player Movement")]
    [SerializeField] int shipIndex = 0;
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

    [Header("Overdrive")]
    [SerializeField] int oDuration = 4;
    [SerializeField] int oScoreNeeded = 200;

    [Header("Seeking Properties")]
    [SerializeField] private float force = 10f;
    [SerializeField] private float rotationForce = 200f;

    [Header("Audio")]
    [SerializeField] AudioClip lifeDown;

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
    [Header("UI Bars")]
    [SerializeField]  HealthBar healthBar;
    [SerializeField]  ShieldBar shieldBar;
    [SerializeField] OverdriveBar driveBar;

    //overdrive
    float currentOverdrive;
    bool driveActive = false;
    bool homing = false;
    Coroutine normalFire;

    // Start is called before the first frame update
    void Start()
    {  
        SetupMoveBoundaries();

        currentHealth = maxHealth;
        currentLives = startingLives;

        healthBar.SetMaxValue(maxHealth);
        healthBar.SetSlider(currentHealth);

        shieldBar.SetMaxValue(shieldDuration);
        shieldBar.SetSlider(shieldDuration);

        currentShields = shieldDuration;
        currentOverdrive = 0;

        driveBar.SetMaxValue(oScoreNeeded);
        driveBar.SetSlider(currentOverdrive);
        
        normalFire = StartCoroutine(FireContinuously());
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space)) {

            if (currentOverdrive >= oScoreNeeded && !driveActive)
            {

                print("overdrive activating");
                driveActive = true;

                if (shipIndex == 0)
                {
                    StartCoroutine(FalconOverdrive());

                }
                else if (shipIndex == 1)
                {

                    StartCoroutine(EagleOverdrive());

                }
                else
                {

                }
            }
            else
            {
                print("overdrive not ready " + currentOverdrive);
            }
        }
    
        
        if (Input.GetKeyDown(KeyCode.C) && currentShields > 0 && !driveActive)
        {

            if (rechargeShieldsC != null) { StopCoroutine(rechargeShieldsC); }
            shieldsC = StartCoroutine(TurnOnShields());
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            if (currentShields > 0)
            {
                StopCoroutine(shieldsC);
                Destroy(shieldObject);
                StartCoroutine(RechargeShields());
            }

            normalFire = StartCoroutine(FireContinuously());

        }
        
    }

    private IEnumerator TurnOnShields ()
    {

        StopCoroutine(normalFire);

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

    public int GetShipType()
    {
        return shipIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();

        if (!damageDealer) { return; }

        currentHealth -= damageDealer.getDamage();

        damageDealer.Hit();

        if (currentHealth <= 0)
        {
            AudioSource.PlayClipAtPoint(lifeDown, Camera.main.transform.position);
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
        FindObjectOfType<GameSession>().Lose();
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

    public void IncreaseLives()
    {
        currentLives++;
    }

    public void IncreaseHealth(int health)
    {
        if(currentHealth + health >= maxHealth)
        {
            currentHealth = maxHealth;

        }else if(currentHealth + health < maxHealth)
        {
            currentHealth += health;
        }
       
    }

    public void IncreaseSuper(int sc)
    {
        
        if(currentOverdrive <= oScoreNeeded && ! driveActive)
        {                  
            float temp = currentOverdrive += sc;

            if(temp < oScoreNeeded)
            {
                currentOverdrive += sc;
            }
            else
            {
                currentOverdrive = oScoreNeeded;
            }

            driveBar.SetSlider(currentOverdrive);
            print("added points: " + currentOverdrive);

        }
              
    }

    private IEnumerator EagleOverdrive()
    {
        //turn on termp shields
        Vector3 pos = transform.position;

        GameObject tempShields = Instantiate(shieldPrefab, pos, Quaternion.identity, transform);

        float firingTemp = projectileFiringperiod;

        projectileFiringperiod /= 3;

        yield return new WaitForSeconds(oDuration);

        projectileFiringperiod = firingTemp;

       Destroy(tempShields);

        driveActive = false;
    }

    private IEnumerator FalconOverdrive()
    {

        StopCoroutine(normalFire);

        Vector3 pos = transform.position;

        GameObject tempShields = Instantiate(shieldPrefab, pos, Quaternion.identity, transform);

        float totalTime = 0f;

        print("Starting homing shots");

        homing = true;

        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            HomingLaser ll = laser.GetComponent<HomingLaser>();

            ll.Launch(force, rotationForce);

            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootVolume);

            //add to total time
            totalTime += projectileFiringperiod;

            //if total time is done break
            if (totalTime >= oDuration)
            {
                driveBar.SetSlider(0);
                currentOverdrive = 0;
                print("stopping homing shots");
                break;
            }
            else
            {
                currentOverdrive -= projectileFiringperiod;
                driveBar.SetSlider(currentOverdrive);
            }

            yield return new WaitForSeconds(projectileFiringperiod);
        }

        Destroy(tempShields);

        homing = false;
        driveActive = false;

        normalFire = StartCoroutine(FireContinuously());

    }

}
