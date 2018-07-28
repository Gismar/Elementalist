using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBehaviour : MonoBehaviour {
    private bool _IsIdle = false;
    protected bool _IsAttacking = false;
    protected bool _BeganAim = false;
    private bool _CanUseSecondary = true;
    private float _IdleTimer;
    private float _IdleTimerLerp;
    private float _SecondaryAttackTimer;

    protected IOrb _Orb;
    protected Vector2 _Offset;
    protected Transform _Player;
	
	void Update () {
        if (_IsIdle)
        {
            transform.position = new Vector2(_Player.position.x + Mathf.Cos(Time.time + _Offset.x) * transform.localScale.x,
                                            _Player.position.y + Mathf.Sin(Time.time + _Offset.y) * transform.localScale.y);
        }
        else
        {
            if (_IdleTimer < Time.time)
            {
                transform.position = Vector3.Lerp(transform.position, _Player.position, _IdleTimerLerp);
                _IdleTimerLerp += Time.deltaTime / Vector3.Distance(transform.position, _Player.position);
            }
            if (_IdleTimerLerp >= 1)
            {
                _IsIdle = true;
            }
        }

        if (!_CanUseSecondary)
        {
            _CanUseSecondary = _SecondaryAttackTimer <= Time.time;
            transform.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(0.7f, 0.7f, 0.7f, 0.7f), (_SecondaryAttackTimer - Time.time) / _Orb.SecondaryAttackDelay);
        }

        if (_IsAttacking) return;
        if (Input.GetMouseButtonDown(0))
        {
            _Orb.ActivateAimLine();
            _IsIdle = false;
            SetUpIdleTimer();
        }

        if (Input.GetMouseButton(0))
        {
            transform.rotation = Quaternion.Euler(0, 0, Aim());
            _Orb.UpdateAimLine();
            SetUpIdleTimer();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SetUpIdleTimer();
            _IsAttacking = true;
            _BeganAim = false;
            _Orb.MainAttack();
        }

        if (Input.GetMouseButtonDown(1) && _CanUseSecondary)
        {
            SetUpIdleTimer();
            SetUpSecondaryAttackTimer();
            _IsAttacking = true;
            _Orb.SecondaryAttack();
        }
    }

    private void SetUpSecondaryAttackTimer()
    {
        _SecondaryAttackTimer = Time.time + _Orb.SecondaryAttackDelay;
        _CanUseSecondary = false;
    }

    private void SetUpIdleTimer()
    {
        _IdleTimer = Time.time + _Orb.IdleDelay;
        _IdleTimerLerp = 0;
    }

    private float Aim()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (mousePos - transform.position).normalized;
        var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        return rotation;
    }
}
