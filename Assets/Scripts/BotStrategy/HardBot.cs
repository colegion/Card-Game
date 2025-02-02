using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;

namespace BotStrategy
{
    public class HardBot : IBotStrategy
    {
        private Bot _bot;
        private HashSet<CardValue> _playedCards = new HashSet<CardValue>();
        private List<CardConfig> _seenCards = new List<CardConfig>();

        public void InjectBot(Bot bot)
        {
            if (_bot != null) return;
            _bot = bot;
        }

        public void Play()
        {
            var cardsOnTable = GameController.Instance.GetCardsOnTable();
            
            var newSeenCards = new List<CardConfig>();

            foreach (var card in cardsOnTable)
            {
                var tempConfig = card.GetConfig();
                
                if (!_seenCards.Contains(tempConfig) && !_playedCards.Contains(tempConfig.cardValue))
                {
                    newSeenCards.Add(tempConfig);
                }
            }
            
            _seenCards.AddRange(newSeenCards);

            var hand = _bot.GetHand();
            Card lastCard = cardsOnTable.Count > 0 ? cardsOnTable[^1] : null;

            if (lastCard == null)
            {
                var card = GetLeastProbableCard(hand);
                _playedCards.Add(card.GetConfig().cardValue);
                _bot.OnCardPlayed(card);
                return;
            }
            
            foreach (var card in hand)
            {
                if (card.GetConfig().cardValue == lastCard.GetConfig().cardValue)
                {
                    _playedCards.Add(card.GetConfig().cardValue);
                    _bot.OnCardPlayed(card);
                    return;
                }
            }

            if (TryPlayJack()) return;
            
            var leastProbableCard = GetLeastProbableCard(hand);
            if (leastProbableCard != null)
            {
                _playedCards.Add(leastProbableCard.GetConfig().cardValue);
                _bot.OnCardPlayed(leastProbableCard);
            }
        }

        private bool TryPlayJack()
        {
            foreach (var card in _bot.GetHand())
            {
                if (card.IsJackCard())
                {
                    _playedCards.Add(card.GetConfig().cardValue);
                    _bot.OnCardPlayed(card);
                    return true;
                }
            }
            return false;
        }

        private Card GetLeastProbableCard(List<Card> hand)
        {
            Dictionary<CardValue, int> cardFrequencies = new Dictionary<CardValue, int>();
            
            foreach (var value in _playedCards)
            {
                cardFrequencies.TryAdd(value, 1);
                cardFrequencies[value]++;
            }

            foreach (var cardConfig in _seenCards)
            {
                var cardValue = cardConfig.cardValue;
                cardFrequencies.TryAdd(cardValue, 1);
                cardFrequencies[cardValue]++;
            }

            Card leastProbableCard = null;
            int minFrequency = int.MaxValue;
            foreach (var card in hand)
            {
                var cardValue = card.GetConfig().cardValue;
                int frequency = cardFrequencies.GetValueOrDefault(cardValue, 0);

                if (frequency < minFrequency)
                {
                    minFrequency = frequency;
                    leastProbableCard = card;
                }
            }

            return leastProbableCard;
        }
    }
}
