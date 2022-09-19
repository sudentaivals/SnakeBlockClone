using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : CanvasPresent
{
    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.Subscribe(EventBusEvent.Pause, ActivateCanvas);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBus.Unsubscribe(EventBusEvent.Pause, ActivateCanvas);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetButtonDown("Pause") && !_isAnyCanvasActive)
        {
            GameManager.Instance.TriggerGameState(GameState.Pause);
        }
    }
}
