using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    public Vector2 speed = new Vector2(0.0f, 0.0f);
    
    public Vector2 Speed
    {
        get { return speed; }
        set { speed = value; }
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
        
        Vector3 start = gameObject.transform.position;
        Vector3 dir = new Vector3(speed.normalized.x, speed.normalized.y, 0.0f);
        Vector3 end = start + dir * 1;  
        Debug.DrawLine(start, end, Color.red, 0.1f, true);
    }

}
