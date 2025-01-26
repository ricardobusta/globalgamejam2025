using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Camera camera;
    public Bubble bubblePrefab;

    private Stack<Bubble> bubblePool;

    public AudioSource audioSource;

    private Bubble bubble;

    private bool blowing;
    private float scale;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        var pos = camera.transform.position + ray.direction * 10;
        bubble = Bubble.GetBubble(bubblePrefab, pos);
        blowing = true;
        audioSource.volume = 1;
        audioSource.Play();
        scale = 0.01f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        bubble.released = true;
        bubble = null;
        blowing = false;
        audioSource.volume = 0;
        audioSource.Stop();
    }

    private void Update()
    {
        if (blowing)
        {
            scale += 1 * Time.deltaTime;
            bubble.transform.localScale = new Vector3(scale, scale, scale);

            audioSource.pitch = (Mathf.Pow(scale / 5f, 0.5f) + 1f)/2f;

            if (scale > 5)
            {
                bubble.popped = true;
                blowing = false;
                audioSource.volume = 0;
                audioSource.Stop();
            }
        }
    }
}