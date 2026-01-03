using UnityEngine;

public class PipeMovement : MonoBehaviour
{
   
    [SerializeField] private float destroyX = -10f;

    private void Update()
    {
        // Artık hızı DifficultyManager'dan alıyoruz
        float currentSpeed = DifficultyManager.Instance.currentMoveSpeed;
        
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}