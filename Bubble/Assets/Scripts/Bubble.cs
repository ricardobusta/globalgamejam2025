using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bubble : MonoBehaviour
    {
        public bool released;

        public bool popped;
        
        private void Awake()
        {
            released = false;
        }

        private void Update()
        {
            if (popped)
            {
                
            }else if (released)
            {
                transform.position += Vector3.up * Time.deltaTime;

                if (transform.position.y > 5)
                {
                    popped = true;
                    gameObject.SetActive(false);
                }
            }
        }
    }
}