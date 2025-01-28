using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;

public abstract class User : MonoBehaviour
{
    [SerializeField] protected CardAnimator cardAnimator;
    
    protected List<Card> Cards = new List<Card>();
    protected IGameState UserState;

    public void InjectUserState(IGameState state)
    {
        if (UserState != null) return;
        UserState = state;
    }
    
    public void SetCards(List<Card> cards)
    {
        Cards = cards;
    }
    
    public abstract void OnTurnStart();
    public abstract void OnCardPlayed(Card card);
}
