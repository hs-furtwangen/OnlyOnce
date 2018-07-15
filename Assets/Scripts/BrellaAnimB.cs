using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrellaAnimB : MonoBehaviour {

    private void HidePlayer()
    {
        PlayerPlatformerController.Instance.SpriteHide();
    }

    private void TeleportPlayer()
    {
        PlayerPlatformerController.Instance.Teleport();
    }

}
