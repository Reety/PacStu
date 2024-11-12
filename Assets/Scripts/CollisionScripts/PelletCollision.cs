using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CollisionScripts
{
    public class PelletCollision : MonoBehaviour
    {
        private Tilemap pelMap;
        public static event Action OnCollision;
        // Start is called before the first frame update

        private void Awake()
        {
            pelMap = GetComponent<Tilemap>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            Vector3Int pelPos = Vector3Int.FloorToInt(other.GetContact(0).point);
            if (pelMap.HasTile(pelPos))
            {
                pelMap.SetTile(pelPos, null);
                OnCollision?.Invoke();
            }
        }
    }
}