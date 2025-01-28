using System;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PoolController poolController;

    private List<CardConfig> _deck;

    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameController is not initialized! Ensure there is a GameController in the scene.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        
        poolController.Initialize();
        BuildDeck();
    }

    private void BuildDeck()
    {
        _deck = new List<CardConfig>();
        foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
        {
            foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
            {
                var tempConfig = new CardConfig()
                {
                    cardSuit = suit,
                    cardValue = value
                };
                
                _deck.Add(tempConfig);
                _deck.Shuffle();
            }
        }
    }
}