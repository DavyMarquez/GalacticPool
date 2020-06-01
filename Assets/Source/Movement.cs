using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    public Vector2 speed = new Vector2(0.0f, 0.0f);
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = new Vector2(this.transform.position.x, this.transform.position.y) + speed * Time.deltaTime;
        this.transform.position = new Vector3(newPos.x, newPos.y, 0.0f);
    }

}
