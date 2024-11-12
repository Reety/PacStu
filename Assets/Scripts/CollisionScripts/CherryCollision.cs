using System;
using UnityEngine;

namespace CollisionScripts
{
    public class CherryCollision : MonoBehaviour
    {
        public static event Action OnCollision;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            OnCollision?.Invoke();
            Destroy(gameObject);
        }
    }
}
