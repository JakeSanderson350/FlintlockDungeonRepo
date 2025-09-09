using ImprovedTimers;
using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //TEMP CLASS

    [SerializeField] int damage = 999;
    [SerializeField] float reload = 7;

    bool isReloading;

    CountdownTimer reloadTimer;

    private void OnEnable()
    {
        InputManager.inputAttack += Shoot;
    }

    private void OnDisable()
    {
        InputManager.inputAttack -= Shoot;
    }

    void Shoot()
    {
        if (isReloading)
            return;

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hitInfo) && hitInfo.transform.TryGetComponent(out Health health))
        {
            Debug.DrawLine(Camera.main.transform.position, hitInfo.transform.position, Color.red, 3f);
            health.Value -= damage;
        }

        reloadTimer = new(reload);
        reloadTimer.OnTimerStart += StartReload;
        reloadTimer.OnTimerStop += FinishReload;
        reloadTimer.Start();
    }

    void StartReload()
    {
        Tween.LocalRotation(transform, new Vector3(-30, 0, 0), .5f);
        isReloading = true;
    }

    void FinishReload()
    {
        Tween.LocalRotation(transform, Vector3.zero, .5f);
        isReloading = false;

        reloadTimer.Dispose();
    }
}
