using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] bgm;

    [SerializeField] private bool playBgm;

    private int bgmIndex;

    private bool hasStopped = false;

    private void Update()
    {
        if (!playBgm)
        {
            if (BGMIsPlaying())
            {
                Debug.Log("1");
                StopAllBGM();
                hasStopped = true;
            }
        }
        else
        {
            if (!BGMIsPlaying() && hasStopped)
            {
                Debug.Log("2");
                PlayRandomBGM();
                hasStopped = false;
            }
        }
    }


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
}