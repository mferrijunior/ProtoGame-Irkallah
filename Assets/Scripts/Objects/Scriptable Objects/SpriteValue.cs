using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = null, menuName = "Scriptable Objects/Sprite Value", order = 0)]
public class SpriteValue : ScriptableObject, ISerializationCallbackReceiver
{
    public Sprite initialValue;

    [HideInInspector]
    public Sprite runtimeValue;

    public void OnAfterDeserialize()
    {
        runtimeValue = initialValue;
    }

    public void OnBeforeSerialize() { }
}
