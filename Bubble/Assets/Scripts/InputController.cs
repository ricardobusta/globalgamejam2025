using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Camera camera;
    public Bubble bubblePrefab;

    private Bubble bubble;

    private bool blowing;
    private float scale;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        bubble = Instantiate(bubblePrefab, camera.transform.position + ray.direction * 10, Quaternion.identity);
        blowing = true;
        scale = 0.01f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bubble.released = true;
        bubble = null;
        blowing = false;
    }

    private void Update()
    {
        if (blowing)
        {
            scale += 1 * Time.deltaTime;
            bubble.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
