using System.Collections.Generic;
using UnityEngine;

public class ObjectsInventory : MonoBehaviour
{
    public static ObjectsInventory instance;

    public bool[] keyPieces;

    private void Awake()
    {
        instance = this;
    }

    public void AddKeyPiece(int pieceIndex)
    {
        keyPieces[pieceIndex] = true;
    }

    public bool CheckKeyPiece(int[] indexes)
    {
        foreach (int index in indexes)
        {
            if (!keyPieces[index])
            {
                return false;
            }
        }

        return true;
    }
}
