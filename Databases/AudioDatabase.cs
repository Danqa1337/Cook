using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum SoundName
{
    Null,
    Any,
    Slash,
}

[CreateAssetMenu(fileName = "New Audio Database", menuName = "Databases/AudioDatabase")]
public class AudioDatabase : Database<AudioDatabase, IngredientsTable, IngredientsTable.Param, StaticIngredientData>
{
    [SerializeField] public EventReference EventNotFound;
    [SerializeField] public EventReference Slash;

    private SerializableDictionary<SoundName, EventReference> eventsByName;

    protected override void StartReimport()
    {
        base.StartReimport();
        eventsByName = new SerializableDictionary<SoundName, EventReference>();
        var eventRefs = instance.GetType().GetFields();
        var soundNameValues = Enum.GetValues(typeof(SoundName));

        instance.eventsByName.Add(SoundName.Null, instance.EventNotFound);
        foreach (var item in soundNameValues)
        {
            bool found = false;
            foreach (var reference in eventRefs)
            {
                if (reference.Name == item.ToString())
                {
                    found = true;
                    instance.eventsByName.Add((SoundName)item, (EventReference)reference.GetValue(instance));
                    break;
                }
            }
            if (!found)
            {
                UnityEngine.Debug.Log("Audio event not found for " + item.ToString());
            }
        }
    }

    public static EventReference GetAudioEvent(SoundName soundName)
    {
        if (instance.eventsByName.ContainsKey(soundName))
        {
            return instance.eventsByName[soundName];
        }
        else
        {
            UnityEngine.Debug.Log("AudioEvent for " + soundName + " not found");
            return instance.EventNotFound;
        }
    }

    protected override void ProcessParam(IngredientsTable.Param param)
    {
    }

    public override void StartUp()
    {
    }
}