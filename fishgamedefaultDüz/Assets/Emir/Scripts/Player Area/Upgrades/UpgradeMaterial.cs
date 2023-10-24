using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMaterial : MonoBehaviour
{
    public int matId;
    public string meshName;


    bool isSelected;
    bool isUnlocked;
    int selection;

    public MeshRenderer meshRenderer;
    public Material material;
    public GameObject selectBtn;
    public GameObject unlockBtn;
    public GameObject selectedBtn;

    public UpgradeMaterial[] otherUpgradeMats;

    // 0 = locked not selected
    // 1 = unlocked not selected
    // 2 = unlocked selected

    public void Load()
    {
        selection = PlayerPrefs.GetInt(meshName + matId + "selection", 0);
        isSelected = selection == 2;
        isUnlocked = selection != 0;
        if (isSelected)
        {
            meshRenderer.material = material;
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
        PlayerPrefs.SetInt(meshName + matId + "selection", selection);
    }

    public void Select()
    {
        if (!isUnlocked)
            return;

        if (isSelected)
            return;

        isSelected = true;
        AudioManager.Instance.PlayUISoundAtGameCam();
        selectedBtn.SetActive(true);
        selectBtn.SetActive(false);
        selection = 2;
        meshRenderer.material = material;
        PlayerPrefs.SetInt(meshName + matId + "selection", selection);
        foreach (var otherUpgrade in otherUpgradeMats)
        {
            otherUpgrade.Deselect();
        }
    }

    public void Unlock()
    {
        if (isUnlocked)
            return;

        if (isSelected)
            return;

        // watch ads

        AudioManager.Instance.PlayUpgradeUIClip();
        isUnlocked = true;
        unlockBtn.SetActive(false);
        selection = 1;
        PlayerPrefs.SetInt(meshName + matId + "selection", selection);
        Select();
    }

}
