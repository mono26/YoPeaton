using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableObject : MonoBehaviour
{
    private SpriteRenderer renderer;

    [SerializeField]
    private Color visibleColor;
    [SerializeField]
    private Color hidedColor;
    // Start is called before the first frame update
    void Start()
    {
        renderer = this.GetComponent<SpriteRenderer>();
        renderer.color = visibleColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerCar"))
        {
            renderer.color = hidedColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerCar"))
        {
            renderer.color = visibleColor;
        }
    }
}
