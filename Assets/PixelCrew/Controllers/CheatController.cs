﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CheatController : MonoBehaviour
{
    [SerializeField] private float _inputTimeToLive;
    [SerializeField] private CheatItem[] _cheats;

    private string _currentInput;
    private float _inputTime;

    private void Awake()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnDestroy()
    {
        Keyboard.current.onTextInput -= OnTextInput;
    }

    private void Update()
    {
        if (_inputTime < 0)
        {
            _currentInput = string.Empty;
        }
        else
        {
            _inputTime -= Time.deltaTime;
        }
    }


    private void OnTextInput(char inputChar)
    {
        _currentInput += inputChar;
        _inputTime = _inputTimeToLive;

        FindAnyCheats();
    }

    private void FindAnyCheats()
    {
        if (string.IsNullOrEmpty(_currentInput)) return;

        foreach (var cheat in _cheats)
        {
            if (_currentInput.Contains(cheat.Name))
            {
                cheat.Action?.Invoke();
                _currentInput = string.Empty;
                break;
            }
        }
    }
}

[Serializable]
public class CheatItem
{
    [SerializeField] private string _name;
    [SerializeField] private UnityEvent _action;

    public string Name => _name;
    public UnityEvent Action => _action;
}
