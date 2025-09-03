using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    private Camera cam;

    [Header("Presets")]
    [SerializeField] Rigidbody linkedObject;
    [SerializeField] float effectsMinSpeed = 30.0f;
    [SerializeField] float effectsMaxSpeed = 150.0f;

    [Tooltip("FOV constraints")]
    public float minFOV = 70.0f;
    public float maxFOV = 90.0f;

    [Tooltip("Shake constraints")]
    public float minShake = 0.0f;
    public float maxShake = 1.0f;

    [Header("Perlin System")]
    [SerializeField] float trauma;

    [Tooltip("the power of the shake")]
    [SerializeField] float traumaMult = 16; 

    [Tooltip("the range of movment")]
    [SerializeField] float traumaMag = 0.8f; 

    [Tooltip("the rotational power")]
    [SerializeField] float traumaRotMag = 17f; 

    [Tooltip("the depth multiplier")]
    [SerializeField] float traumaDepthMag = 0.6f; 

    [Tooltip("how quickly the shake falls off")]
    [SerializeField] float traumaDecay = 1.3f; 

    private float timeCounter = 0; //counter stored for smooth transition

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        FindMinTrauma();
        ZoomCam();
        ShakeCam();
    }
    
    private void FindMinTrauma()
    {
        float speedShake = Mathf.Lerp(minShake, maxShake, Mathf.InverseLerp(effectsMinSpeed, effectsMaxSpeed, linkedObject.velocity.magnitude));
        trauma = speedShake > trauma ? speedShake : trauma;
    }

    private void ZoomCam()
    {
        cam.fieldOfView = Mathf.Lerp(minFOV, maxFOV, Mathf.InverseLerp(effectsMinSpeed, effectsMaxSpeed, linkedObject.velocity.magnitude));
    }

    private void ShakeCam()
    {
        if (trauma > 0)
        {
            //increase the time counter (how fast the position changes) based off the traumaMult and some root of the Trauma
            timeCounter += Time.deltaTime * Mathf.Pow(trauma, 0.3f) * traumaMult;
            Vector3 newPos = GetVector3() * traumaMag * trauma; ;
            transform.localPosition = newPos;
            transform.localRotation = Quaternion.Euler(newPos * traumaRotMag);
            trauma -= Time.deltaTime * traumaDecay * (trauma + 0.3f);
        }
    }

    float GetFloat(float seed) => (Mathf.PerlinNoise(seed, timeCounter) - 0.5f) * 2f;
    Vector3 GetVector3() =>  new Vector3(GetFloat(1), GetFloat(10), GetFloat(100) * traumaDepthMag);
}
