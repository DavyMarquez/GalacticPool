using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float speed = 0.1f;
    public float restitution = 0.0f;

    // max length for the throw
    public float maxLength = 10.0f;

    /* Position of the mouse when clicked and released to 
    / calculate force to apply */
    private Vector3 _mouseDownPos;
    private Vector3 _mouseUpPos;

    private Vector2 _direction = new Vector2(0.0f, 0.0f);

    // amount of speed thats going to be applied to the movement
    private float _magnitude = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _direction = new Vector2(0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = this.transform.position + new Vector3(_direction.x, _direction.y, 0.0f) * speed * _magnitude;
        this.transform.position = new Vector3(newPos.x, newPos.y, 0.0f);
    }

    private void OnMouseDown()
    {
        _mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mouseDownPos.z = 0.0f;

    }

    private void OnMouseUp()
    {
        _mouseUpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mouseUpPos.z = 0.0f;

        Vector2 downAux = new Vector2(_mouseDownPos.x, _mouseDownPos.y);
        Vector2 upAux = new Vector2(_mouseUpPos.x, _mouseUpPos.y);
        _direction = (downAux - upAux);

        _magnitude = Mathf.Min(_direction.magnitude, maxLength);
        _magnitude /= maxLength;
        _direction.Normalize();

        Debug.Log("Speed = " + speed * _magnitude);
    }
}
