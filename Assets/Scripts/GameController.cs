using System;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField] private PoolController poolController;
    [SerializeField] private CardTapHandler cardTapHandler;
    [SerializeField] private Player player;
    public int UserCount = 2;

    private List<CardConfig> _deck;

    private static GameController _instance;

    private IGameState _currentState;
    private InitialState _initialState;
    private BotTurnState _botState;
    private PlayerTurnState _playerState;
    private OutcomeState _outcomeState;
    private CardDistributionState _cardDistributionState;

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
        cardTapHandler.Initialize();
        cardTapHandler.InjectPlayer(player);
        BuildDeck();
        
        _initialState = new InitialState();
        _botState = new BotTurnState();
        _playerState = new PlayerTurnState();
        _outcomeState = new OutcomeState();
        _cardDistributionState = new CardDistributionState();
        
        _currentState = _initialState;
        _currentState.EnterState();
    }

    private void BuildDeck()
    {
        _deck = new List<CardConfig>();
        foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
        {
            if(suit == CardSuit.Null) continue;
            foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
            {
                if (value == CardValue.Null) continue;
                var tempConfig = new CardConfig()
                {
                    cardSuit = suit,
                    cardValue = value
                };
                
                _deck.Add(tempConfig);
            }
        }
        
        _deck.Shuffle();
    }

    public Card GetCard()
    {
        return (Card)poolController.GetPooledObject(PoolableTypes.Card);
    }

    public CardConfig GetRandomConfig()
    {
        var randomIndex = Random.Range(0, _deck.Count);
        var tempConfig = _deck[randomIndex];
        return tempConfig;
    }

    public void RemoveCardFromDeck(CardConfig config)
    {
        _deck.Remove(config);
    }
    
    public void ChangeState(IGameState newState)
    {
        _currentState.ExitState();
        _currentState = newState;
        _currentState.EnterState();
    }
}