using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkPlayer : MonoBehaviour
{
    [SerializeField] AK.Wwise.Event _wwiseEvent;

    private void Start()
    {
        _wwiseEvent.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncBeat, MusicBeat);
    }

    private void MusicBeat(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
    {
        print("MusicBeat");
    }
}
