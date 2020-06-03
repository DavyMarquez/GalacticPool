using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float maxSpeed = 0.1f;

    // max length for the throw
    public float maxLength = 10.0f;

    public Sprite arrow;


    /* Position of the mouse when clicked and released to 
    / calculate force to apply */
    private Vector3 _mouseDownPos;
    private Vector3 _mouseUpPos;

    private bool _dragging = false;

    private bool _cancel = false;

    private Movement _movement;

    private GameObject _arrowGO;
    private SpriteRenderer _sr;

    // Start is called before the first frame update
    void Start()
    {
        _movement = this.GetComponent<Movement>();

        _arrowGO = Instantiate(new GameObject(), transform);

        _arrowGO.transform.position = new Vector3(0, 0, -1);

        _sr = _arrowGO.AddComponent<SpriteRenderer>();
        _sr.sprite = arrow;
        _sr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Down, get the mouse position to drag relative to it
        if (Input.GetMouseButtonDown(0))
        {
            _mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mouseDownPos.z = 0.0f;

            _dragging = true;
            
        }
        // Up, calculate the speed and add it to the object
        else if (Input.GetMouseButtonUp(0))
        {
            if (_cancel)
            {
                _cancel = false;
                _dragging = false;

                return;
            }
            _mouseUpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mouseUpPos.z = 0.0f;

            Vector2 downAux = new Vector2(_mouseDownPos.x, _mouseDownPos.y);
            Vector2 upAux = new Vector2(_mouseUpPos.x, _mouseUpPos.y);
            Vector2 speed = (downAux - upAux);

            float magnitude = Mathf.Min(speed.magnitude, maxLength);
            magnitude /= maxLength;
            speed.Normalize();

            speed *= (magnitude * maxSpeed);
            _movement.Speed = speed;

            _dragging = false;
            _sr.enabled = false;
        }
        // cancel the throw
        else if (Input.GetMouseButtonDown(1))
        {
            _cancel = true;
        }
        // print an arrow to inform of the movement speed and direction
        else if (_dragging && !_cancel)
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

            Debug.DrawLine(_movement.transform.position, 
                _movement.transform.position + new Vector3(speed.x, speed.y, 0), Color.red);

            _arrowGO.transform.position = _movement.transform.position + new Vector3(0,0,-1);

            _arrowGO.transform.localScale = new Vector2( magnitude, magnitude);


            Vector2 aux = speed.normalized;

            float sign = Vector2.Dot(aux, new Vector2(0, 1)) > 0 ? 1 : -1;

            float angle = (float) (180.0 / Math.PI * 
                Math.Acos(Vector2.Dot(aux, new Vector2(1, 0))));

            Quaternion rot = Quaternion.identity;
            rot.eulerAngles = new Vector3(0, 0, sign * angle);

            _arrowGO.transform.rotation = rot;

            _sr.color = Color.Lerp(Color.green, Color.red, magnitude);

            _sr.enabled = true;

            
            GetComponent<Predictor>().SimulateUpdate(20, 0.1f, speed);
            
        }
    }
}

/*
Component CopyComponent(Component original, GameObject destination)
{
    System.Type type = original.GetType();
    Component copy = destination.AddComponent(type);
    // Copied fields can be restricted with BindingFlags
    System.Reflection.FieldInfo[] fields = type.GetFields();
    foreach (System.Reflection.FieldInfo field in fields)
    {
        field.SetValue(copy, field.GetValue(original));
    }
    return copy;
}
*/