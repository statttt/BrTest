using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayController : MonoBehaviour
{
    [SerializeField]private Text _textSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BoxCollider2D>())
        {
            _textSprite.text = collision.GetComponent<SpriteRenderer>().sprite.name;
        }
    }
}
