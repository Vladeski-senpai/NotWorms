using UnityEngine;

public class InputHandler : MonoBehaviour
{
    Aim aim;
    RaycastHit2D[] hitsInfo;

    void Start()
    {
        aim = GetComponent<Aim>();
    }

    // ѕровер€ем есть ли игрок в области клика
    public bool CheckForPlayer()
    {
        bool wasFound = false;
        hitsInfo = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(aim.CursorPosition), Vector2.zero);

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
