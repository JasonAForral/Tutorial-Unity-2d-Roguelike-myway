using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    public Sprite dmgSprite;
    public int hp = 4;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRendererChild;

    void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRendererChild = GetComponentInChildren<SpriteRenderer>();
    }

    public void DamageWall (int loss)
    {
        spriteRenderer.sprite = dmgSprite;
        spriteRendererChild.sprite = dmgSprite;
        hp -= loss;
        if (0 >= hp)
            gameObject.SetActive(false);
    }
}
