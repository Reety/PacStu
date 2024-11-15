using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIScripts
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Animator buttonAnim;

        public event Action<int> ButtonAnimOver;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public IEnumerator ButtonAnimate(int sceneIndex)
        {
            //waits for animations to finish before starting the next one
            AnimatorStateInfo currState = buttonAnim.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(currState.length);
            AnimatorStateInfo newState = buttonAnim.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(newState.length);
            
            ButtonAnimOver?.Invoke(sceneIndex);
        }
    }
}
