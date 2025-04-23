using UnityEngine;

public abstract class NoteMover : MonoBehaviour
{
    public int lane, groupIndex;
    protected float moveSpeed;
    public float duration;
    public bool isHittable, isHit;

    public Transform head;

    public virtual void Initialize(NoteData data, int group, float speed, Color color)
    {
        lane       = data.lane;
        groupIndex = group;
        moveSpeed  = speed;
        duration = data.duration;
        ApplyColor(head, color);

        var headCollider = head.GetComponent<BoxCollider2D>();
        if (headCollider != null)
        {
            float threshold = NoteManager.Instance.hitThreshold; // or InputHandler.Instance.hitThreshold;
            headCollider.size = new Vector2(headCollider.size.x, threshold * 2f);
            headCollider.offset = new Vector2(0f, 0f); // or tweak if needed
        }
    }

    public virtual void GrayOut()
    {
        ApplyColor(head, Color.gray);
        GetComponent<Collider2D>().enabled = false;
    }

    protected void ApplyColor(Transform t, Color c)
    {
        if (t == null) return;
        var sr = t.GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = c;
    }

    protected virtual void Update() => transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    public void SetHittable(bool v) => isHittable = v;

    void OnBecameInvisible()
    {
        if (!isHit) ScoreManager.Instance.RegisterMiss();
        Destroy(gameObject);
    }

    public abstract HitRating? TryTapHit(float hitThres, float greatThres, float perfThres, float judgmentY);
    public abstract bool TryBeginHold(double dspTime, float hitThres, float judgmentY);
    public abstract HitRating EndHold(double dspTime);
}
