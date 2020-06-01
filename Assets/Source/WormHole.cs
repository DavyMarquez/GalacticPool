using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHole : MonoBehaviour
{
    public WormHole OtherHole;

    private List<GameObject> passing;

    private Vector2 speed;

    // Start is called before the first frame update
    void Start()
    {
        passing = new List<GameObject>(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Ball") && !passing.Contains(collision.gameObject))
        {
            GameObject obj = collision.gameObject;

            passing.Add(obj);
            OtherHole.passing.Add(obj);

            obj.transform.position = OtherHole.transform.position;

            Movement mov = obj.GetComponent<Movement>();

            float angle = OtherHole.transform.rotation.eulerAngles.z - 
                transform.rotation.eulerAngles.z;

            mov.Speed = Quaternion.AngleAxis(angle, Vector3.forward) * mov.Speed;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Ball"))
        {
            GameObject obj = collision.gameObject;

            passing.Remove(obj);
        }
    }
}
