using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Numerics;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class Predictor : MonoBehaviour
{
    public GameObject indicatorPrefab; 

    public uint numIndicators = 20; // Num of indicators stored in the array

    public float indicatorScale = 1.0f; // Scale of indicator

    public Color indicatorColor = Color.red; // Color of indicator

    public GameObject[] _indicatorArray; // Array of trajectory 

    private SpriteRenderer _spriteRenderer; // Sprite for the idicators

    private uint _iterator = 0; // Next array position to show on trajectory prediction

    // Start is called before the first frame update
    void Start()
    {

        // Initialize all indicators
        _indicatorArray = new GameObject[numIndicators];
        for(int i = 0; i < numIndicators; ++i)
        {
            _indicatorArray[i] = Instantiate(indicatorPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            _indicatorArray[i].name = "Indicator" + i;
            _indicatorArray[i].transform.parent = gameObject.transform;
            _indicatorArray[i].transform.localScale = new Vector3(indicatorScale, indicatorScale, indicatorScale);
            _indicatorArray[i].SetActive(false);
            _indicatorArray[i].transform.position = gameObject.transform.position + new Vector3(i, 0.0f, 0.0f);
            _indicatorArray[i].GetComponent<SpriteRenderer>().color = indicatorColor;
            _indicatorArray[i].transform.rotation = Quaternion.identity;
        }
        _iterator = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Paint(Vector2 pos, Vector2 dir)
    {
        
        _indicatorArray[_iterator].GetComponent<SpriteRenderer>().enabled = true;
        _indicatorArray[_iterator].transform.position = pos;

        Vector2 aux = dir.normalized;

       // Debug.Log(aux);
        float sign = Vector2.Dot(aux, new Vector2(0, 1)) > 0 ? 1 : -1;

        float angle = (float)(180.0 / Math.PI *
            Math.Acos(Vector2.Dot(aux, new Vector2(1, 0))));

        Quaternion rot = Quaternion.identity;
        rot.eulerAngles = new Vector3(0, 0,  (sign * angle) - 90);

        _indicatorArray[_iterator].transform.rotation = rot;

        _indicatorArray[_iterator].SetActive(true);
        _iterator = (_iterator + 1) % numIndicators;
        
    }

    /* Simulates the update of the movement to see the trayectory */
    public void SimulateUpdate(uint times, float timeStep, Vector2 speed)
    {
        Vector2 initialPos = new Vector2(transform.position.x, transform.position.y);
        _iterator = 0;
        for (float i = 1; i <= times; ++i)
        {
            Vector2 newPos = initialPos + speed * timeStep * i;
            Paint(new Vector2(newPos.x, newPos.y), 
                    speed);
            //transform.position = new Vector3(_position.x, _position.y, 0.0f);
            //GetComponent<Predictor>().Paint(_position, new Vector2(0.0f, 0.0f));
        }
    }

}
