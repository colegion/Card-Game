using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Helpers
{
    public class CardAnimator : MonoBehaviour
    {
        public void AnimateSelectedCard(Card card, Transform cardTarget, Action onComplete)
        {
            card.DisableBackground();
            card.transform.DOMove(cardTarget.position, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }

        public void MoveCardsToTarget(List<Card> cards, Transform target)
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
