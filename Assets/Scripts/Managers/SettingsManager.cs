using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SettingsManager : MonoBehaviour
{
    [Range(0, 100)] public int masterVolume;
    [Range(0, 100)] public int musicVolume;
    [Range(0, 100)] public int sFXVolume;
    [Range(0, 100)] public int voiceoverVolume;

    // References to Settings UI
    public Slider masterVolume_slider;
    public Slider musicVolume_slider;
    public Slider sfxVolume_slider;
    public Slider voiceoverVolume_slider;
        
    public TMP_Text masterVolumeAmount;
    public TMP_Text musicVolumeAmount;
    public TMP_Text sfxVolumeAmount;
    public TMP_Text voiceoverVolumeAmount;

<<<<<<< Updated upstream

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

=======
    private void OnEnable()
    {
        #region Slider Value change events
        masterVolume_slider.onValueChanged.AddListener(value =>
        {
            masterVolume = Mathf.RoundToInt(value);
            masterVolumeAmount.text = masterVolume.ToString();
        });

        musicVolume_slider.onValueChanged.AddListener(value =>
        {
            musicVolume = Mathf.RoundToInt(value);
            musicVolumeAmount.text = musicVolume.ToString();
        });

        sfxVolume_slider.onValueChanged.AddListener(value =>
        {
            sfxVolume = Mathf.RoundToInt(value);
            sfxVolumeAmount.text = sfxVolume.ToString();
        });

        voiceoverVolume_slider.onValueChanged.AddListener(value =>
        {
            voiceoverVolume = Mathf.RoundToInt(value);
            voiceoverVolumeAmount.text = voiceoverVolume.ToString();
        });

        #endregion;

    }


    private void OnDestroy()
    {
        
    }
    private void Start()
    {
        RefreshUI();
    }




    public void RefreshUI()
    {
        masterVolume_slider.value = masterVolume;
        musicVolume_slider.value = musicVolume;
        sfxVolume_slider.value = sfxVolume;
        voiceoverVolume_slider.value = voiceoverVolume;
    }
 

}
>>>>>>> Stashed changes
