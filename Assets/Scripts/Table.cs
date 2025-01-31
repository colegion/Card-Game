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
    
    private List<Card> _cardsOnTable = new List<Card>();
    
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
    
    public void AppendCardsOnTable(Card card)
    {
        if (_cardsOnTable.Contains(card))
        {
            Debug.LogError("Duplicate card!");
        }
        else
        {
            _cardsOnTable.Add(card);
        }
    }
    
    public List<Card> GetCardsOnTable()
    {
        return _cardsOnTable;
    }
    
    public void ClearOnTableCards()
    {
        ResetTransformCounter();
        _cardsOnTable.Clear();
    }

    private void ResetTransformCounter()
    {
        _totalCounter = 0;
    }

}
