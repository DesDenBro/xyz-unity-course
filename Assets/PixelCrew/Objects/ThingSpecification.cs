using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingSpecification : MonoBehaviour
{
    [SerializeField] private int _cost;

    public int GetCost()
    {
        return _cost;
    }
}
