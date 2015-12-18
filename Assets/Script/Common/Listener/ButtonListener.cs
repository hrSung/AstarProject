using UnityEngine;
using System.Collections;

public class ButtonListener : MonoBehaviour 
{
    public int index;
    public string btnName;
    public delegate void PressListener(ButtonListener _btn, bool isPress);
    public PressListener OnPressed;

    public delegate void ClickListener(ButtonListener _btn);
    public ClickListener OnClicked;

    public delegate void DragListener(ButtonListener _btn, Vector2 delta);
    public DragListener OnDraged;

    public delegate void TriggerListener(GameObject _go);
    public TriggerListener OnTrigger;


    void OnPress(bool isPress)
    {
        if (null != OnPressed)
        {
            OnPressed(this, isPress);
        }
    }

    void OnClick()
    {
        if (null != OnTrigger)
        {
            OnTrigger(gameObject);
            OnTrigger = null;
        }

        if (null != OnClicked)
        {
            OnClicked(this);
        }
    }

    void OnDrag(Vector2 delta)
    {
        if (null != OnDraged)
        {
            OnDraged(this, delta);
        }
    }
}
