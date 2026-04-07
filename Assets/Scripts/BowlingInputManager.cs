using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BowlingInputManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider meterSlider; 
    public TextMeshProUGUI instructionText;

    [Header("Meter Settings")]
    public float pingPongSpeed = 1.5f; 

    [Header("Current State (Read-Only)")]
    public bool isSwing = true;     
    public int sideMultiplier = 1;  
    public float effectStrength = 0f;
    public bool hasBowled = false;

    private float pingPongTime = 0f;

    void Update()
    {
        UpdateUI();

        if (hasBowled) return;

        HandleInput();
        AnimateMeter();
    }

    private void HandleInput()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1)) isSwing = true;
        if (Input.GetKeyDown(KeyCode.Alpha2)) isSwing = false;

        if (Input.GetKeyDown(KeyCode.RightArrow)) sideMultiplier = 1;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) sideMultiplier = -1;
    
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasBowled = true;
            CalculateStrength();
            Debug.Log($"BOWLED! Mode: {(isSwing ? "Swing" : "Spin")} | Side: {sideMultiplier} | Strength: {effectStrength:F1}%");
        }
    }

    private void AnimateMeter()
    {
      
        pingPongTime += Time.deltaTime * pingPongSpeed;
        meterSlider.value = Mathf.PingPong(pingPongTime, 1f);
    }

    private void CalculateStrength()
    {
        
        float distanceFromCenter = Mathf.Abs(meterSlider.value - 0.5f);
        float maxDistance = 0.5f;

        float percentage = 1f - (distanceFromCenter / maxDistance);

        effectStrength = Mathf.Clamp(percentage * 100f, 0f, 100f);
    }

    private void UpdateUI()
    {
        if (instructionText != null && !hasBowled)
        {
            string mode = isSwing ? "SWING" : "SPIN";
            string side = sideMultiplier == 1 ? "Right" : "Left";
            instructionText.text = $"Mode: {mode}\nSide: {side}\n\nPress Space to Bowl";
        }
    }
}