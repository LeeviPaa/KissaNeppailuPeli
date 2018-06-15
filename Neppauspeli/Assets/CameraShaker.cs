using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraShaker : MonoBehaviour {

    public float _magnitude = 1.2f;
    public float _frequency = 25;
    public float _roughness = 25;
    public float _time = 0.5f;
    public float _timescale = 0.5f;
    public float _falloff = 3;


    private Camera thisCamera;

    private void Start()
    {
        thisCamera = Camera.main;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += LevelWasLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LevelWasLoaded;
    }
    //#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ShakeCamera(_time, 1, _timescale, _frequency);
        }
    }
    //#endif
    private void LevelWasLoaded(Scene s, LoadSceneMode lsm)
    {
        thisCamera = Camera.main;
    }

    public void ShakeCamera(float time, float magnitudeMultiplier, float timescale, float frequency)
    {
        StartCoroutine(ICameraShake(time, magnitudeMultiplier, timescale, frequency));
    }
    public void ShakeCamera(float time, float magnitudeMultiplier, float timescale)
    {
        StartCoroutine(ICameraShake(time, magnitudeMultiplier, timescale, _frequency));
    }

    IEnumerator ICameraShake(float time, float magnitudeMult, float timeScale, float frequen)
    {
        float elapsed = 0;
        float hz = 0;
        float frequency = 1/ frequen;
        Vector3 startPos = thisCamera.transform.localPosition;
        Quaternion startRot = thisCamera.transform.localRotation;

        float mag = _magnitude * magnitudeMult;

        Vector3 target = new Vector3(Random.Range(-mag, mag), Random.Range(-mag, mag), 0);
        Quaternion rotTarget = Quaternion.Euler(Random.Range(-mag, mag) * 10, Random.Range(-mag, mag) * 10, Random.Range(-mag, mag) * 10);
        Time.timeScale = timeScale;

        while(elapsed < time)
        {
            elapsed += Time.deltaTime;
            hz += Time.deltaTime;

            if (hz >= frequency)
            {
                mag = _magnitude / (1 + elapsed * _falloff);
                target = new Vector3(Random.Range(-mag, mag), Random.Range(-mag, mag), 0);
                rotTarget = Quaternion.Euler(Random.Range(-mag, mag) * 10, Random.Range(-mag, mag) * 10, Random.Range(-mag, mag) * 10);

                hz = 0;
            }

            thisCamera.transform.localRotation = Quaternion.Lerp(transform.localRotation, rotTarget, (_roughness * Time.deltaTime)/(1+elapsed*_falloff)) * startRot;
            thisCamera.transform.localPosition = Vector3.Lerp(thisCamera.transform.localPosition, startPos+target, (_roughness * Time.deltaTime) / (1 + elapsed*_falloff));
            yield return null;
        }

        thisCamera.transform.localPosition = startPos;
        thisCamera.transform.localRotation = startRot;
        Time.timeScale = 1;
    }
}
