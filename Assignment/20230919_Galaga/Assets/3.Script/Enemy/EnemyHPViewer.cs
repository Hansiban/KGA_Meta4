using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPViewer : MonoBehaviour
{
    private EnemyControl enemy;
    private Slider slider;

    public void Setup(EnemyControl enemy)
    {
        this.enemy = enemy;
        TryGetComponent(out slider);
    }

    private void Update()
    {
        slider.value = enemy.CURRENTHP / enemy.MAXHP;
    }

}
