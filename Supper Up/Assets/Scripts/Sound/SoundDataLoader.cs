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

    //�ѱ� ���ڵ��� ���� �����Լ�
    private string EncodeKorean(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";       //�ؽ�Ʈ�� NULL���̸� �Լ��� ������.
        byte[] bytes = Encoding.Default.GetBytes(text);  //string�� Byte�迭�� ��ȯ�� ��
        return Encoding.UTF8.GetString(bytes);           //���ڵ��� UTF8�� �ٲ۴�.
    }

}
