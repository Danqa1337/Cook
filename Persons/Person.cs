using System;
using UnityEngine;
using DG.Tweening;

public abstract class Person : MapObjectComponent
{
    public abstract PersonType PersonType { get; }

    public abstract string Name { get; }

    public virtual void Dismiss()
    {
        Destroy(gameObject);
    }

    public virtual void OnSpawned()
    {
        transform.DOLocalMoveX(10, 1).From();
    }

    protected virtual Tween Quit()
    {
        return transform.DOMoveX(transform.position.x - 10, 1).OnComplete(delegate { Destroy(); });
    }

    protected virtual void Destroy()
    {
        Destroy(gameObject);
    }

    public abstract void Interract();

    protected override void OnPlayerEnter()
    {
        Interract();
    }

    protected override void OnPlayerExit()
    {
    }

    protected override void OnPlayerStay()
    {
    }
}

public static class DialogManager
{
    public static event Action<Dialog> OnDialogStart;

    public static event Action OnDialogEnd;

    public static void StartDialog(Dialog dialog)
    {
        OnDialogStart?.Invoke(dialog);
    }

    public static void EndDialog()
    {
        OnDialogEnd?.Invoke();
    }
}