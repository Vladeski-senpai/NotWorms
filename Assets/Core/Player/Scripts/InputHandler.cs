using UnityEngine;

public class InputHandler : MonoBehaviour
{
    Camera cam;
    RaycastHit2D[] hitsInfo;
    Vector3 touchWorldPos;

    void Start()
    {
        cam = Camera.main;
    }

    // ѕровер€ем есть ли игрок в области клика
    public bool CheckForPlayer(Vector3 touchPos)
    {
        bool wasFound = false;

        touchWorldPos = cam.ScreenToWorldPoint(touchPos);
        touchWorldPos.z = 0;
        hitsInfo = Physics2D.RaycastAll(touchWorldPos, Vector2.zero);

        if (hitsInfo != null)
            foreach (RaycastHit2D hit in hitsInfo)
                if (hit.transform == transform)
                {
                    wasFound = true;
                    break;
                }

        return wasFound;
    }
}
