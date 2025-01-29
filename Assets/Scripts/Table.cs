using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Table : MonoBehaviour
{
    [SerializeField] private Transform[] cardTransforms;
    [SerializeField] private float yOffset;

    private int _transformIndex;
    private int _totalCounter;
    
    public Vector3 GetCardTarget()
    {
        var target = cardTransforms[_transformIndex];
        _transformIndex++;
        _totalCounter++;
        if (_transformIndex == cardTransforms.Length)
        {
            _transformIndex = 0;
        }
        return target.position + new Vector3(0, yOffset * _totalCounter, 0);
    }

    public void ResetTransformCounter()
    {
        _totalCounter = 0;
    }

}
