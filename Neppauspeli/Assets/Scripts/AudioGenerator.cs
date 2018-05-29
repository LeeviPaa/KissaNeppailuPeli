using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGenerator : MonoBehaviour {

    public AudioSource thisAudioSource;
    Animator thisAnim;
    private float[] samples = new float[64];
    public List<Transform> audioBlocks = new List<Transform>();
    public float lerpSpeed = 1;
    public float scaleMultiplier = 10;
    public float maxHeight = 100;
    
    void Update ()
    {
        GetSpectrumData();
        BlockAudioSync();
	}

    private void GetSpectrumData()
    {
        thisAudioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    private void BlockAudioSync()
    {
        int i = 0;
        foreach(Transform t in audioBlocks)
        {
            int a = (64 / audioBlocks.Count) - 1;
            float scaleMult = samples[i * a] * scaleMultiplier;
            scaleMult = Mathf.Clamp(scaleMult, 5, 5 + maxHeight);
            t.localScale = Vector3.Lerp(t.localScale, new Vector3(t.localScale.x, scaleMult, t.localScale.z), lerpSpeed * Time.deltaTime);
            i++;
        }
    }
}
