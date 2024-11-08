using UnityEngine;

namespace Movement
{
    public class ParticleController : MonoBehaviour
    {
        [SerializeField] ParticleSystem moveParticle;

        [SerializeField] private PacStudentController pacStu;
        
        private Vector3 
            movingRight = new Vector3(0, 0, 90),
            movingDown = Vector3.zero,
            movingLeft = new Vector3(0, 0, -90),
            movingUp = new Vector3(0, 0, 180);

        private readonly int
            WalkDown = Animator.StringToHash("WalkDown"),
            WalkUp = Animator.StringToHash("WalkUp"),
            WalkRight = Animator.StringToHash("WalkRight"),
            WalkLeft = Animator.StringToHash("WalkLeft"),
            Idle = Animator.StringToHash("Idle");
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (pacStu.CurrentState == Idle)
            {
                moveParticle.Stop();
                return;
            }
            
            ParticleSettings();
        }

        private void ParticleSettings(int direction)
        {
            var particleShape = moveParticle.shape;
            if (direction == WalkDown) particleShape.rotation = movingDown;
            if (direction == WalkLeft) particleShape.rotation = movingLeft;
            if (direction == WalkRight) particleShape.rotation = movingRight;
            if (direction == WalkUp) particleShape.rotation = movingUp;
            
            if (moveParticle.isStopped) moveParticle.Play();
            
            return;
        }
    }
}
