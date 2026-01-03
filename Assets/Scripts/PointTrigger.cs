using UnityEngine;

public class PointTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BirdController>() != null)
        {
            GameManager.Instance.IncreaseScore();
        }
    }
}