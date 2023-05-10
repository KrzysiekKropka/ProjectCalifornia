using UnityEngine;

public class DeadBodyFollow : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Sprite[] sprites;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipY = true;
    }

    public void DeadBodyPosition(int bodyColor, bool isDreamy = false)
    {
        if(!isDreamy)spriteRenderer.sprite = sprites[bodyColor];

        transform.position = enemy.transform.position;
        transform.rotation = enemy.transform.rotation;
        if (Random.Range(0, 2) == 1) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }

    public void DestroyItself()
    {
        Destroy(gameObject);
    }
}
