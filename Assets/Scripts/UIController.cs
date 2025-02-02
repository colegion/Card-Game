using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject blackishPanel;
    [SerializeField] private TextMeshProUGUI endGameField;
    [SerializeField] private TextMeshProUGUI roundField;
    [SerializeField] private Button startGameButton;
    [SerializeField] private TMP_Dropdown botTypeDropdown; 

    private BotType _selectedBotType;
    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void Start()
    {
        PopulateBotDropdown();
    }

    private void RequestGame()
    {
        blackishPanel.gameObject.SetActive(false);
        GameController.Instance.StartGame(_selectedBotType);
    }

    private void HandleOnGameFinished(bool isWin)
    {
        blackishPanel.gameObject.SetActive(true);
        endGameField.gameObject.SetActive(true);
        if (isWin)
        {
            endGameField.text = "You win!";
            endGameField.color = Color.green;
        }
        else
        {
            endGameField.text = "You lose.";
            endGameField.color = Color.red;
        }
    }
    
    private void PopulateBotDropdown()
    {
        botTypeDropdown.ClearOptions();
        List<string> options = new List<string>(Enum.GetNames(typeof(BotType)));
        botTypeDropdown.AddOptions(options);
        
        botTypeDropdown.onValueChanged.AddListener(index =>
        {
            _selectedBotType = (BotType)index;
        });

        _selectedBotType = (BotType)botTypeDropdown.value;
    }

    private void AnimateRoundField(int round, Action onComplete)
    {
        roundField.text = $"Round {round}";
        roundField.transform.localScale = Vector3.one;
        roundField.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();

        sequence.Append(roundField.transform.DOScale(1.5f, 0.15f)
            .SetEase(Ease.OutBack));
        sequence.Join(roundField.DOColor(Color.yellow, 0.15f));
        sequence.Append(roundField.transform.DOScale(1.0f, 0.35f)
            .SetEase(Ease.InOutQuad));
        sequence.Join(roundField.DOColor(Color.white, 0.35f));
        sequence.OnComplete(() =>
        {
            roundField.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    private void AddListeners()
    {
        startGameButton.onClick.AddListener(RequestGame);
        GameController.OnGameFinished += HandleOnGameFinished;
        CardDistributionState.OnRoundDistributed += AnimateRoundField;
    }

    private void RemoveListeners()
    {
        startGameButton.onClick.RemoveListener(RequestGame);
        GameController.OnGameFinished -= HandleOnGameFinished;
        CardDistributionState.OnRoundDistributed -= AnimateRoundField;
    }
}
