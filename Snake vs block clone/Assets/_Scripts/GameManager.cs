using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonInstance<GameManager>
{
    [SerializeField] int _thisLevelId;
    [Header("SFX")]
    [SerializeField] AudioClip _victorySfx;
    [SerializeField] [Range(0f, 1f)] float _victorySfxVolume = 1f;
    [SerializeField] AudioClip _loseSfx;
    [SerializeField] [Range(0f, 1f)] float _loseSfxVolume = 1f;

    public int LevelId => _thisLevelId;

    void Start()
    {
        TriggerGameState(GameState.Play);
    }

    public void TriggerGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Play:
                Time.timeScale = 1f;
                EventBus.Publish(EventBusEvent.Unpause, this, null);
                break;
            case GameState.Pause:
                Time.timeScale = 0f;
                EventBus.Publish(EventBusEvent.Pause, this, null);
                break;
            case GameState.GameOver:
                EventBus.Publish(EventBusEvent.PlaySound, this, new PlaySoundEventArgs(_loseSfxVolume, _loseSfx));
                EventBus.Publish(EventBusEvent.GameOver, this, null);
                break;
            case GameState.Victory:
                EventBus.Publish(EventBusEvent.PlaySound, this, new PlaySoundEventArgs(_victorySfxVolume, _victorySfx));
                EventBus.Publish(EventBusEvent.Victory, this, null);
                break;
            default:
                break;
        }
    }

    public void Win()
    {
        TriggerGameState(GameState.Victory);
    }
}

public enum GameState
{
    Play,
    Pause,
    GameOver,
    Victory,
}
