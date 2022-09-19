using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboTextAnimation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    private Animator _animator;

    private const string SMALL_COMBO = "Small";
    private const string MEDIUM_COMBO = "Medium";
    private const string BIG_COMBO = "Big";
    private const string DEFAULT = "Start";


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void SetNewComboText(int comboCount)
    {
        if (comboCount == 0)
        {
            _text.text = "";
            return;
        }
        if (comboCount > 0 && comboCount <= 5)
        {
            Transition(SMALL_COMBO);
        }
        else if (comboCount > 5 && comboCount <= 10)
        {
            Transition(MEDIUM_COMBO);
        }
        else
        {
            Transition(BIG_COMBO);
        }
        _text.text = $"x{comboCount}";
    }

    private void Transition(string animationName)
    {
        _animator.Play(DEFAULT);
        _animator.Play(animationName);
    }
}
