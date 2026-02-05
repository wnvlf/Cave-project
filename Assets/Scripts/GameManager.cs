using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.PlayBgm(AudioManager.Bgm.Battle, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySfxButton()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Click);
    }
}
