using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Long")]
public class Long : ScriptableObject
{
    [SerializeField] long value;
    public long Value { get { return value; } set { this.value = value; OnValueChangedEvent?.Invoke(value); } }
    [HideInInspector] public UnityEvent<long> OnValueChangedEvent;
}
