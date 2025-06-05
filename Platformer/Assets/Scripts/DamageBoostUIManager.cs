using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DamageBoostUIManager : MonoBehaviour
{
    public static DamageBoostUIManager instance;

    public TextMeshProUGUI damageBoostText_Player1;
    public TextMeshProUGUI damageBoostText_Player2;
    public Image damageBoostImage_Player1;
    public Image damageBoostImage_Player2;

    private float timer_Player1 = 0f;
    private float timer_Player2 = 0f;

    private bool isCounting_Player1 = false;
    private bool isCounting_Player2 = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        if (isCounting_Player1)
        {
            timer_Player1 -= Time.deltaTime;
            damageBoostText_Player1.text = "Damage Boost: " + Mathf.Ceil(timer_Player1).ToString() + "s";

            if (timer_Player1 <= 0)
            {
                damageBoostText_Player1.gameObject.SetActive(false);
                damageBoostImage_Player1.gameObject.SetActive(false);
                isCounting_Player1 = false;
            }
        }

        if (isCounting_Player2)
        {
            timer_Player2 -= Time.deltaTime;
            damageBoostText_Player2.text = "Damage Boost: " + Mathf.Ceil(timer_Player2).ToString() + "s";

            if (timer_Player2 <= 0)
            {
                damageBoostText_Player2.gameObject.SetActive(false);
                damageBoostImage_Player2.gameObject.SetActive(false);
                isCounting_Player2 = false;
            }
        }
    }

    public void StartBoostTimer(float duration, string playerTag)
    {
        if (playerTag == "Player1")
        {
            timer_Player1 = duration;
            isCounting_Player1 = true;

            damageBoostText_Player1.gameObject.SetActive(true);
            damageBoostImage_Player1.gameObject.SetActive(true);
        }
        else if (playerTag == "Player2")
        {
            timer_Player2 = duration;
            isCounting_Player2 = true;

            damageBoostText_Player2.gameObject.SetActive(true);
            damageBoostImage_Player2.gameObject.SetActive(true);
        }
    }
}
