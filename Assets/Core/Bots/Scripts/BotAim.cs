using UnityEngine;

public class BotAim
{
    // Solve the firing arc with a fixed lateral speed. Vertical speed and gravity varies. 
    // This enables a visually pleasing arc.
    //
    // proj_pos (Vector3): point projectile will fire from
    // lateral_speed (float): scalar speed of projectile along XZ plane
    // target_pos (Vector3): point projectile is trying to hit
    // max_height (float): height above Max(proj_pos, impact_pos) for projectile to peak at
    //
    // fire_velocity (out Vector3): firing velocity
    // gravity (out float): gravity necessary to projectile to hit precisely max_height
    //
    // return (bool): true if a valid solution was found
    public static bool solve_ballistic_arc_lateral(Vector3 proj_pos, Vector3 target_pos, float lateral_speed,
        float max_height, out Vector3 fire_velocity, out float gravity)
    {
        // Handling these cases is up to your project's coding standards
        Debug.Assert(proj_pos != target_pos && lateral_speed > 0 && max_height > proj_pos.y, "fts.solve_ballistic_arc called with invalid data");

        fire_velocity = Vector3.zero;
        gravity = float.NaN;

        Vector3 diff = target_pos - proj_pos;
        Vector3 diffXZ = new Vector3(diff.x, 0f, diff.z);
        float lateralDist = diffXZ.magnitude;

        if (lateralDist == 0)
            return false;

        float time = lateralDist / lateral_speed;

        fire_velocity = diffXZ.normalized * lateral_speed;

        // System of equations. Hit max_height at t=.5*time. Hit target at t=time.
        //
        // peak = y0 + vertical_speed*halfTime + .5*gravity*halfTime^2
        // end = y0 + vertical_speed*time + .5*gravity*time^s
        // Wolfram Alpha: solve b = a + .5*v*t + .5*g*(.5*t)^2, c = a + vt + .5*g*t^2 for g, v
        float a = proj_pos.y;       // initial
        float b = max_height;       // peak
        float c = target_pos.y;     // final

        gravity = -4 * (a - 2 * b + c) / (time * time);
        fire_velocity.y = -(3 * a - 4 * b + c) / time;

        return true;
    }
}