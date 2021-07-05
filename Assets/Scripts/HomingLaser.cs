using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingLaser : MonoBehaviour
{

    private Transform target;
    private float force = 10f;
    private float rotationForce =200f;
    [SerializeField] private float range = 30f;
    [SerializeField] private Rigidbody2D rb;

    bool homing = false;
    bool active = false;
    GameObject enemies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.Find("Enemies");    
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            FindClosestEnemy();
        }
        
    }

    private void FindClosestEnemy()
    {
        if (enemies.transform.childCount > 0)
        {
            try
            {
                for (int i = 0; i < enemies.transform.childCount; i++)
                {
                    Transform child = enemies.transform.GetChild(i);

                    if (Vector3.Distance(transform.position, child.position) <= range)
                    {
                        target = child;
                        homing = true;
                        break;
                    }
                }
            }
            catch
            {
                print("failed to find child");
            }
            
        }
        else
        {
            print("no enemies found");
        }
    }

    public void Launch(float f, float rf)
    {
        force = f;
        rotationForce = rf;

        rb.velocity = new Vector2(0, force);
        active = true;
    }

    private void FixedUpdate()
    {
        if (homing && target != null)
        {
            try
            {
                print("trying to track " + target.ToString());

                Vector2 direction = (Vector2)target.position - rb.position;

                direction.Normalize();

                float rotateAmount = Vector3.Cross(direction, transform.up).z;

                rb.angularVelocity = -rotateAmount * rotationForce;

                rb.velocity = transform.up * force;
            }
            catch
            {
                print("Failed to track target");
                homing = false;
            }
          
        }
       
    }
}
