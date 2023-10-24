using UnityEngine;

public class ActivateObjectOnTrigger : TriggerAreas
{
    public GameObject objToActivate;

    protected override void OnPlayerEnter(Collider other)
    {
        base.OnPlayerEnter(other);
        PlayerMovement.Instance.DisableMovement();
        InGameUI.Instance.ActiveUI = objToActivate;
        objToActivate.SetActive(true);
    }
}
