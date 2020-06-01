using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Bounce : MonoBehaviour
{
    [Range(0, 1)]
    public float restitution = 1.0f;

    public float Restitution
    {
        get { return restitution; }
        set { restitution = value; }
    }

    private Vector2 _newSpeed = new Vector2(0.0f, 0.0f);

    // Whether or not there is a collision
    private bool _collided = false;

    // This GameObject movement component
    private Movement _movement;

    public float mass = 1.0f;

    public float Mass
    {
        get { return mass; }
        set { mass = value; }
    }

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
            _movement.Speed = _newSpeed * restitution;
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

            _newSpeed = new Vector2(0.0f, 0.0f);

            // Assume the other ball is static 
            Vector2 normal = other.contacts[0].normal;
            if(!_movement.Speed.Equals(new Vector2(0.0f, 0.0f)))
            {
                float theta = Vector2.Angle(new Vector2(1.0f, 0.0f), normal); // in degrees
                theta = Mathf.Deg2Rad * theta;
                float newSpeedMag = _movement.Speed.magnitude *
                    Mathf.Sqrt(mass * mass + bounce.Mass * bounce.Mass + 2 * mass * bounce.Mass * Mathf.Cos(theta)) /
                    (mass + bounce.Mass);

                Vector3 normal3 = new Vector3(-normal.x, -normal.y, 0.0f);
                _newSpeed = Vector3.Cross(normal3, new Vector3(0.0f, 0.0f, -1.0f)) * newSpeedMag;
            }
            

            // Assume this ball is static
            if (!otherMovement.Speed.Equals(new Vector2(0.0f, 0.0f)))
            {
                float theta = Vector2.Angle(new Vector2(1.0f, 0.0f), normal);
                theta = Mathf.Deg2Rad * theta;

                float newSpeedMag = otherMovement.Speed.magnitude * (2 * bounce.Mass / (bounce.Mass + mass)) *
                    Mathf.Sin(theta / 2.0f);
                Vector3 newDir = normal;
                newDir.Normalize();
                _newSpeed += new Vector2(newDir.x * newSpeedMag, newDir.y * newSpeedMag);
                
            }

            _collided = true;

        }
        else // Static object
        {
            
            // Change speed direction and magnitude based on perfect specular relexion and restitution factor
            Vector2 normal = other.contacts[0].normal;
            Vector2 dir = -1.0f * _movement.Speed;
            dir.Normalize();

            Vector2 outDir = 2 * (Vector2.Dot(normal, dir)) * normal - dir;
            outDir.Normalize();
            _newSpeed = outDir * _movement.Speed.magnitude;
            _collided = true;
        }
    }
    
}
