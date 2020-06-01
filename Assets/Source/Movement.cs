using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float restitution = 0.0f;

    private Rigidbody2D rigidbody;

    /* Position of the mouse when clicked and released to 
    / calculate force to apply */
    private Vector3 _mousePosDown;
    private Vector3 _mousePosUp;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //_mousePosDown = 
    }
}
