using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Helpers
{
    public class Utilities : MonoBehaviour
    {
        [SerializeField] private List<CardSpriteContainer> cardSprites;

        private static Dictionary<CardSuit, CardSpriteContainer> _cardsDictionary;

        private void Start()
        {
            _cardsDictionary = new Dictionary<CardSuit, CardSpriteContainer>();
            foreach (var container in cardSprites)
            {
                if (!_cardsDictionary.TryAdd(container.suit, container))
                {
                    Debug.LogWarning($"Duplicate entry for suit: {container.suit}. Only the first entry will be used.");
                }
            }
        }
        
        public static Sprite GetCardSprite(CardSuit suit, CardValue value)
        {
            if (_cardsDictionary == null || !_cardsDictionary.ContainsKey(suit))
            {
                Debug.LogError($"Card suit '{suit}' not found in the dictionary.");
                return null;
            }

            var container = _cardsDictionary[suit];
            
            foreach (var faceCard in container.faceCards)
            {
                if (faceCard.value == value)
                {
                    return faceCard.sprite;
                }
            }
            
            if (value >= CardValue.One && value <= CardValue.Ten)
            {
                return container.defaultSuitSprite;
            }

            Debug.LogError($"Sprite not found for suit: {suit}, value: {value}");
            return null;
        }
    }

    [Serializable]
    public struct CardSpriteContainer
    {
        public CardSuit suit;
        public Sprite defaultSuitSprite;
        public List<CardSpriteEntry> faceCards;
    }

    [Serializable]
    public struct CardSpriteEntry
    {
        public CardValue value;
        public Sprite sprite;
    }

    public struct CardConfig
    {
        public CardSuit cardSuit;
        public CardValue cardValue;
    }

    public enum CardSuit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades,
    }

    public enum CardValue
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }

    public enum PoolableTypes
    {
        Card,
        
    }
}
