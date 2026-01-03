using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 1.5f;
    private float destroyX = -15f;

    private void Update()
    {
        float currentSpeed = DifficultyManager.Instance.currentMoveSpeed * speedMultiplier;

        // DÜZELTME BURADA: 
        // Sona eklediğimiz "Space.World" sayesinde taş dönse bile dümdüz sola gider.
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime, Space.World);

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}