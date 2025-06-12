using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    public Material nightSkybox;
    public Light directLight;
    public Material daySkybox;

    private Color dayLight;
    private Color nightLight;

    private PlayerController player;
    private bool isOneTime = false;
    void Start()
    {
        RenderSettings.skybox = daySkybox;
        dayLight = directLight.color;
        nightLight = new Color32(44, 59, 118, 255);


        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y > 38f)  //�ʹݺο� �Ʒñ�������
        {
            if (!isOneTime)
            {
                if (SoundManager.instance != null)
                {
                    SoundManager.instance.FadeSound("DayTime", 1f);
                }

                isOneTime = true;
            }
        }
        else
        {
            if (isOneTime)
            {
                if (SoundManager.instance != null)
                    SoundManager.instance.FadeSound("DayTime", 0f);

                isOneTime = false;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RenderSettings.skybox = nightSkybox;  //��ī�̹ڽ� ����
            DynamicGI.UpdateEnvironment();

            directLight.color = nightLight;  //�� ����

            if (SoundManager.instance != null) SoundManager.instance.FadeSound("Evening", 1f);
            if (SoundManager.instance != null) SoundManager.instance.FadeSound("DayTime", 0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RenderSettings.skybox = daySkybox;  //��ī�̹ڽ� ����
            DynamicGI.UpdateEnvironment();

            directLight.color = dayLight;  //�� ����

            if (SoundManager.instance != null) SoundManager.instance.FadeSound("DayTime", 1f);
            if (SoundManager.instance != null) SoundManager.instance.FadeSound("Evening", 0f);
        }
    }
}
