using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationObject : MonoBehaviour
{
    [SerializeField] private int id;

    private bool isOneTime = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOneTime)
        {
            if (StoryManager.Instance != null)
            {
                StoryManager.Instance.StartNarration(id);
                Debug.Log("OK");
            }
            isOneTime = true;


            switch (id)
            {
                case 3:  //�칰���� �� ���ô� ����
                    StartCoroutine(playSound("DrinkWater", 10f));
                break;

                case 5:  //���� �Ǿ ���밡 ��� ����
                    StartCoroutine(playSound("WolfHowling", 10f));
                    break;

                case 7:  //������ �߰��ؼ� ���� ����
                    StartCoroutine(playSound("Chest_Open", 7f));
                break;

                case 8:  //������ �Դ� ����
                    StartCoroutine(playSound("EatFood", 10f));
                break;

                case 9:  //�������� ���ϴ� ����
                    if (SoundManager.instance != null)              //���� �̸� --> Ŭ�� --> ���ҽ��� (�̸����� ��� ���ҽ� �ޱ�)
                    {
                        AudioClip clip = SoundManager.instance.FindClipWithName("Anvil_Strike");
                        AudioSource[] sources = SoundManager.instance.FindSourcesWithClip(clip);

                        foreach (var sound in sources)
                        {
                            sound.Play();
                        }
                    }
                    break;

                case 10:  //���� ������ ���� �߸��� ����
                    StartCoroutine(playSound("Enemy_Kill", 5f, true, 0.3f, 0.5f));
                    break;

                case 13:  //�������
                    StartCoroutine(playSound("WarStart", 1f));
                    break;
            }
        }
    }

    private IEnumerator playSound( string soundName, float stayTime, bool playMore = false, float stayTime2 = 0, float stayTime3 = 0)
    {
        yield return new WaitForSeconds(stayTime);

        Debug.Log("�鸰��");

        if (SoundManager.instance != null) SoundManager.instance.PlaySound(soundName);

        if (playMore)
        {
            yield return new WaitForSeconds(stayTime2);

            if (SoundManager.instance != null) SoundManager.instance.PlaySound(soundName);

            yield return new WaitForSeconds(stayTime3);

            if (SoundManager.instance != null) SoundManager.instance.PlaySound(soundName);
        }
    }
}
