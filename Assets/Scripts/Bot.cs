using System.Collections;
using System.Collections.Generic;
using BotStrategy;
using Interfaces;
using UnityEngine;

public class Bot : User
{
    private IBotStrategy _currentStrategy = new EasyBot();
    
    public override void OnTurnStart()
    {
        _currentStrategy.Play();
    }

    public List<Card> GetHand()
    {
        return Cards;
    }
}
