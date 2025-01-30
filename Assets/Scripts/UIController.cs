using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject blackishPanel;
    [SerializeField] private TextMeshProUGUI endGameField;
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

    private void AddListeners()
    {
        startGameButton.onClick.AddListener(RequestGame);
        GameController.OnGameFinished += HandleOnGameFinished;
    }

    private void RemoveListeners()
    {
        startGameButton.onClick.RemoveListener(RequestGame);
        GameController.OnGameFinished -= HandleOnGameFinished;
    }
}
