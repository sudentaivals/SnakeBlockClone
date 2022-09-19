using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayMenu : MonoBehaviour
{
    [SerializeField] TMP_Text _currentLevelText;
    [SerializeField] TMP_Text _nextLevelText;
    [SerializeField] TMP_Text _currentSegments;
    [SerializeField] Image _fillBar;
    [SerializeField] Image _nextLevelImage;
    private Transform _player;
    private Transform _finish;

    private void OnEnable()
    {
        EventBus.Subscribe(EventBusEvent.SegmentsQuntityChanged, SetNewSegmentQuantity);
        EventBus.Subscribe(EventBusEvent.Victory, SetNextLevelImageColor);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventBusEvent.SegmentsQuntityChanged, SetNewSegmentQuantity);
        EventBus.Unsubscribe(EventBusEvent.Victory, SetNextLevelImageColor);
    }

    void Start()
    {
        _currentLevelText.text = GameManager.Instance.LevelId.ToString();
        _nextLevelText.text = (GameManager.Instance.LevelId + 1).ToString();

        _player = FindObjectOfType<PlayerController>().transform;
        _finish = FindObjectOfType<Finish>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        var range = _player.position.z / _finish.position.z;
        _fillBar.fillAmount = Mathf.Clamp(range, 0f, 1f);
    }

    private void SetNewSegmentQuantity(UnityEngine.Object sender, EventArgs args)
    {
        var segments = sender as SegmentManager;
        _currentSegments.text = segments.CurrentSegments.ToString();
    }

    private void SetNextLevelImageColor(UnityEngine.Object sender, EventArgs args)
    {
        _nextLevelImage.color = _fillBar.color;
    }
}
