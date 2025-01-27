using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Helpers
{
    public class Utilities : MonoBehaviour
    {
        [SerializeField] private List<CardSpriteContainer> cardSprites;
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
}
