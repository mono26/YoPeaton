using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableObject : MonoBehaviour
{
    private SpriteRenderer renderer;

    [SerializeField]
    private Color visibleColor;
    [SerializeField]
    private Color hiddenColor;
    // Start is called before the first frame update
    void Start()
    {
        visibleColor = new Color(1, 1, 1, 1);
        hiddenColor = new Color(1, 1, 1, 0.25f);
        renderer = this.GetComponent<SpriteRenderer>();
        renderer.color = visibleColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("COLISION DE: "+ this.gameObject.name + ", CON: " + collision.name);
        if (collision.CompareTag("PlayerCar"))
        {
            renderer.color = hiddenColor;
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
