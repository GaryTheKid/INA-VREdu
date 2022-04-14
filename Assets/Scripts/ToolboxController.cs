using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolboxController : MonoBehaviour
{
    [SerializeField] private GameObject uiCanvas;
    private Animator animator;
    private enum ToolboxState
    {
        open,
        close
    }
    private ToolboxState state;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        state = ToolboxState.close;
    }

    public void Interact()
    {
        switch (state)
        {
            case ToolboxState.close: OpenToolbox(); break;
            case ToolboxState.open: CloseToolbox(); break;
        }
    }

    private void OpenToolbox()
    {
        animator.SetTrigger("isOpening");
        uiCanvas.SetActive(true);
        state = ToolboxState.open;
    }

    private void CloseToolbox()
    {
        animator.SetTrigger("isClosing");
        uiCanvas.SetActive(false);
        state = ToolboxState.close;
    }
}
