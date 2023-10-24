using UnityEngine;

public class BuildingBase : MonoBehaviour
{
    public virtual void BuildAnim()
    {
        AudioManager.Instance.PlayOpenUpgradeAreaAt(transform.position);
        gameObject.SetActive(true);
    }

    public virtual void Load()
    {
        gameObject.SetActive(true);
    }

}
