using Unity.VisualScripting;
using UnityEngine;

namespace Movement
{
    public class ParticleController : MonoBehaviour
    {
        [SerializeField] ParticleSystem moveParticle;
        [SerializeField] private ParticleSystem deathParticle;

        private IPlayableCharacter pacStu;

        public bool emitParticle = false;
        
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
        void Awake()
        {
            pacStu = GetComponent<IPlayableCharacter>();
        }

        // Update is called once per frame
        void Update()
        {
            counter += Time.deltaTime;

            if (counter >= trailEmitPeriod && emitParticle)
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
            if (direction == Idle)
            {
                moveParticle.Stop();
                return;
            }
            
            moveParticle.Play();
        }

        public void StopParticle()
        {
            emitParticle = false;
            moveParticle.Stop();
            counter = 0;
        }

        public void PlayDeathParticle()
        {
            deathParticle.Play();
        }

        public void StartParticle() => emitParticle = true;

    }
}
