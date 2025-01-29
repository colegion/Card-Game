using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Helpers
{
    public class CardAnimator : MonoBehaviour
    {
        public void AnimateSelectedCard(Card card, Vector3 cardTarget, bool forceDisable, Action onComplete)
        {
            if(forceDisable) card.DisableBackground();
            card.transform.DOMove(cardTarget, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }

        public void OnCardsCollected(List<Card> cards, Transform target)
        {
            Sequence sequence = DOTween.Sequence();

            foreach (var card in cards)
            {
                sequence.Append(card.transform.DOMove(target.position, 0.3f).SetEase(Ease.Linear));
                sequence.AppendInterval(0.15f);
            }
            
            sequence.OnComplete(() =>
            {
                foreach (var card in cards)
                {
                    GameController.Instance.ReturnObjectToPool(card);
                }
            });
        }

    }
}
