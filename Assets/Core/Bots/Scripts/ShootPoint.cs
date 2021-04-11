using UnityEngine;

public class ShootPoint : MonoBehaviour
{
    [SerializeField] Rigidbody2D rbody;

    public void SetPosition(Transform target, float y, float shootDistance)
    {
        Vector2 spawnPosition = Vector2.right * shootDistance * (Random.Range(0, 2) == 0 ? 1 : -1);
        spawnPosition.x += target.position.x;
        spawnPosition.y = y + 1;

        if (spawnPosition.x > 49)
            spawnPosition.x = 49;
        else if (spawnPosition.x < -49)
            spawnPosition.x = -49;

        transform.position = spawnPosition;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
}
