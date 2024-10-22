using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class itemSO : ScriptableObject
{
    public GameObject prefab;
    public string itemName;
    public int level;
    public int point;
    public Color color;

}
