using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class EventTriggerShortcut
{
    public static void AddEvent(this EventTrigger trigger, EventTriggerType triggerType, Action<PointerEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = triggerType;
        entry.callback.AddListener((data) => { action?.Invoke((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
}