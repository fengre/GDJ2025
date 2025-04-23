// File: HoldNoteMover.cs
using UnityEngine;

/// <summary>
/// A “hold” note: press when the head enters the line, release after its duration.
/// </summary>
public class HoldNoteMover : NoteMover
{
    private bool isHolding = false;
    private double holdStartTime;
    private float originalBodyHeight;
    private float originalTailY;

    public Transform body, tail;

    public override void Initialize(NoteData data, int groupIndex, float speed, Color color)
    {
        base.Initialize(data, groupIndex, speed, color);

        // HEAD
        var headSR = head.GetComponent<SpriteRenderer>();
        headSR.color = color;
        float headH = headSR.sprite.bounds.size.y * head.localScale.y;

        // Place head at center origin
        head.localPosition = Vector3.zero;

        // TAIL
        var tailSR = tail.GetComponent<SpriteRenderer>();
        tailSR.color = color;
        float tailH = tailSR.sprite.bounds.size.y * tail.localScale.y;

        // Calculate travel distance and position tail
        float travelDist = speed * duration;
        tail.localPosition = new Vector3(0f, travelDist, 0f);

        // BODY
        var bodySR = body.GetComponent<SpriteRenderer>();
        bodySR.color = color;
        float spriteBodyH = bodySR.sprite.bounds.size.y;

        // Scale and position body
        float desiredBodyH = travelDist;
        float scaleY = desiredBodyH / spriteBodyH;
        body.localScale = new Vector3(body.localScale.x, scaleY, body.localScale.z);
        body.localPosition = new Vector3(0f, desiredBodyH * 0.5f, 0f);
    }

    public override void GrayOut()
    {
        base.GrayOut();
        // gray out hold visuals
        ApplyColor(body, Color.gray);
        ApplyColor(tail, Color.gray);
    }

    public override HitRating? TryTapHit(
        float hitThreshold,
        float greatThreshold,
        float perfectThreshold,
        float judgmentY)
    {
        // hold notes don’t respond to taps
        return null;
    }

    public override bool TryBeginHold(double songTime, float hitThreshold, float judgmentY)
    {
        if (isHit) return false;

        float distance = Mathf.Abs(transform.position.y - judgmentY);
        if (distance <= hitThreshold)
        {
            isHolding = true;
            isHit = true; // Mark the note as hit
            Debug.Log("Hold note hit!");
            holdStartTime = songTime;
            originalBodyHeight = body.localScale.y;
            originalTailY = tail.localPosition.y;

            return true;
        }
        return false;
    }

    public override HitRating EndHold(double songTime)
    {
        isHolding = false;

        // Your custom hold evaluation logic here:
        double heldDuration = songTime - holdStartTime;
        float ratio = (float)(heldDuration / duration);

        if (ratio >= 0.95f) return HitRating.Perfect;
        if (ratio >= 0.75f) return HitRating.Great;
        if (ratio >= 0.5f)  return HitRating.Good;
        return HitRating.Miss;
    }

    protected override void Update()
    {
        base.Update();

        if (isHolding)
        {
            double elapsed = GameManager.Instance.GetSongTime() - holdStartTime;
            float shrinkRatio = Mathf.Clamp01((float)(elapsed / duration));

            if (head != null)
            {
                Destroy(head.gameObject);
                head = null;
            }

            if (body == null)
            {
                isHolding = false;
                return;
            }

            Vector3 judgmentLineLocal = transform.InverseTransformPoint(
                NoteManager.Instance.judgmentLine.position
            );
            float newBodyHeight = Mathf.Lerp(originalBodyHeight, 0f, shrinkRatio);
            float bottomY = judgmentLineLocal.y;
            float centerY = bottomY + (newBodyHeight * 0.5f);

            // Update body
            var spriteH = body.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
            body.localScale = new Vector3(body.localScale.x, newBodyHeight / spriteH, body.localScale.z);
            body.localPosition = new Vector3(0f, centerY, 0f);

            // Keep tail connected to top of body
            if (tail != null)
            {
                float topY = centerY + newBodyHeight * 0.5f;
                tail.localPosition = new Vector3(0f, topY, 0f);

                if (shrinkRatio > 0.99f)
                {
                    tail.gameObject.SetActive(false);
                    tail = null;
                }
            }

            if (shrinkRatio >= 0.99f)
            {
                body.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!isHit && tail != null &&
                tail.transform.position.y < NoteManager.Instance.judgmentLine.position.y - 2f)
            {
                Destroy(gameObject);
            }
        }
    }

    
}