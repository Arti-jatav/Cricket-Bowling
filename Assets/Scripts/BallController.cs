using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("References")]
    public Transform bounceMarker;
    public BowlingInputManager inputManager;
    private TrailRenderer trailRenderer;

    [Header("Settings")]
    public float deliverySpeed = 20f;
    public float maxTravelDistance = 35f;

    [Header("Swing & Spin Settings")]
    public float maxSwingForce = 2.5f;   
    public float maxSpinAngle = 15f;

    private bool isDelivered = false;
    private bool hasBounced = false;
    private Vector3 releasePosition;
    private Vector3 lockedBouncePosition;

    private float flightDuration;
    private float timeElapsed;
    private Vector3 perpendicularDir;
    private float currentSwingStrength;

    private Vector3 postBounceVelocity;

    void Start()
    {
        releasePosition = transform.position;
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        if (!isDelivered && inputManager != null && inputManager.hasBowled)
        {
            StartDelivery();
        }

        if (isDelivered)
        {
            MoveBall();

            if (Vector3.Distance(releasePosition, transform.position) > maxTravelDistance)
            {
                ResetDelivery();
            }
        }
    }

    private void StartDelivery()
    {
        isDelivered = true;
        lockedBouncePosition = bounceMarker.position;

        float distanceToBounce = Vector3.Distance(releasePosition, lockedBouncePosition);
        flightDuration = distanceToBounce / deliverySpeed;
        timeElapsed = 0f;


        Vector3 directionToBounce = (lockedBouncePosition - releasePosition).normalized;
        perpendicularDir = Vector3.Cross(Vector3.up, directionToBounce).normalized;

        if (inputManager.isSwing)
        {
            float strengthMultiplier = inputManager.effectStrength / 100f;
            currentSwingStrength = maxSwingForce * strengthMultiplier * inputManager.sideMultiplier;
        }
        else
        {
            currentSwingStrength = 0f; 
        }
    }

    private void MoveBall()
    {
        if (!hasBounced)
        {
            
            timeElapsed += Time.deltaTime;

            
            float t = Mathf.Clamp01(timeElapsed / flightDuration);

            
            Vector3 linearPosition = Vector3.Lerp(releasePosition, lockedBouncePosition, t);

            
            Vector3 curveOffset = perpendicularDir * (currentSwingStrength * Mathf.Sin(t * Mathf.PI));

           
            transform.position = linearPosition + curveOffset;

           
            if (t >= 1.0f)
            {
                TriggerBounce();
            }
        }
        else
        {
            
            transform.position += postBounceVelocity * Time.deltaTime;
        }
    }

    private void TriggerBounce()
    {
        hasBounced = true;

        
        transform.position = lockedBouncePosition;

       
        Vector3 linearVel = (lockedBouncePosition - releasePosition) / flightDuration;

        
        Vector3 swingVel = perpendicularDir * (currentSwingStrength * Mathf.PI * -1f) / flightDuration;

        postBounceVelocity = linearVel + swingVel;

       
        postBounceVelocity.y = Mathf.Abs(linearVel.y) * 0.35f;

       
        if (!inputManager.isSwing)
        {
            float strengthMultiplier = inputManager.effectStrength / 100f;
            float spinAngle = maxSpinAngle * strengthMultiplier * inputManager.sideMultiplier;

           
            postBounceVelocity = Quaternion.Euler(0, spinAngle, 0) * postBounceVelocity;
           
        }
        else
        {
            Debug.Log("Bounce! Swing stops instantly, ball continues straight along tangent.");
        }
    }

    private void ResetDelivery()
    {
        isDelivered = false;
        hasBounced = false;
        transform.position = releasePosition;

        if (trailRenderer != null) trailRenderer.Clear();
        if (inputManager != null) inputManager.hasBowled = false;
    }
}