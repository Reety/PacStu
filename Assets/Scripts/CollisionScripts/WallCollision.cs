using LevelScripts;
using UnityEngine;

namespace CollisionScripts
{
    public class WallCollision : MonoBehaviour
    {
        [SerializeField] private ParticleSystem wallParticles;

        // Start is called before the first frame update

        private void Awake()
        {
            wallParticles.Pause();
        
        }

        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
        
            if (!other.gameObject.CompareTag("Player")) return;

            
            Vector3 collisionPoint = other.GetContact(0).point;
           
            wallParticles.transform.position = collisionPoint;
            wallParticles.Play();
        }
    }
}
