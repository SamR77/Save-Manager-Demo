using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsManager : MonoBehaviour
{
    [Range(0, 100)] public int masterVolume;
    [Range(0, 100)] public int musicVolume;
    [Range(0, 100)] public int sFXVolume;
    [Range(0, 100)] public int voiceoverVolume;



    #region Save and Load

    public void Save(ref SettingsData data)
    { 
        data.MasterVolume = masterVolume;
        data.MusicVolume = musicVolume;
        data.SFXVolume = sFXVolume;
        data.VoiceoverVolume = voiceoverVolume;
    }

    public void Load(SettingsData data)
    {
        masterVolume = data.MasterVolume;
        musicVolume = data.MusicVolume;
        sFXVolume = data.SFXVolume;
        voiceoverVolume = data.VoiceoverVolume;

    }
    #endregion


    private void Update()
    {
        
            if (Keyboard.current.numpad7Key.wasPressedThisFrame)
            {
                GameManager.Instance.SaveManager.SaveSettingsData();
            }

            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                GameManager.Instance.SaveManager.LoadSettingsData();        
            }
      

    }




}

