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

            float theta1 = Mathf.Atan2( _movement.Speed.normalized.y, _movement.Speed.normalized.x);
            float theta2 = Mathf.Atan2(otherMovement.Speed.normalized.y, otherMovement.Speed.normalized.x);
            float phi = Mathf.Atan2(other.gameObject.transform.position.y - gameObject.transform.position.y,
                other.gameObject.transform.position.x - gameObject.transform.position.x);
            float m1 = mass;
            float m2 = bounce.Mass;

            float v1 = _movement.Speed.magnitude;
            float v2 = otherMovement.Speed.magnitude;

            float vx1 = (v1 * Mathf.Cos(theta1 - phi) * (m1 - m2) + 2 * m2 * v2 * Mathf.Cos(theta2 - phi)) / 
                (m1 + m2) * Mathf.Cos(phi) + v1 * Mathf.Sin(theta1 - phi) * Mathf.Cos(phi + Mathf.PI / 2);
            float vy1 = (v1 * Mathf.Cos(theta1 - phi) * (m1 - m2) + 2 * m2 * v2 * Mathf.Cos(theta2 - phi)) 
                / (m1 + m2) * Mathf.Sin(phi) + v1 * Mathf.Sin(theta1 - phi) * Mathf.Sin(phi + Mathf.PI / 2);

            _newSpeed = new Vector2(vx1, vy1);

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
