using UnityEngine;

public class Stamina : MonoBehaviour
{
    [Header("Stamina Stats")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaConsuptionPerSecond;
    [SerializeField] private float staminaRegebPerSecond;
    [SerializeField] [Range(0, 1)] private float minimumStaminaToRunAgain;

    private float currentStamina;

    private bool noStamina = false;
    private bool isRunning = false;

    private void Start()
    {
        currentStamina = maxStamina;
    }

    private void Update()
    {
        DetectedSprintInput();
        ManageStamina();
        Debug.Log(currentStamina);
    }

    private void ManageStamina()
    {
        if (!isRunning) currentStamina += staminaRegebPerSecond * Time.deltaTime;
        else currentStamina -= staminaConsuptionPerSecond * Time.deltaTime;
        if (currentStamina < 0f)
        {
            currentStamina = 0f;
            noStamina = true;
        }
        if (currentStamina >= maxStamina * minimumStaminaToRunAgain)
        {
            noStamina = false;
        }
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }

    private void DetectedSprintInput()
    {
        if (Input.GetButton("Sprint") && !noStamina)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    public bool GetIsRunning()
    {
        return isRunning;
    }
}
