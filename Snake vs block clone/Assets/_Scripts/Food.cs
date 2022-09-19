using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] int _segmentsToAdd = 5;
    [SerializeField][Range(0f, 1f)] float _pickupSfxVolume;
    [SerializeField] AudioClip _clip;
    [SerializeField] TMP_Text _text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var pc))
        {
            EventBus.Publish(EventBusEvent.PlaySound, this, new PlaySoundEventArgs(_pickupSfxVolume, _clip));
            EventBus.Publish(EventBusEvent.FoodPickup, this, new AddPlayerSegmentEventArgs(_segmentsToAdd));
            Destroy(gameObject);
        }
    }

    private void OnValidate()
    {
        _text.text = _segmentsToAdd.ToString();
    }
}
