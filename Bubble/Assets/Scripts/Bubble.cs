using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private YieldInstruction delay;

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
            delay = new WaitForSeconds(particleSystemPrefab.main.duration);
        }

        private void Update()
        {
            if (popped)
            {
                PopBubble();
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

        public async void PopBubble()
        {
            gameObject.SetActive(false);
            bubblePool.Push(this);

            var particle = particlePool.Count > 0
                ? particlePool.Pop()
                : Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);
            particle.gameObject.SetActive(true);
            particle.transform.localScale = transform.localScale * 0.2f;
            particle.transform.position = transform.position;
            particle.Play();
            particle.GetComponent<AudioSource>().Play();

            await Task.Delay(Mathf.RoundToInt(particleSystemPrefab.main.duration*1000));
            
            particle.gameObject.SetActive(false);
            particlePool.Push(particle);
        }
    }
}