using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = null, menuName = "Scriptable Objects/Float Value", order = 0)]
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver
{
    public float initialValue;

    [HideInInspector]
    public float runtimeValue;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        runtimeValue = initialValue;
    }
}
