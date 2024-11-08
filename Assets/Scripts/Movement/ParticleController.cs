using UnityEngine;

namespace Movement
{
    public class ParticleController : MonoBehaviour
    {
        [SerializeField] ParticleSystem moveParticle;

        [SerializeField] private PacStudentController pacStu;

        private float trailEmitPeriod = 0.05f;
        private float counter = 0;
        
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
            counter += Time.deltaTime;
            
            if (pacStu.CurrentState == Idle)
            {
                moveParticle.Stop();
                return;
            }

            if (counter >= trailEmitPeriod)
            {
                ParticleSettings(pacStu.CurrentState);
                counter = 0;
            }

        }

        private void ParticleSettings(int direction)
        {
            var particleShape = moveParticle.shape;
            if (direction == WalkDown) particleShape.rotation = movingDown;
            if (direction == WalkLeft) particleShape.rotation = movingLeft;
            if (direction == WalkRight) particleShape.rotation = movingRight;
            if (direction == WalkUp) particleShape.rotation = movingUp;
            
            moveParticle.Play();
            
            return;
        }
    }
}
