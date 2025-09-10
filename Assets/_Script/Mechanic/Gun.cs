using ImprovedTimers;
using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //TEMP CLASS

    [SerializeField] LineRenderer line;
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
        reloadTimer?.Dispose();
    }

    void Shoot()
    {
        if (isReloading)
            return;


        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hitInfo))
        {
            Debug.DrawLine(Camera.main.transform.position, hitInfo.point, Color.red, 3f);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, hitInfo.point);

            foreach(var hit in Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward).ToList())
                if(hit.transform.TryGetComponent(out Health health)) 
                    health.Value -= damage;
        }

        line.enabled = true;
        reloadTimer = new(reload);
        reloadTimer.OnTimerStart += StartReload;
        reloadTimer.OnTimerStop += FinishReload;
        reloadTimer.Start();
    }

    void StartReload()
    {
        EventManager.SetCameraTrauma(3f);
        Tween.ShakeLocalPosition(transform, Vector3.one * .1f, .2f);
        Tween.LocalRotation(transform, new Vector3(-30, 0, 0), .2f, Ease.OutSine);
        isReloading = true;
    }

    void FinishReload()
    {
        Tween.LocalRotation(transform, Vector3.zero, .2f, Ease.OutSine);
        isReloading = false;
        line.enabled = false;

        reloadTimer.Dispose();
    }
}
