using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float torqueAmount = 1f;
    [SerializeField] float baseSpeed = 15f;
    [SerializeField] float boostSpeed = 35f;
    [SerializeField] ParticleSystem powerupParticles;
    [SerializeField] ScoreManager scoreManager;

    InputAction moveAction;
    Rigidbody2D rb;
    SurfaceEffector2D surfaceEffector2D;

    Vector2 moveVector;
    public bool canControlPlayer = true;
    float previousRotation;
    float totalRotation;
    int activePowerupCount;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        surfaceEffector2D = FindFirstObjectByType<SurfaceEffector2D>();
        //scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    private void Update()
    {
        if (canControlPlayer)
        {
            RotatePlayer();
            BoostPlayer();
            CalculateFlip();
        }
    }

    private void RotatePlayer()
    {
        moveVector = moveAction.ReadValue<Vector2>();

        if (moveVector.x < 0)
        {
            rb.AddTorque(torqueAmount);
        }
        else if (moveVector.x > 0)
        {
            rb.AddTorque(-torqueAmount);
        }
    }

    private void BoostPlayer()
    {
        if (moveVector.y > 0)
        {
            surfaceEffector2D.speed = boostSpeed;
        }
        else if (moveVector.y == 0)
        {
            surfaceEffector2D.speed = baseSpeed;
        }
    }

    private void CalculateFlip()
    {
        float currentRotation = transform.rotation.eulerAngles.z;

        totalRotation += Mathf.DeltaAngle(previousRotation, currentRotation);

        if (totalRotation > 340 || totalRotation < -340)
        {
            totalRotation = 0;
            scoreManager.AddScore(100);
        }

        previousRotation = currentRotation;
    }

    public void ActivatePowerup(PowerupSO powerup)
    {
        powerupParticles.Play();
        activePowerupCount += 1;
        print(activePowerupCount);

        if (powerup.GetPowerupType() == "speed")
        {
            baseSpeed += powerup.GetValueChange();
            boostSpeed += powerup.GetValueChange();
        }

        if (powerup.GetPowerupType() == "torque")
        {
            torqueAmount += powerup.GetValueChange();
        }
    }

    public void DeactivatePowerup(PowerupSO powerup)
    {
        activePowerupCount -= 1;
        print("Deactivate " + activePowerupCount);
        if (activePowerupCount == 0)
        {
            powerupParticles.Stop();
        }

        if (powerup.GetPowerupType() == "speed")
        {
            baseSpeed -= powerup.GetValueChange();
            boostSpeed -= powerup.GetValueChange();
        }

        if (powerup.GetPowerupType() == "torque")
        {
            torqueAmount -= powerup.GetValueChange();
        }
    }
}
