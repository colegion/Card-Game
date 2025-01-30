using System;
using TMPro;
using UnityEngine;

namespace Helpers
{
    public class UserUIHelper : MonoBehaviour
    {
        [SerializeField] private User user;
        [SerializeField] private TextMeshProUGUI totalCardCountField;
        [SerializeField] private TextMeshProUGUI totalPointField;
        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void HandleOnCollectedCardsUpdated(int collectedCardCount, int points)
        {
            totalCardCountField.text = $"Collected: {collectedCardCount}";
            totalPointField.text = $"Points: {points}";
        }

        private void AddListeners()
        {
            user.OnCollectedCardsUpdated += HandleOnCollectedCardsUpdated;
        }

        private void RemoveListeners()
        {
            user.OnCollectedCardsUpdated -= HandleOnCollectedCardsUpdated;
        }
    }
}
