using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    

    public void UpdateHealthBar(float current , float max)
    {
        slider.value = current / max;
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
