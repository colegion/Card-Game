using System;
using System.Collections;
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
            card.transform.DOMove(cardTarget, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }

        public void OnCardsCollected(List<Card> cards, Transform target, Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();

            foreach (var card in cards)
            {
                sequence.Append(card.transform.DOMove(target.position, 0.1f).SetEase(Ease.Linear));
                sequence.AppendInterval(0.03f);
            }
            
            sequence.AppendCallback(() =>
            {
                foreach (var card in cards)
                {
                    GameController.Instance.ReturnObjectToPool(card);
                }
                
                onComplete?.Invoke();
            });
        }

        public void OnUserGotPisti(List<Card> cards, Transform target, Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();

            foreach (var card in cards)
            {
                sequence.Append(card.transform.DOScale(1.5f, 0.15f).SetEase(Ease.OutBack));

                sequence.Append(card.transform.DOMove(target.position, 0.35f).SetEase(Ease.OutExpo))
                    .Join(card.transform.DOScale(1.2f, 0.15f));

                sequence.Append(card.transform.DOScale(0f, 0.12f).SetEase(Ease.InBack));

                sequence.AppendInterval(0.02f);
            }

            sequence.AppendCallback(() =>
            {
                Camera.main.transform.DOShakePosition(0.2f, 0.2f, 20, 90, false, true);
        
                foreach (var card in cards)
                {
                    GameController.Instance.ReturnObjectToPool(card);
                }
        
                onComplete?.Invoke();
            });
        }
    }
}
