
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private Rigidbody rb;

    public bool throwBullet;


    private float _currentTime;


    private void FixedUpdate()
    {
        if (!throwBullet)
        {
            return;
        }
        rb.velocity = transform.forward * speed;
        if (_currentTime <= lifeTime)
        {
            _currentTime += Time.fixedDeltaTime;
            return;
        }
        LifeTimeFinish();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health stateHandler))
        {
            stateHandler.TakeDamage(20);
        }

        LifeTimeFinish();

    }

    private void LifeTimeFinish()
    {
        Destroy(gameObject, 1f);
    }
}
