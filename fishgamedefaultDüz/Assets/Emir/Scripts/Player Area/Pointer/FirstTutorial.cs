using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTutorial : MonoBehaviour
{
    [SerializeField] GameObject leaveBtn;
    [SerializeField] CatchedFishArea catchedFishArea;
    [SerializeField] GameObject secondTutorial;
    [SerializeField] HookMovement hookMovement;
    bool isDone;

    private void Awake() {
        isDone = PlayerPrefs.GetInt("pointerPhase", 0) > 0;
        if (isDone)
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if (catchedFishArea.fishCount < 6)
        {
            leaveBtn.SetActive(false);
        }
        else if (!isDone)
        {
            isDone = true;
            leaveBtn.SetActive(false);
            hookMovement.DisableMovement();
            secondTutorial.SetActive(true);
        }
        else
        {
            hookMovement.DisableMovement();
        }
    }

    public void OnTutorialLeavePressed()
    {
        hookMovement.EnableMovement();
        secondTutorial.SetActive(false);
        leaveBtn.SetActive(true);
        Destroy(gameObject);
    }
}
