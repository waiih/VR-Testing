using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public Transform[] boardTransforms;
    public Board[] attachedBoards;
    public bool isFullyBoarded => attachedBoards.Length == boardTransforms.Length;
    private int transformIndex = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Board" || isFullyBoarded) return;

        Board board = other.GetComponent<Board>();
        if (board != null)
        {
            board.Construct(boardTransforms[transformIndex]);
            attachedBoards[transformIndex] = board;
            transformIndex++;
        } else {
            Debug.Log("Object tagged as Board does not contain a board script.");
        }
    }
}
