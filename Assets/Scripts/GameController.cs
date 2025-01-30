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
    [SerializeField] private CardAnimator cardAnimator;
    [SerializeField] private Table table;
    [SerializeField] private List<User> users;
    
    private static GameController _instance;
    private static Dictionary<GameStateTypes, IGameState> _gameStates = new Dictionary<GameStateTypes, IGameState>();

    private IGameState _currentState;
    
    private List<CardConfig> _deck;
    private List<Card> _cardsOnTable = new List<Card>();
    private GameStateTypes _lastOutcomeCallerType;

    public static event Action<bool> OnGameFinished;

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
    }

    public void StartGame(BotType bot)
    {
        poolController.Initialize();
        cardTapHandler.Initialize();
        cardTapHandler.InjectPlayer(GetUser(false) as Player);

        foreach (var user in users)
        {
            if (user is Bot)
            {
                ((Bot)user).SetBotStrategy(bot);
            }
        }
        
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

    public void ReturnObjectToPool(IPoolable objectToReturn)
    {
        poolController.ReturnPooledObject(objectToReturn);
    }

    public CardConfig GetRandomConfig()
    {
        var randomIndex = Random.Range(0, _deck.Count);
        var tempConfig = _deck[randomIndex];
        return tempConfig;
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
            cardAnimator.AnimateSelectedCard(card, table.GetCardTarget(), false, () =>
            {
                
            });
        }
    }

    public List<Card> GetCardsOnTable()
    {
        return _cardsOnTable;
    }

    public void ClearOnTableCards()
    {
        table.ResetTransformCounter();
        _cardsOnTable.Clear();
    }

    public void RemoveCardFromDeck(CardConfig config)
    {
        _deck.Remove(config);
    }
    
    public void ChangeState(IGameState newState)
    {
        if (IsGameFinished())
        {
            OnGameFinished?.Invoke(users.IsPlayerWinner());
        }
        else
        {
            _currentState = newState;
            _currentState.EnterState();
        }
    }

    public List<User> GetAllUsers()
    {
        return users;
    }

    public User GetUser(bool isBot)
    {
        if (isBot)
        {
            return users.Find(u => u is Bot);
        }
        else
        {
            return users.Find(u => u is Player);
        }
    }

    public void SetLastCallerType(GameStateTypes type)
    {
        _lastOutcomeCallerType = type;
    }

    public bool IsGameFinished()
    {
        return _deck.Count == 0;
    }

    public GameStateTypes GetLastOutcomeCallerType()
    {
        return _lastOutcomeCallerType;
    }
    
    public IGameState GetStateByType(GameStateTypes type)
    {
        return _gameStates[type];
    }
}