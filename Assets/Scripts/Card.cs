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
    
    private CardConfig? _cardConfig;
    
    public void ConfigureSelf(CardConfig config)
    {
        _cardConfig = config;
        cardFace.sprite = Utilities.GetCardSprite(_cardConfig.Value.cardSuit, _cardConfig.Value.cardValue);
        cardValue.text = $"{_cardConfig.Value.cardValue}";
    }
    
    public void OnPooled()
    {
        visuals.gameObject.SetActive(false);
    }

    public void OnFetchFromPool()
    {
        visuals.gameObject.SetActive(true);
    }

    public void OnReturnPool()
    {
        _cardConfig = null;
        cardFace.sprite = null;
        cardValue.text = "";
    }
}
