using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bubble : MonoBehaviour
    {
        public bool released;

        public bool popped;

        public static Stack<Bubble> bubblePool = new();
        public static Stack<ParticleSystem> particlePool = new();

        private float maxHeight;

        public ParticleSystem particleSystemPrefab;

        public static Bubble GetBubble(Bubble prefab, Vector2 position)
        {
            var bubble = bubblePool.Count == 0
                ? Instantiate(prefab, position, Quaternion.identity)
                : bubblePool.Pop();
            bubble.popped = false;
            bubble.released = false;
            bubble.maxHeight = Mathf.Min(position.y + Random.Range(2f, 5f), 7f);
            bubble.transform.position = position;
            bubble.gameObject.SetActive(true);
            return bubble;
        }

        private void Awake()
        {
            released = false;
        }

        private void Update()
        {
            if (popped)
            {
                StartCoroutine(PopBubble());
            }
            else if (released)
            {
                transform.position += Vector3.up * Time.deltaTime;

                if (transform.position.y > maxHeight)
                {
                    popped = true;
                }
            }
        }

        public IEnumerator PopBubble()
        {
            gameObject.SetActive(false);
            bubblePool.Push(this);

            var particle = particlePool.Count > 0
                ? particlePool.Pop()
                : Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);
            particle.Play();

            yield return new WaitForSeconds(particle.main.duration);
            
            particle.gameObject.SetActive(false);
            particlePool.Push(particle);
        }
    }
}