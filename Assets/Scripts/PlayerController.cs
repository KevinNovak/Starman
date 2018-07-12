using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] float xSpeed = 40f;
    [SerializeField] float ySpeed = 40f;

    [Header("Position")]
    [SerializeField] float xRange = 20f;
    [SerializeField] float yMin = -10f;
    [SerializeField] float yMax = 10f;

    [Header("Rotation")]
    [SerializeField] float posPitchFactor = -2f;
    [SerializeField] float throwPitchFactor = -20f;
    [SerializeField] float posYawFactor = 2f;
    [SerializeField] float throwRollFactor = -20f;

    [Header("Objects")]
    [SerializeField] GameObject circuter;
    [SerializeField] GameObject[] bullets;

    private float xCenter, yCenter;
    private float xThrow, yThrow;
    private bool controlEnabled = true;
    private ScoreBoard scoreBoard;

    // Use this for initialization
    void Start()
    {
        xCenter = transform.localPosition.x;
        yCenter = (yMax + yMin) / 2;
        scoreBoard = FindObjectOfType<ScoreBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controlEnabled && !PauseMenu.paused)
        {
            HandleTranslation();
            HandleRotation();
            HandleFiring();
        }
    }

    private void HandleTranslation()
    {
        // X Position
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");

        var xOffset = xThrow * xSpeed * Time.deltaTime;

        var xRawPos = transform.localPosition.x + xOffset;
        var xPos = Mathf.Clamp(xRawPos, -xRange + xCenter, xRange + xCenter);

        // Y Position
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        var yOffset = yThrow * ySpeed * Time.deltaTime;

        var yRawPos = transform.localPosition.y + yOffset;
        var yPos = Mathf.Clamp(yRawPos, yMin, yMax);

        // Translate
        transform.localPosition = new Vector3(xPos, yPos, transform.localPosition.z);
    }

    private void HandleRotation()
    {
        // Pitch
        var pitchFromPos = (transform.localPosition.y - yCenter) * posPitchFactor;
        var pitchFromThrow = yThrow * throwPitchFactor;
        var pitch = pitchFromPos + pitchFromThrow;

        var yaw = (transform.localPosition.x - xCenter) * posYawFactor;

        var roll = xThrow * throwRollFactor;

        // Rotate
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }
    private void HandleFiring()
    {
        var firing = CrossPlatformInputManager.GetButton("Fire");
        ActivateBullets(firing);
    }

    private void ActivateBullets(bool active)
    {
        foreach (var bullet in bullets)
        {
            var emmisionModule = bullet.GetComponent<ParticleSystem>().emission;
            emmisionModule.enabled = active;
        }
    }

    // Called by string reference
    void OnDeath()
    {
        controlEnabled = false;
        circuter.GetComponent<BetterWaypointFollower>().enabled = false;
        scoreBoard.EndGame();
    }
}
