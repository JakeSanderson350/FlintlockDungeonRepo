using System;
using System.Collections.Generic;
using UnityEngine;

public class Forces : MonoBehaviour
{
    public Vector3 Velocity => GetVelocity();

    [SerializeField] CharacterController characterController;

    List<Force> forces = new();
    Vector3 constForce = Vector3.zero;

    [Serializable] public class Force 
    {
        public Vector3 force = Vector3.zero; //distance to move per second
        public float drag = 0f; //force lost per second
        public float time = Mathf.Infinity; //time limit to apply force 

        public Force(Force force)
        {
            this.force = force.force;
            this.drag = force.drag;
            this.time = force.time;
        }

        public Force(Vector3 force, float drag, float time)
        {
            this.force = force;
            this.drag = drag;
            this.time = time;
        }

        /// <summary> DO NOT CALL THIS. OOP IS BEING STUPID. This is exactly why we have the "friend" flag in c++ </summary>
        public Vector3 GetForce(float deltaTime)
        {
            Vector3 output = force;

            force -= Vector3.ClampMagnitude(force * drag * deltaTime, force.magnitude);
            time -= deltaTime;

            return output * deltaTime;
        }
    }

    private void Update()
    {
        ApplyForces(Time.deltaTime);
    }

    public void AddForceConstant(Vector3 force)
    {
        constForce += force;
    }

    public void AddForce(Force force)
    {
        forces.Add(new Force(force));
    }

    public void AddForce(Vector3 force, float drag, float time = Mathf.Infinity)
    {
        forces.Add(new Force(force, drag, time));
    }
    
    public void RemoveAllForces()
    {
        forces.Clear();
        constForce = Vector3.zero;
    }

    public Vector3 GetVelocity()
    {
        Vector3 total = constForce;
        forces.ForEach(f => total += f.force);

        return total;
    }

    void ApplyForces(float deltaTime)
    {
        Vector3 total = Vector3.zero;

        total += constForce * deltaTime;
        forces.ForEach(f => total += f.GetForce(deltaTime));

        Vector3 expectedPos = transform.position + total;
        characterController.Move(total);
        DecayForces(expectedPos);
    }

    void DecayForces(Vector3 target)
    {
        Vector3 totalError =  target - transform.position; //the missile knows where it is

        //modify forces which are in error (this occours when colliding with objects) 
        foreach (Force force in forces)
        {
            Vector3 factor = force.force * Mathf.Clamp01(Vector3.Dot(force.force, totalError));
            force.force -= factor;
            Debug.DrawRay(transform.position, factor.normalized, Color.yellow);
        }

        //scrub out expired forces 
        forces.RemoveAll(forces => forces.force.magnitude <= 0.0001f || forces.time <= 0f); 
        constForce = Vector3.zero;

    }
}

