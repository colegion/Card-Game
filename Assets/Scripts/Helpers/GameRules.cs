using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public class GameRules
    {
        public bool IsMoveCollectable(List<Card> cardsOnTable, out CollectType type)
        {
            if (IsLastCardJack(cardsOnTable[^1]))
            {
                type = CollectType.Jack;
                return true;
            }

            if (AreLastTwoCardsSame(cardsOnTable) && cardsOnTable.Count == 2)
            {
                type = CollectType.Pisti;
                return true;
            }
            
            if (AreLastTwoCardsSame(cardsOnTable))
            {
                type = CollectType.IdenticalCard;
                return true;
            }

            type = CollectType.None;
            return false;
        }
        
        
        private bool AreLastTwoCardsSame(List<Card> cards)
        {
            var lastCard = cards[^1].GetConfig();
            var secondLastCard = cards[^2].GetConfig();
            Debug.Log($"Last two cards: {lastCard.cardValue} {lastCard.cardSuit} and {secondLastCard.cardValue} {secondLastCard.cardSuit}");
            return lastCard.Equals(secondLastCard);
        }

        private bool IsLastCardJack(Card card)
        {
            return card.IsJackCard();
        }
    }
}
