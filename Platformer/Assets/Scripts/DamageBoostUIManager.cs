using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DamageBoostUIManager : MonoBehaviour
{
    public static DamageBoostUIManager instance;

    public TextMeshProUGUI damageBoostText;
    public Image damageBoostImage; 

    private float timer = 0f;
    private bool isCounting = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        if (isCounting)
        {
            timer -= Time.deltaTime;
            damageBoostText.text = "Damage Boost: " + Mathf.Ceil(timer).ToString() + "s";

            if (timer <= 0)
            {
                damageBoostText.gameObject.SetActive(false);
                damageBoostImage.gameObject.SetActive(false);
                isCounting = false;
            }
        }
    }

    public void StartBoostTimer(float duration)
    {
        timer = duration;
        isCounting = true;

        damageBoostText.gameObject.SetActive(true);
        damageBoostImage.gameObject.SetActive(true);

        damageBoostText.text = "Damage Boost: " + Mathf.Ceil(timer).ToString() + "s";
    }
}
