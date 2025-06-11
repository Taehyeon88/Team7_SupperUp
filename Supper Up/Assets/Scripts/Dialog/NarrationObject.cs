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
                case 3:  //우물에서 물 마시는 사운드
                    StartCoroutine(playSound("DrinkWater", 10f));
                break;

                case 5:  //밤이 되어서 늑대가 우는 사운드
                    StartCoroutine(playSound("WolfHowling", 10f));
                    break;

                case 7:  //보물을 발견해서 여는 사운드
                    StartCoroutine(playSound("Chest_Open", 7f));
                break;

                case 8:  //음식을 먹는 사운드
                    StartCoroutine(playSound("EatFood", 10f));
                break;

                case 9:  //대장장이 질하는 사운드
                    if (SoundManager.instance != null)              //사운드 이름 --> 클립 --> 리소스들 (이름으로 모든 리소스 받기)
                    {
                        AudioClip clip = SoundManager.instance.FindClipWithName("Anvil_Strike");
                        AudioSource[] sources = SoundManager.instance.FindSourcesWithClip(clip);

                        foreach (var sound in sources)
                        {
                            sound.Play();
                        }
                    }
                    break;

                case 10:  //도적 여럿을 쓰러 뜨리는 사운드
                    StartCoroutine(playSound("Enemy_Kill", 5f, true, 0.3f, 0.5f));
                    break;

                case 13:  //전쟁사운드
                    StartCoroutine(playSound("WarStart", 1f));
                    break;
            }
        }
    }

    private IEnumerator playSound( string soundName, float stayTime, bool playMore = false, float stayTime2 = 0, float stayTime3 = 0)
    {
        yield return new WaitForSeconds(stayTime);

        Debug.Log("들린다");

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
