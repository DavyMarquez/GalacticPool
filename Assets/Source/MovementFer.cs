using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFer : MonoBehaviour
{
    /*
     * The good G is 10^-11, but we are going to use 10^0, now 1.0 in mass equals 1000 tons
     */
    static double G = 6.67408 * Math.Pow(10.0f, 0);

    public Vector2 startSpeed;

    public float mass = 1.0f;

    public Vector2 speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = startSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y) 
            + speed * Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Orbit"))
        {
            GameObject obj = collision.gameObject;

            Orbit orb = obj.GetComponent<Orbit>();

            Vector2 planetDirection = obj.transform.position - transform.position;

            double force = G * (mass * orb.mass) / planetDirection.sqrMagnitude;

            double acceleration = force / mass;

            Vector2 accelVec = (float)acceleration * planetDirection.normalized;

            speed = speed + accelVec * Time.deltaTime;
            

        }
    }
}

