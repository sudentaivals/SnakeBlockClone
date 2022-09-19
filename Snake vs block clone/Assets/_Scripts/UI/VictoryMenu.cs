using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryMenu : CanvasPresent
{
    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.Subscribe(EventBusEvent.Victory, ActivateCanvas);
        
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBus.Unsubscribe(EventBusEvent.Victory, ActivateCanvas);
    }

}
