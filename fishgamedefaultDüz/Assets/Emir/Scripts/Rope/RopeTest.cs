using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

public class RopeTest : MonoBehaviour
{
    public Transform startPos;
    // private GameObject ropePartPrefab; // Prefab for the rope part
    public Transform hookPosition;    // The position of the hook

    // public int poolLength = 10;       // Number of rope parts to spawn
    // public float ropeSpacing = 0.2f; // Distance between rope parts

    // [SerializeField] List<Joint> ropeParts;
    [SerializeField] TextMeshPro maxLengthTMP;
    private LineRenderer lineRenderer;
    [SerializeField] TextMeshProUGUI upgradeWarnTMP;
    // private Rigidbody hookRB;
    // int ropeLengthCurrent;

    Color2 col;
    Color2 colTar;

    public float activationDistance = 2.0f; // Distance from the hook for activation

    bool isChangingColor;

    bool passedStart;


    public void WarnLength()
    {
        if (isChangingColor)
            return;

        isChangingColor = true;
        AudioManager.Instance.PlayNegativeClipAt(hookPosition.position);
        maxLengthTMP.transform.position = hookPosition.position + new Vector3(0, 1, 0);
        maxLengthTMP.transform.DOShakePosition(0.3f, 0.1f, 10, 90);
        lineRenderer.DOColor(col, colTar, 0.7f).OnComplete(ResetColor);
        maxLengthTMP.DOFade(1, 0.1f);

        if (passedStart)
            return;

        
        PlayerPrefs.SetInt("passedRopeLength", 1);
        upgradeWarnTMP.DOFade(1, 0.1f);
        transform.DOScale(transform.localScale, 2f).OnComplete(CloseWarn);
    }

    private void ResetColor()
    {
        lineRenderer.DOColor(colTar, col, 1f).SetEase(Ease.InCubic).OnComplete(() => isChangingColor = false);
        maxLengthTMP.DOFade(0, 0.7f);
    }

    private void CloseWarn()
    {
        passedStart = true;
        upgradeWarnTMP.DOFade(0, 1f);
    }

    void Awake()
    {
        passedStart = PlayerPrefs.GetInt("passedRopeLength", 0) == 1;
        // ropeParts = new List<Joint>();
        lineRenderer = GetComponent<LineRenderer>();

        col = new Color2();
        col.ca = lineRenderer.startColor;
        col.cb = lineRenderer.endColor;

        colTar = new Color2();
        colTar.ca = Color.red;
        colTar.cb = Color.red;
        /* hookRB = hookPosition.GetComponent<Rigidbody>();

        // Spawn rope parts and set them inactive
        for (int i = 0; i < poolLength; i++)
        {
            Vector3 spawnPosition = transform.position - Vector3.up * i * ropeSpacing;
            ropeParts.Add(Instantiate(ropePartPrefab, spawnPosition, Quaternion.identity).GetComponent<Joint>());
            ropeParts[i].gameObject.SetActive(false);
        }

        // Initially set ropeLengthCurrent to the number of active rope segments
        ropeLengthCurrent = CountActiveRopeSegments();

        ropeParts[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        // Connect the rope parts
        ConnectRopeSegments(); */
    }

    void LateUpdate()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos.position);
        lineRenderer.SetPosition(1, hookPosition.position);
    }

    /* void LateUpdate()
    {
        // Calculate the distance between the rope start and the hook
        float distance = transform.position.y - hookPosition.position.y;
        distance = Mathf.Abs(distance);


        // Calculate the number of rope segments to activate based on distance
        int segmentsToActivate = Mathf.FloorToInt(distance / ropeSpacing) + 1;

        // Ensure segmentsToActivate is within the valid range
        segmentsToActivate = Mathf.Clamp(segmentsToActivate, 0, poolLength);

        // Activate the appropriate number of rope segments
        for (int i = 0; i < ropeParts.Count; i++)
        {
            if (!ropeParts[i].gameObject.activeSelf && i - 1 > -1)
            {
                ropeParts[i].transform.position = (hookPosition.position - ropeParts[i - 1].transform.position) / 2 + ropeParts[i - 1].transform.position;
            }   
            bool setActive = i < segmentsToActivate;
            ropeParts[i].gameObject.SetActive(setActive);
            if (!setActive)
            {
                ropeParts[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                break;
            }
                
        }

        // Update ropeLengthCurrent based on the active count of rope segments
        ropeLengthCurrent = CountActiveRopeSegments();

        // Update Line Renderer
        UpdateLineRenderer();
        ConnectRopeSegments();
    } */

    // ...

    // Update Line Renderer
    /* void UpdateLineRenderer()
    {
        lineRenderer.positionCount = ropeLengthCurrent + 1;
        for (int i = 0; i < ropeLengthCurrent; i++)
        {
            lineRenderer.SetPosition(i, ropeParts[i].transform.position);
        }
        lineRenderer.SetPosition(ropeLengthCurrent, hookPosition.position);
    } */



    // Count the number of active rope segments
    /* public int CountActiveRopeSegments()
    {
        int count = 0;
        foreach (Joint ropePart in ropeParts)
        {
            if (ropePart.gameObject.activeSelf)
            {
                count++;
            }
            else
            {
                return count;
            }
        }
        return count;
    } */

    // Connect the active rope segments
    /* void ConnectRopeSegments()
    {
        for (int i = 0; i < ropeLengthCurrent - 1; i++)
        {
            ropeParts[i].connectedBody = ropeParts[i + 1].GetComponent<Rigidbody>();
        }



        if (ropeLengthCurrent > 0)
        {
            ropeParts[ropeLengthCurrent - 1].connectedBody = hookRB;
        }
    } */
}