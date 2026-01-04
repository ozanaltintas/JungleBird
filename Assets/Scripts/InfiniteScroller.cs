using UnityEngine;

public class InfiniteScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2f; // Kayma hızı
    [SerializeField] private float width = 20f;     // Bir resmin tam genişliği

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        // Zamanla sola doğru kaydır
        float newPos = Mathf.Repeat(Time.time * scrollSpeed, width);
        transform.position = startPosition + Vector3.left * newPos;
    }
}