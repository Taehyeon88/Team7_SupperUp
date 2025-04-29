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

    //한글 인코딩을 위한 핼퍼함수
    private string EncodeKorean(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";       //텍스트가 NULL값이면 함수를 끝낸다.
        byte[] bytes = Encoding.Default.GetBytes(text);  //string을 Byte배열로 변환한 후
        return Encoding.UTF8.GetString(bytes);           //인코딩을 UTF8롤 바꾼다.
    }

    void LoadSoundData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);

        if (jsonFile != null)
        {
            //원본 텍스트에서 UTF-8로 변환 처리
            byte[] bytes = Encoding.Default.GetBytes(jsonFile.text);
            string currentText = Encoding.UTF8.GetString(bytes);

            //변환된 텍스트 사용
            soundList = JsonConvert.DeserializeObject<List<SoundData>>(currentText);

            Debug.Log($"로드된 사운드 수 : {soundList.Count}");

            foreach (var sound in soundList)
            {
                Debug.Log($"사운드: {EncodeKorean(sound.id)}");
            }

        }
        else
        {
            Debug.LogError($"JSON파일을 찾을 수 없습니다. : {jsonFileName}");
        }
    }
}
