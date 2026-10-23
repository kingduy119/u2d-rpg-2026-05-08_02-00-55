using VContainer;
using VContainer.Unity;
using UnityEngine;
using Characters;
using System;
using Weapons;
using UnityEngine.EventSystems;


public class GamePlayLS : LifetimeScope
{
    [SerializeField] ControlsSettings _controlsSettings;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GamePlay>(Lifetime.Singleton);
        builder.RegisterEntryPoint<TestMouseEvent>(Lifetime.Singleton).AsSelf();


        builder.RegisterInstance<IControlsSettings>(_controlsSettings);

        builder.Register<IKeyBoardHandler, KeyBoardHandler>(Lifetime.Singleton);

        builder.RegisterComponentInHierarchy<PlayerSelectTilemapView>();
    }
}

public class GamePlay : IStartable, IDisposable
{
    [Inject] readonly FactoryService _factory;

    public void Start()
    {
        CterEvent.Shoot += Character_Shoot;
    }

    public void Dispose()
    {
        CterEvent.Shoot -= Character_Shoot;
    }

    private void Character_Shoot(Character cter, ProjectSO projectSO)
    {
        Vector2 direction = (cter.Target.position - cter.transform.position).normalized;
        var projectile = _factory.GetPrefab(projectSO);

        if (projectile != null)
        {
            projectile.Target = cter.Target.gameObject;
            projectile.Launch(cter.AttackPoint.transform, direction);
        }
    }


}


public class TestMouseEvent : ITickable
{
    public Vector3 MouseWorldPosition => GetMousePosition();
    public bool PointerDown = false;

    ITargetController _target;

    public void Tick()
    {
        if (EventSystem.current.IsPointerOverGameObject() || !Camera.main)
            return;

        // MouseWorldPosition = GetMousePosition();
        if (Input.GetMouseButtonDown(0))
        {
            HandleMoveCharacter();
            PointerDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            PointerDown = false;
            // _target?.MoveByPointer(MouseWorldPosition);
            if (_target != null)
            {
                GameDefenseEvents.ChangeTilemapPosition(_target);
                _target = null;
            }
        }

        _target?.MoveByPointer(MouseWorldPosition);

    }

    private void HandleMoveCharacter()
    {
        if (_target == null)
        {
            Collider2D collider = PointerCollider2D("Player");
            if (collider != null)
            {
                var targets = collider.GetComponents<ITargetController>();
                if (targets.Length > 0) _target = targets[0];
            }
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        return mousePosition;
    }

    public Collider2D PointerCollider2D(string layer) => PointerCollider2D(LayerMask.GetMask(layer));
    public Collider2D PointerCollider2D(LayerMask layer)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            MouseWorldPosition,
            Vector2.zero, Mathf.Infinity, layer);
        return hit.collider;
    }
}