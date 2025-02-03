using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck
{
    private List<CardConfig> _deck;

    public Deck()
    {
        BuildDeck();
    }

    private void BuildDeck()
    {
        _deck = new List<CardConfig>();
        foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
        {
            if (suit == CardSuit.Null) continue;
            foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
            {
                if (value == CardValue.Null) continue;
                var tempConfig = new CardConfig()
                {
                    cardSuit = suit,
                    cardValue = value
                };

                tempConfig.point = Utilities.GetCardPoint(tempConfig);

                _deck.Add(tempConfig);
            }
        }

        _deck.Shuffle();
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

    public int GetCont()
    {
        return _deck.Count;
    }

    public bool IsDeckEmpty => _deck.Count == 0;
}