using UnityEngine;

public class BuffFX : MonoBehaviour
{
    [SerializeField] private float duration = 3f;
    [SerializeField] private float moveUpDistance = 1f;

    private Vector3 startPosition;
    private float elapsedTime;

    private void Start()
    {
        startPosition = transform.position;
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        transform.position = startPosition + Vector3.up * moveUpDistance * t;
    }
}
