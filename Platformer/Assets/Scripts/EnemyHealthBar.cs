using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void UpdateHealthBar(float current , float max)
    {
        slider.value = current / max;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
