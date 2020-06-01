using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float maxSpeed = 0.1f;

    // max length for the throw
    public float maxLength = 10.0f;

    /* Position of the mouse when clicked and released to 
    / calculate force to apply */
    private Vector3 _mouseDownPos;
    private Vector3 _mouseUpPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Vector2 speed = (downAux - upAux);

        float magnitude = Mathf.Min(speed.magnitude, maxLength);
         magnitude /= maxLength;
        speed.Normalize();

        speed *= (magnitude * maxSpeed);

        Movement m = this.GetComponent<Movement>();
        m.speed = speed;
    }
}
