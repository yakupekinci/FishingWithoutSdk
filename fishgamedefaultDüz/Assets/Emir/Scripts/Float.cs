using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Float")]
public class Float : ScriptableObject
{
    [SerializeField] float value;
    public float Value { get { return value; } set { this.value = value; OnValueChangedEvent?.Invoke(value); } }
    [HideInInspector] public UnityEvent<float> OnValueChangedEvent;
}
