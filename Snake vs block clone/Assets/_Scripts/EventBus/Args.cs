using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEventArgs : EventArgs
{
    public float Volume { get; }

    public AudioClip Clip { get; }

    public float Pitch { get; }

    public PlaySoundEventArgs(float volume, AudioClip clip)
    {
        Volume = volume;
        Clip = clip;
        Pitch = 1;
    }

    public PlaySoundEventArgs(float volume, AudioClip clip, float pitch)
    {
        Volume = volume;
        Clip = clip;
        Pitch = pitch;
    }
}

public class AddPlayerSegmentEventArgs : EventArgs
{
    public int SegmentsToAdd { get; }
    public AddPlayerSegmentEventArgs(int segmentsToAdd)
    {
        SegmentsToAdd = segmentsToAdd;
    }
}