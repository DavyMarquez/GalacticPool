using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Bounce : MonoBehaviour
{
    [Range(0, 1)]
    public float restitution = 1.0f;

    private Vector2 _newSpeed = new Vector2(0.0f, 0.0f);

    // Whether or not there is a collision
    private bool _collided = false;

    // This GameObject movement component
    private Movement _movement;

    public float mass = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        _movement = gameObject.GetComponent<Movement>();
        Assert.IsTrue(_movement, "No Movement component on the game object");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (_movement && _collided)
        {
            _movement.speed = _newSpeed * restitution;
            _collided = false;
        }
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Assert.IsTrue(other.gameObject.tag == "Bounce", "No Bounce component on the game object");
        Bounce bounce = other.gameObject.GetComponent<Bounce>();
        if (bounce)
        { // Ball-ball
            
            Movement otherMovement = other.gameObject.GetComponent<Movement>();
            Assert.IsTrue(otherMovement, "The collided object does not have a Movement component");

            // Assume the other ball is static 
            Vector2 normal = other.contacts[0].normal;
            Vector2 speedDir = _movement.Speed.normalized;
            float theta = Vector2.Angle(new Vector2(1.0f, 0.0f), normal); // in degrees
            theta = Mathf.Deg2Rad * theta;
            float phi1 = Mathf.Atan(bounce.mass * Mathf.Sin(theta));
            float newSpeedMag = Mathf.Sqrt(mass * mass + bounce.mass * bounce.mass + 2 * mass * bounce.mass* Mathf.Cos(theta))
                / (mass + bounce.mass);

            _newSpeed = Quaternion.Euler(0, 0, 90.0f) * (-1.0f * normal * newSpeedMag);
            Debug.Log(theta);

            _collided = true;

        }
        else // Static object
        {
            
            // Change speed direction and magnitude based on perfect specular relexion and restitution factor
            Vector2 normal = other.contacts[0].normal;
            Vector2 dir = -1.0f * _movement.speed;
            dir.Normalize();

            Vector2 outDir = 2 * (Vector2.Dot(normal, dir)) * normal - dir;
            outDir.Normalize();
            _newSpeed = outDir * _movement.speed.magnitude;
            _collided = true;
        }
    }
    
}
