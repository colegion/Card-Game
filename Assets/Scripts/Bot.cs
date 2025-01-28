using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class Bot : User
{
    private IBotStrategy _currentStrategy;
    
    public override void OnTurnStart()
    {
        _currentStrategy.Play();
    }

    public override void OnCardPlayed(Card card)
    {
        throw new System.NotImplementedException();
    }

    public List<Card> GetHand()
    {
        return Cards;
    }
}
