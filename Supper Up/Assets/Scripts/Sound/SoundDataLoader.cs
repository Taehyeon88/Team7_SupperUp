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

    //한글 인코딩을 위한 핼퍼함수
    private string EncodeKorean(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";       //텍스트가 NULL값이면 함수를 끝낸다.
        byte[] bytes = Encoding.Default.GetBytes(text);  //string을 Byte배열로 변환한 후
        return Encoding.UTF8.GetString(bytes);           //인코딩을 UTF8롤 바꾼다.
    }

}
