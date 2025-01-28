using System.Collections;
using System.Collections.Generic;
using Helpers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using IPoolable = Interfaces.IPoolable;

public class Card : MonoBehaviour, IPoolable
{
    [SerializeField] private GameObject visuals;
    [SerializeField] private SpriteRenderer cardFace;
    [SerializeField] private TextMesh cardValue;
    [SerializeField] private Collider cardCollider;
    
    private CardConfig? _cardConfig;
    private int _points;
    
    public void ConfigureSelf(CardConfig config)
    {
        _cardConfig = config;
        cardFace.sprite = Utilities.GetCardSprite(_cardConfig.Value.cardSuit, _cardConfig.Value.cardValue);
        cardValue.text = $"{_cardConfig.Value.cardValue}";
        _points = Utilities.GetCardPoint(config);
    }
    
    public void OnPooled()
    {
        visuals.gameObject.SetActive(false);
        cardCollider.enabled = false;
    }

    public void OnFetchFromPool()
    {
        visuals.gameObject.SetActive(true);
        cardCollider.enabled = true;
    }

    public void OnReturnPool()
    {
        _cardConfig = null;
        cardFace.sprite = null;
        cardValue.text = "";
        _points = 0;
    }
}
