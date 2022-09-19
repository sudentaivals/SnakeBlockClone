using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out var pc))
        {
            GameManager.Instance.TriggerGameState(GameState.Victory);
        }
    }
}
