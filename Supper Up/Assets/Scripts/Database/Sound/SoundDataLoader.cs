using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;

public class SoundDataLoader : MonoBehaviour
{
    [SerializeField]
    private string jsonFileName = "SoundSheet";

    private List<SoundData> soundList;

    private void Start()
    {
        LoadSoundData();
    }

    //�ѱ� ���ڵ��� ���� �����Լ�
    private string EncodeKorean(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";       //�ؽ�Ʈ�� NULL���̸� �Լ��� ������.
        byte[] bytes = Encoding.Default.GetBytes(text);  //string�� Byte�迭�� ��ȯ�� ��
        return Encoding.UTF8.GetString(bytes);           //���ڵ��� UTF8�� �ٲ۴�.
    }

    void LoadSoundData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);

        if (jsonFile != null)
        {
            //���� �ؽ�Ʈ���� UTF-8�� ��ȯ ó��
            byte[] bytes = Encoding.Default.GetBytes(jsonFile.text);
            string currentText = Encoding.UTF8.GetString(bytes);

            //��ȯ�� �ؽ�Ʈ ���
            soundList = JsonConvert.DeserializeObject<List<SoundData>>(currentText);

            Debug.Log($"�ε�� ���� �� : {soundList.Count}");

            foreach (var sound in soundList)
            {
                Debug.Log($"����: {EncodeKorean(sound.id)}");
            }

        }
        else
        {
            Debug.LogError($"JSON������ ã�� �� �����ϴ�. : {jsonFileName}");
        }
    }
}
