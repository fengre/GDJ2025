// using UnityEngine;

// [RequireComponent(typeof(Animator))]
// public class GroupAnimatorController : MonoBehaviour
// {
//     public int groupIndex; // Index to get the correct group
//     private Animator animator;

//     void Start()
//     {
//         animator = GetComponent<Animator>();
//     }

//     void Update()
//     {
//         if (GroupManager.Instance == null || GroupManager.Instance.groups == null)
//             return;

//         if (groupIndex >= 0 && groupIndex < GroupManager.Instance.groups.Count)
//         {
//             Debug.Log(GroupManager.Instance.groups[groupIndex].groupName);
//             float value = GroupManager.Instance.groups[groupIndex].groupValue;
//             Debug.Log(value);

//             if (value < 25f || value > 75f)
//                 SetState("Panic");
//             else if ((value >= 25f && value < 45f) || (value > 55f && value <= 75f))
//                 SetState("Danger");
//             else
//                 SetState("Normal");
//         }
//     }

//     private void SetState(string stateName)
//     {
//         if (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
//         {
//             animator.Play(stateName);
//         }
//     }
// }


using UnityEngine;

public class GroupAnimatorController : MonoBehaviour
{
    public int groupIndex;
    private Animator animator;

    private float redZoneTimer = 0f;
    public float redZoneThreshold = 5f;

    private string currentState = "Normal";

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GroupManager.Instance == null || groupIndex >= GroupManager.Instance.groups.Count)
            return;

        float value = GroupManager.Instance.groups[groupIndex].groupValue;

        if (IsInRedZone(value))
        {
            redZoneTimer += Time.deltaTime;

            if (redZoneTimer >= redZoneThreshold && currentState != "Death")
            {
                SetState("Death");
                return; // Don't allow back out of death
            }
        }
        else
        {
            redZoneTimer = 0f; // reset if not in red
        }

        if (currentState == "Death") return; // lock into death once triggered

        if (value < 25f || value > 75f)
            SetState("Panic");
        else if (value < 45f || value > 55f)
            SetState("Danger");
        else
            SetState("Normal");
    }

    private void SetState(string newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        animator.Play(newState);
    }

    private bool IsInRedZone(float val) => val < 25f || val > 75f;
}

