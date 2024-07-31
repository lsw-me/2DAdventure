using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    private Character currentCharactera;
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;

    private bool isRecovering;
    private void Update()
    {
        if(healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -=Time.deltaTime;
        }
        if(isRecovering)
        {
            float persentage = currentCharactera.currentPower / currentCharactera.maxPower;
            powerImage.fillAmount = persentage;

            if(persentage >=1)
            {
                isRecovering = false;
                return;
            }
        }
    }

    /// <summary>
    ///  ����Health�ı���ٷֱ�
    /// </summary>
    /// <param name="persentage">�ٷֱȣ�Current/Max</param>


    public void OnHealthChange(float persentage)
    {
        healthImage.fillAmount = persentage;    
    } 
    public void OnPowerChange(Character character)
    {
        isRecovering = true;
        currentCharactera = character;

    }
}
