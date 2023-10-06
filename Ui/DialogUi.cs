using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.UI;
using System;

public class DialogUi : UiCanvas
{
    [SerializeField] private TextMeshProUGUI _dialogField;
    [SerializeField] private float _charDelay = 0.025f;
    [SerializeField] private float _phraseDelay = 0.5f;
    [SerializeField] private GameObject _buttonsHolder;
    [SerializeField] private Button _positiveButton;
    [SerializeField] private Button _negativeButton;
    [SerializeField] private TextMeshProUGUI _positiveAnswerText;
    [SerializeField] private TextMeshProUGUI _negativeAnswerText;
    [SerializeField] private TextMeshProUGUI _nameLabel;

    private bool _isTyping;
    private Dialog _currentDialog;
    private List<string> _scheduledPhrases;
    private List<char> _currentTypingChars;

    private void OnEnable()
    {
        DialogManager.OnDialogStart += OnDialogStart;
        DialogManager.OnDialogEnd += OnDialogEnd;
    }

    private void OnDisable()
    {
        DialogManager.OnDialogStart -= OnDialogStart;
        DialogManager.OnDialogEnd -= OnDialogEnd;
    }

    private void Start()
    {
        NextPage();
    }

    private void OnDialogStart(Dialog dialog)
    {
        _scheduledPhrases = dialog.Phrases.ToList();
        _currentDialog = dialog;
        _nameLabel.text = dialog.Person.Name;

        if (dialog.PositiveAnswer != null)
        {
            _positiveAnswerText.text = dialog.PositiveAnswer;
            _positiveButton.gameObject.SetActive(true);
        }
        else
        {
            _positiveAnswerText.text = "";
            _positiveButton.gameObject.SetActive(false);
        }

        if (dialog.NegativeAnswer != null)
        {
            _negativeAnswerText.text = dialog.NegativeAnswer;
            _negativeButton.gameObject.SetActive(true);
        }
        else
        {
            _negativeAnswerText.text = "";
            _negativeButton.gameObject.SetActive(false);
        }
        _buttonsHolder.SetActive(false);

        NextPage();

        for (int i = 0; i < dialog.Phrases.Length; i++)
        {
            DrawText(dialog.Phrases[i]);
        }
    }

    private void OnDialogEnd()
    {
        Hide();
    }

    private void NextPage()
    {
        _dialogField.text = "";
    }

    private void DrawText(string text)
    {
        StartCoroutine(TypeTextIE());
        IEnumerator TypeTextIE()
        {
            if (text.Length > 0)
            {
                while (_isTyping)
                {
                    yield return new WaitForEndOfFrame();
                }

                _isTyping = true;

                yield return new WaitForSeconds(_phraseDelay);
                if (_dialogField.text.Length > 0)
                {
                    _dialogField.text += "\n";
                }
                var substrings = text.Split(' ');
                for (int s = 0; s < substrings.Length; s++)
                {
                    var subString = substrings[s];
                    if (subString.StartsWith("/"))
                    {
                        if (subString[1] == 's')
                        {
                            var spriteIndex = int.Parse(subString.Substring(2));
                            _dialogField.text += TextMeshProUtills.Sprite(spriteIndex);
                        }
                    }
                    else
                    {
                        _dialogField.text += " ";
                        for (int i = 0; i < subString.Length; i++)
                        {
                            _dialogField.text += subString[i];
                            yield return new WaitForSeconds(_charDelay);
                        }
                    }
                }
                _isTyping = false;
                _scheduledPhrases.RemoveAt(0);

                if (_scheduledPhrases.Count == 0)
                {
                    _buttonsHolder.SetActive(true);
                }
            }
        }
    }

    private void EndTyping()
    {
        _buttonsHolder.SetActive(true);
    }

    public void SkipTyping()
    {
        if (_isTyping)
        {
            StopAllCoroutines();
            for (int i = 0; i < _currentDialog.Phrases.Length; i++)
            {
                _dialogField.text += "\n" + _currentDialog.Phrases[i];
            }
            _isTyping = false;
            EndTyping();
        }
    }

    public void PositiveAnswer()
    {
        if (_currentDialog != null)
        {
            _currentDialog.AnswerPositive();
        }
    }

    public void NegativeAnswer()
    {
        if (_currentDialog != null)
        {
            _currentDialog.AnswerNegative();
        }
    }

    public void SkipDialog()
    {
        SkipTyping();
        PositiveAnswer();
    }

    private void Hide()
    {
    }
}

public class Dialog
{
    public readonly Person Person;
    public readonly string[] Phrases;
    public readonly string NegativeAnswer;
    public readonly string PositiveAnswer;

    public event Action OnPositiveAnswer;

    public event Action OnNegativeAnswer;

    public Dialog(Person person, IEnumerable<string> phrases, string positiveAnswer, string negativeAnswer)
    {
        Person = person;
        Phrases = phrases.ToArray();
        PositiveAnswer = positiveAnswer;
        NegativeAnswer = negativeAnswer;
    }

    public void AnswerPositive()
    {
        Debug.Log("Dialog asnwered positive");
        OnPositiveAnswer?.Invoke();
    }

    public void AnswerNegative()
    {
        Debug.Log("Dialog asnwered negative");
        OnNegativeAnswer?.Invoke();
    }
}