// File: TapNoteMover.cs
using UnityEngine;

/// <summary>
/// A simple “tap” note: hit once when it passes the judgment line.
/// </summary>
public class TapNoteMover : NoteMover
{

    public override HitRating? TryTapHit(
        float hitThreshold,
        float greatThreshold,
        float perfectThreshold,
        float judgmentY)
    {
        if (!isHittable || isHit) return null;

        float dist = Mathf.Abs(transform.position.y - judgmentY);
        if (dist > hitThreshold) return null;

        isHit = true;
        Destroy(gameObject);

        if (dist <= perfectThreshold) return HitRating.Perfect;
        if (dist <= greatThreshold)   return HitRating.Great;
        return HitRating.Good;
    }

    public override bool TryBeginHold(
        double currentDspTime,
        float hitThreshold,
        float judgmentY)
    {
        // taps never begin holds
        return false;
    }

    public override HitRating EndHold(double currentDspTime)
    {
        // taps never end holds
        return HitRating.Miss;
    }
}
