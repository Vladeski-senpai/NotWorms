using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAim : MonoBehaviour
{
    [SerializeField] float velocity;
    [SerializeField] Transform target;
    [SerializeField] Transform body;

    void Start()
    {

    }

    void Update()
    {
        // Shooting
        if (Input.GetKeyDown(KeyCode.G)) Shoot();
    }

    void Shoot()
    {
        float angle;
        CalculateTrajectory(transform.position, target.position, velocity, out angle);

        Debug.Log("Angle " + angle);

        body.rotation = AimRotation(transform.position, target.position, velocity);
    }

    bool CalculateTrajectory(Vector3 start, Vector3 end, float muzzleVelocity, out float angle)
    {
        //, out float highAngle){

        Vector3 dir = end - start;
        float vSqr = muzzleVelocity * muzzleVelocity;
        float y = dir.y;
        dir.y = 0.0f;
        float x = dir.sqrMagnitude;
        float g = -Physics.gravity.y;

        float uRoot = vSqr * vSqr - g * (g * (x) + (2.0f * y * vSqr));


        if (uRoot < 0.0f)
        {

            //target out of range.
            angle = -45.0f;
            //highAngle = -45.0f;
            return false;
        }

        //        float r = Mathf.Sqrt (uRoot);
        //        float bottom = g * Mathf.Sqrt (x);

        angle = -Mathf.Atan2(g * Mathf.Sqrt(x), vSqr + Mathf.Sqrt(uRoot)) * Mathf.Rad2Deg;
        //highAngle = -Mathf.Atan2 (bottom, vSqr - r) * Mathf.Rad2Deg;
        return true;

    }

    Quaternion AimRotation(Vector3 start, Vector3 end, float velocity)
    {

        float low;
        //float high;
        CalculateTrajectory(start, end, velocity, out low);//, out high); //get the angle


        Vector3 wantedRotationVector = Quaternion.LookRotation(end - start).eulerAngles; //get the direction
        wantedRotationVector.x = low; //combine the two
        return Quaternion.Euler(wantedRotationVector); //into a quaternion
    }
}
