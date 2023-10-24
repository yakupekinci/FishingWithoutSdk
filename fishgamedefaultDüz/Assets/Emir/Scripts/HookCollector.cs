using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class HookCollector : MonoBehaviour
{
    public CatchedFishArea catchedFishArea;
    public Action OnFishReleased;
    List<Fish> fish;
    Vector3 startPos;
    public Int hookCurrentStack;
    public Int hookCapacity;
    public Float hookLength;
    public Transform fishCollectPos;
    public TextMeshPro capacityTMP;
    [SerializeField] GameObject[] gfxs;
    [SerializeField] private float hookBackSpeed = 1f;
    public bool disableCollect;
    bool canCollect;

    HookMovement hookMovement;

    void Awake()
    {
        if (TryGetComponent<HookMovement>(out hookMovement))
        {
            hookMovement.OnWorkingStateChanged += OnWorkStateChanged;
        }
        startPos = transform.position;
        fish = new List<Fish>();
    }

    void OnDestroy()
    {
        if (hookMovement)
        {
            hookMovement.OnWorkingStateChanged -= OnWorkStateChanged;
        }
    }

    void OnEnable()
    {
        SetCurrentStack(0);

        int gfxId = PlayerPrefs.GetInt(UpgradeFishRod.MESH_NAME, 0);
        for (int i = 0; i < gfxs.Length; i++)
        {
            gfxs[i].SetActive(i == gfxId);
        }

        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();

        if (gfxId == 0)
        {
            capsuleCollider.radius = 0.24f;
        }
        else if (gfxId == 1)
        {
            capsuleCollider.radius = 0.28f;
        }
        else
        {
            capsuleCollider.radius = 0.21f;
        }
    }

    public void OnWorkStateChanged(bool isWorking)
    {
        canCollect = isWorking;
        if (fish.Count > 0 && !isWorking)
        {
            hookMovement.DisableMovement();
            float moveTime = Mathf.Lerp(0f, hookBackSpeed, Vector3.Distance(transform.position, startPos) / hookLength.Value);
            moveTime += hookCurrentStack.Value * 0.08f;
            transform.DOMove(startPos, moveTime).OnComplete(ReleaseFish);
        }
    }


    public void ReleaseFish()
    {
        for (int i = fish.Count - 1; i >= 0; i--)
        {
            if (fish[i] != null && fish[i].fishSO != null)
            {
                catchedFishArea.AddFish(fish[i].fishSO);
                fish[i].gameObject.SetActive(false);
                fish.RemoveAt(i);
            }
        }
        OnFishReleased?.Invoke();
        SetCurrentStack(0);
        AudioManager.Instance.PlayHookReleaseClipAt(transform.position);
        InGameUI.Instance.ShowTurnBackBtn();
        InGameUI.Instance.EnableTurnBack();
        transform.DORotate(new Vector3(0, 0, -90f), 0.2f).OnComplete(() => hookMovement.EnableMovement());
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canCollect)
            return;

        if (disableCollect)
            return;

        if (other.TryGetComponent<Fish>(out var newFish))
        {
            if (newFish.Capacity + hookCurrentStack.Value <= hookCapacity.Value)
            {
                newFish.Catch();
                newFish.transform.SetParent(fishCollectPos);
                newFish.transform.localPosition = Vector3.zero;
                fish.Add(newFish);
                SetCurrentStack(hookCurrentStack.Value + newFish.Capacity);
                InGameUI.Instance.CloseTurnBackBtn();
                InGameUI.Instance.DisableTurnBack();
            }
        }
    }

    public void SetCurrentStack(int stack)
    {
        hookCurrentStack.Value = stack;
        capacityTMP.SetText(stack.ToString() + " / " + hookCapacity.Value);

        if (stack == hookCapacity.Value)
        {
            hookMovement.StopWorking();
        }
        else if (fish.Count + catchedFishArea.CurrentAmount >= catchedFishArea.MaxStack)
        {
            hookMovement.StopWorking();
        }
    }

}
