using UnityEngine;

public class NoteMover : MonoBehaviour
{
    public float moveSpeed = 300f; // units per second

    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }
}
