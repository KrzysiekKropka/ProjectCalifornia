using UnityEngine;
using TMPro;

public class MoneyDrop : MonoBehaviour
{
    [SerializeField] int rotationSpeed;
    [SerializeField] AudioClip MoneyDropClip;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] Rigidbody2D rb;
    private float rotationZ;
    public int money;

    void Start()
    {
        moneyText.text = money + "$";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(MoneyDropClip, transform.position, 1f);
            collision.GetComponent<Player>().SetMoney(money);
            Destroy(gameObject);
        }
    }
}
