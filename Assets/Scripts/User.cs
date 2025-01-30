using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using Interfaces;
using UnityEngine;

public abstract class User : MonoBehaviour
{
    [SerializeField] protected CardAnimator cardAnimator;
    [SerializeField] private Hand hand;
    [SerializeField] private Table table;
    
    protected List<Card> Cards = new List<Card>();
    protected IGameState UserState;
    
    private List<CardConfig> _collectedCards = new List<CardConfig>();

    public void InjectUserState(IGameState state)
    {
        if (UserState != null) return;
        UserState = state;
    }
    
    public void SetCards(List<Card> cards)
    {
        Cards = cards;
        ReceiveCards();
    }

    private void ReceiveCards()
    {
        foreach (var card in Cards)
        {
            var slot = hand.GetAvailableSlot();
            slot.SetCardReference(card);
            cardAnimator.AnimateSelectedCard(card, slot.GetTarget().position, this is not Bot, null);
        }
    }

    public void CollectCards(List<Card> cards, Action onComplete)
    {
        foreach (var card in cards)
        {
            _collectedCards.Add(card.GetConfig());
        }
        
        cardAnimator.OnCardsCollected(cards, transform, () =>
        {
            onComplete?.Invoke();
        });
    }

    public bool IsHandEmpty()
    {
        return Cards.Count == 0;
    }
    
    public virtual void OnCardPlayed(Card card)
    {
        Cards.Remove(card);
        hand.EmptySlotByCard(card);
        GameController.Instance.AppendCardsOnTable(card);
        cardAnimator.AnimateSelectedCard(card, table.GetCardTarget(), true,() =>
        {
            UserState.ExitState();
        });
    }
    
    public abstract void OnTurnStart();
}
