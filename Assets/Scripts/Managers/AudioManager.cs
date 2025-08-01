using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] bgm;

    [SerializeField] private bool playBgm;

    private int bgmIndex;

    private void Update()
    {
        if (playBgm == false && BGMIsPlaying())
            StopAllBGM();


        if (playBgm && bgm[bgmIndex].isPlaying == false)
            PlayRandomBGM();
    }

    #region BGM

    public void PlayBGM(int index)
    {
        StopAllBGM();

        bgmIndex = index;
        bgm[index].Play();
    }

    public void StopAllBGM()
    {
        for(int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    [ContextMenu("Play Random Music")]
    public void PlayRandomBGM()
    {
        StopAllBGM();
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    private bool BGMIsPlaying()
    {
        for(int i = 0; i < bgm.Length;i++)
        {
            if (bgm[i].isPlaying) return true;
        }

        return false;
    }

    #endregion
}