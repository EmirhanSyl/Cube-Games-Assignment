using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressBarManager : MonoBehaviour
{
    [SerializeField] Gradient gradient;

    private Image fill;
    private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        fill = transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    public void SetProgress(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
