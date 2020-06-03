using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
    /*
     * The good G is 10^-11, but we are going to use 10^0. The gravity force it so tiny, so we
     * up this value to have masses like 1 or 20 and have the gravity effect
     */
    static double G = 6.67408 * Math.Pow(10.0f, 0);

    // Speed of the movement
    private Vector2 speed = new Vector2(0.0f, 0.0f);


    public Vector2 Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    // Mass of the object
    public float mass = 1.0f;
    public float Mass {
        get { return mass; }
        set { mass = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = new Vector2(transform.position.x, transform.position.y) + speed * Time.deltaTime;
        transform.position = new Vector3(newPos.x, newPos.y, 0.0f);
        
        /* Vector3 start = gameObject.transform.position;
        Vector3 dir = new Vector3(speed.normalized.x, speed.normalized.y, 0.0f);
        Vector3 end = start + dir * 1;  
        Debug.DrawLine(start, end, Color.red, 0.1f, true); */
    }

    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Orbit"))
        {
            GameObject obj = collision.gameObject;

            Orbit orb = obj.GetComponent<Orbit>();

            // vector from the center of the ball to the object
            Vector2 planetDirection = obj.transform.position - transform.position;

            // force applied
            double force = G * (mass * orb.mass) / planetDirection.sqrMagnitude;

            // extract the acceleration from the force F = m a
            double acceleration = force / mass;
            
            // put the acceleration value in 2D by multiplying for the direction of the force
            Vector2 accelVec = (float)acceleration * planetDirection.normalized;
        
            // apply the acceleration
            speed = speed + accelVec * Time.deltaTime;


        }
    }

}
