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
    [SerializeField] private Bot bot;
    [SerializeField] private List<User> users;
    
    private static GameController _instance;
    private static Dictionary<GameStateTypes, IGameState> _gameStates = new Dictionary<GameStateTypes, IGameState>();

    private IGameState _currentState;
    
    private List<CardConfig> _deck;
    private List<CardConfig> _cardsOnTable = new List<CardConfig>();

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
        
        _gameStates = new Dictionary<GameStateTypes, IGameState>
        {
            { GameStateTypes.CardDistribution, new CardDistributionState() },
            { GameStateTypes.BotTurn, new BotTurnState() },
            { GameStateTypes.PlayerTurn, new PlayerTurnState() },
            { GameStateTypes.Outcome, new OutcomeState() }
        };

        _currentState = _gameStates[GameStateTypes.CardDistribution];
        _currentState.EnterState();
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

    public void AppendCardsOnTable(CardConfig config)
    {
        if (_cardsOnTable.Contains(config))
        {
            Debug.LogError("Duplicate card!");
        }
        else
        {
            _cardsOnTable.Add(config);
        }
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

    public List<User> GetUsers()
    {
        return users;
    }

    public IGameState GetStateByType(GameStateTypes type)
    {
        return _gameStates[type];
    }
}