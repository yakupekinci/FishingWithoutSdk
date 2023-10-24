using UnityEngine;

public class UpgradeFishRod : MonoBehaviour
{
    public int matId;
    public const string MESH_NAME = "hook_gfx";


    bool isSelected;
    bool isUnlocked;
    int selection;

    public GameObject selectBtn;
    public GameObject unlockBtn;
    public GameObject selectedBtn;

    public UpgradeFishRod[] otherUpgrades;

    // 0 = locked not selected
    // 1 = unlocked not selected
    // 2 = unlocked selected

    private void Awake()
    {
        Load();
    }

    public void Load()
    {
        selection = PlayerPrefs.GetInt(MESH_NAME + matId + "selection", 0);
        if (matId == 0 && selection == 0)
        {
            selection = 2;
            PlayerPrefs.SetInt(MESH_NAME + matId + "selection", selection);
        }
        isSelected = selection == 2;
        isUnlocked = selection != 0;
        if (isSelected)
        {
            selectedBtn.SetActive(true);
            selectBtn.SetActive(false);
            unlockBtn.SetActive(false);
        }
        else if (isUnlocked)
        {
            selectedBtn.SetActive(false);
            selectBtn.SetActive(true);
            unlockBtn.SetActive(false);
        }
        else
        {
            unlockBtn.SetActive(true);
            selectedBtn.SetActive(false);
            selectBtn.SetActive(false);
        }
    }

    public void Deselect()
    {
        if (!isUnlocked)
            return;

        if (!isSelected)
            return;

        isSelected = false;
        selectedBtn.SetActive(false);
        selectBtn.SetActive(true);
        selection = 1;
        PlayerPrefs.SetInt(MESH_NAME + matId + "selection", selection);
    }

    public void Select()
    {
        if (!isUnlocked)
            return;

        if (isSelected)
            return;

        AudioManager.Instance.PlayUISoundAtGameCam();
        Selection();
    }

    public void Unlock()
    {
        if (isUnlocked)
            return;

        if (isSelected)
            return;

        // watch ads

        AudioManager.Instance.PlayUpgradeUIClip();
        /*   AdManager.Instance.ShowAd(Upgrade, null); */
        Upgrade();
    }

    private void Upgrade()
    {
        isUnlocked = true;
        unlockBtn.SetActive(false);
        selection = 1;
        PlayerPrefs.SetInt(MESH_NAME + matId + "selection", selection);
        Selection();
    }

    private void Selection()
    {
        isSelected = true;
        selectedBtn.SetActive(true);
        selectBtn.SetActive(false);
        selection = 2;
        PlayerPrefs.SetInt(MESH_NAME + matId + "selection", selection);
        PlayerPrefs.SetInt(MESH_NAME, matId);
        foreach (var otherUpgrade in otherUpgrades)
        {
            otherUpgrade.Deselect();
        }
    }

}
