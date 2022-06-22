using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public const int SeChannel = 20;
    
    public IReadOnlyCollection<AudioSource> audiosourceForSEList { get; }
    
    protected override void Awake()
    {
        base.Awake();

        SetAudioSourceForSE();
    }

    private void SetAudioSourceForSE()
    {
        GameObject seGameObject = new GameObject("SE");
        seGameObject.transform.SetParent(this.transform, false);
    }
}

