using System;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PoolController poolController;

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
    
    public void ChangeState(IGameState newState)
    {
        _currentState.ExitState();
        _currentState = newState;
        _currentState.EnterState();
    }
}