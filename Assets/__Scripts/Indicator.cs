using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] Transform indicatorHolder;
    private void Start()
    {
        indicatorHolder = GameObject.FindWithTag("IndicatorHolder").transform;
        transform.SetParent(indicatorHolder);
        transform.position = indicatorHolder.position;
    }
    
}
