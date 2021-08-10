using System;
using UnityEngine.Events;

//Evemts
[Serializable]
public class VectorEvent : UnityEvent<float, float> { }

[Serializable]
public class OnTriggerEvent : UnityEvent { }

[Serializable]
public class OnTriggerBoolEvent : UnityEvent<bool>  { }

