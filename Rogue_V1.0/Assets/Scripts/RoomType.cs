using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    public static RoomType instance;
    public int type;
    public enum Type { leftRightOpen,noTop,noBottom,allDirections}
    public Type roomType;

    private void Awake()
    {
        instance = this;
    }
    public void DestroyRoom()
    {
        Destroy(gameObject);
    }
}
