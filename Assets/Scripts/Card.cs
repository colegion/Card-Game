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
    [SerializeField] private Collider2D cardCollider;
    
    private CardConfig _cardConfig;
    private int _points;
    
    public void ConfigureSelf(CardConfig config, bool isFaceDown)
    {
        _cardConfig = config;
        if(!isFaceDown) cardFace.sprite = Utilities.GetCardSprite(_cardConfig.cardSuit, _cardConfig.cardValue);
        cardValue.text = $"{_cardConfig.cardValue}";
        _points = Utilities.GetCardPoint(config);
    }

    public CardConfig OnCardCollected()
    {
        _cardConfig.point = _points;
        return _cardConfig;
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
        _cardConfig.cardSuit = CardSuit.Null;
        _cardConfig.cardValue = CardValue.Null;
        _cardConfig.point = 0;
        _points = 0;
        cardFace.sprite = null;
        cardValue.text = "";
    }
}
