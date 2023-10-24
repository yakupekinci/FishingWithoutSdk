using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Int")]
public class Int : ScriptableObject
{
    [SerializeField] int value;
    public int Value { get { return value; } set { this.value = value; OnValueChangedEvent?.Invoke(value); } }
    [HideInInspector] public UnityEvent<int> OnValueChangedEvent;
}
