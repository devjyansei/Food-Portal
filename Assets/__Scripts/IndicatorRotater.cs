using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorRotater : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 50f;
    
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 20*Time.deltaTime );
    }
}
