using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressBar : MonoBehaviour
{
    [SerializeField] Transform _platforms;
    [SerializeField] Image _levelProgressBar;
    [SerializeField] TextMeshProUGUI _currentLevelText;
    [SerializeField] TextMeshProUGUI _nextLevelText;

    private int _numberOfPlatforms;
    private int _destroyedPlatforms = 0;

    private int DestroyedPlatforms
    {
        get
        {
            return _destroyedPlatforms;
        }
        set
        {
            _destroyedPlatforms = value;
            _levelProgressBar.fillAmount = _destroyedPlatforms / (float)_numberOfPlatforms;
        }
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    void Start()
    {
        _currentLevelText.text = "" + GameManager.Instance.LevelId;
        _nextLevelText.text = "" + (GameManager.Instance.LevelId + 1);
        // -1 for finish platform
        _numberOfPlatforms = _platforms.transform.childCount - 1;
        DestroyedPlatforms = 0;
    }

    private void UpdateFillBar(UnityEngine.Object sender, EventArgs args)
    {
        DestroyedPlatforms++;
    }



}
