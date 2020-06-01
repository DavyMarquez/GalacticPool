using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Component that curved a ball position
 */
public class Orbit : MonoBehaviour
{

    public float mass = 1.0f;

    private float radius = 5.0f;

    public float Radius {
        get { return radius; }
    }

    private CircleCollider2D outter;
        
    // Start is called before the first frame update
    void Start()
    {
        foreach(CircleCollider2D cc in GetComponents<CircleCollider2D>())
        {
            if (cc.isTrigger)
            {
                radius = cc.radius;
                outter = cc;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
