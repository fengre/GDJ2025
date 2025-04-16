using UnityEngine;

public class NoteMover : MonoBehaviour
{
    public float moveSpeed = 300f;
    public int lane;
    public Color noteColor;    // This determines the group.
    public bool isHittable = false;
    public bool isHit = false;

    void Update()
    {
        // Move note downward.
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        // If the note went off-screen without being hit, register a miss.
        if (!isHit)
        {
            // Tell the GroupManager to decrease the group value.
            // GroupManager.Instance.ChangeGroupValue(noteColor, -GroupManager.Instance.missDecrease);
        }
        Destroy(gameObject);
    }
}
