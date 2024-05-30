using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerControl : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;
    private DeerState _state = DeerState.Laying;
    private Vector2[] _wanderingMap;

    private int _currentPositionIndex = 0;
    private int _targetPositionIndex;
    private Vector2 _targetPositionExcludingHeight;
    private float _runSpeed = 3.0f;

    private enum DeerState
    {
        Laying = 1,
        Grazing,
        PoisedToRun,
        Running
    }

    public GameObject Parent;
    public GameObject Player;
    public string RandomPoint1 = string.Empty;
    public string RandomPoint2 = string.Empty;
    public string RandomPoint3 = string.Empty;
    public string RandomPoint4 = string.Empty;
    public string RandomPoint5 = string.Empty;
    public string RandomPoint6 = string.Empty;
    public string RandomPoint7 = string.Empty;
    public string RandomPoint8 = string.Empty;
    public string RandomPoint9 = string.Empty;

    public DeerControl()
    {
    }

    public void Start()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
            _animator.speed = 0.75f;
        }

        _wanderingMap = new Vector2[9];
        string[] splitComponents;

        splitComponents = RandomPoint1.Split(',');
        _wanderingMap[0] = new Vector2(Convert.ToSingle(splitComponents[0]), Convert.ToSingle(splitComponents[1]));

        splitComponents = RandomPoint2.Split(',');
        _wanderingMap[1] = new Vector2(Convert.ToSingle(splitComponents[0]), Convert.ToSingle(splitComponents[1]));

        splitComponents = RandomPoint3.Split(',');
        _wanderingMap[2] = new Vector2(Convert.ToSingle(splitComponents[0]), Convert.ToSingle(splitComponents[1]));

        splitComponents = RandomPoint4.Split(',');
        _wanderingMap[3] = new Vector2(Convert.ToSingle(splitComponents[0]), Convert.ToSingle(splitComponents[1]));

        splitComponents = RandomPoint5.Split(',');
        _wanderingMap[4] = new Vector2(Convert.ToSingle(splitComponents[0]), Convert.ToSingle(splitComponents[1]));

        splitComponents = RandomPoint6.Split(',');
        _wanderingMap[5] = new Vector2(Convert.ToSingle(splitComponents[0]), Convert.ToSingle(splitComponents[1]));

        splitComponents = RandomPoint7.Split(',');
        _wanderingMap[6] = new Vector2(Convert.ToSingle(splitComponents[0]), Convert.ToSingle(splitComponents[1]));

        splitComponents = RandomPoint8.Split(',');
        _wanderingMap[7] = new Vector2(Convert.ToSingle(splitComponents[0]), Convert.ToSingle(splitComponents[1]));

        splitComponents = RandomPoint9.Split(',');
        _wanderingMap[8] = new Vector2(Convert.ToSingle(splitComponents[0]), Convert.ToSingle(splitComponents[1]));

        _currentPositionIndex = 0;
    }

    void OnDisable()
    {
        _state = DeerState.Laying;
    }

    public void Update()
    {
        if (Player != null)
        {
            if (_state == DeerState.Laying && Vector3.Distance(transform.position, Player.transform.position) < 7.5f)
            {
                _state = DeerState.Grazing;
                _animator.SetInteger("AnimationPhase", 2);
            }
            else if (_state == DeerState.Grazing && Vector3.Distance(transform.position, Player.transform.position) < 5.5f)
            {
                _state = DeerState.PoisedToRun;
            }
            else if (_state == DeerState.Running && Parent != null)
            {
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
                {
                    Vector3 targetPositionIncludingHeight = new Vector3(_targetPositionExcludingHeight.x, Parent.transform.position.y, _targetPositionExcludingHeight.y);

                    Parent.transform.LookAt(targetPositionIncludingHeight);
                    Parent.transform.Translate(_runSpeed * Vector3.forward * Time.deltaTime);

                    if (Vector3.Distance(Parent.transform.position, targetPositionIncludingHeight) <= 0.2f)
                    {
                        _currentPositionIndex = _targetPositionIndex;

                        _state = DeerState.Grazing;
                        _animator.SetBool("Running", false);
                        _audioSource.enabled = false;
                    }
                }
                else
                {
                    if (!_animator.GetBool("Running"))
                    {
                        _animator.SetBool("Running", true);

                        _audioSource.enabled = true;
                        _audioSource.Play();
                    }
                }
            }
        }
    }

    private Vector2 NewRandomTargetPosition(ref int newTargetPositionIndex)
    {
        int selectedPositionIndex = _currentPositionIndex;

        while (selectedPositionIndex == _currentPositionIndex)
        {
            float v = UnityEngine.Random.value;

            if (v >= 0.0f && v <= 0.111f)
            {
                selectedPositionIndex = 0;
            }
            else if (v > 0.111f && v <= 0.222f)
            {
                selectedPositionIndex = 1;
            }
            else if (v > 0.222f && v <= 0.333f)
            {
                selectedPositionIndex = 2;
            }
            else if (v > 0.333f && v <= 0.444f)
            {
                selectedPositionIndex = 3;
            }
            else if (v > 0.444f && v <= 0.555f)
            {
                selectedPositionIndex = 4;
            }
            else if (v > 0.555f && v <= 0.666f)
            {
                selectedPositionIndex = 5;
            }
            else if (v > 0.666f && v <= 0.777f)
            {
                selectedPositionIndex = 6;
            }
            else if (v > 0.777f && v <= 0.888f)
            {
                selectedPositionIndex = 7;
            }
            else if (v > 0.888f && v <= 1.0f)
            {
                selectedPositionIndex = 8;
            }
        }

        newTargetPositionIndex = selectedPositionIndex;

        return _wanderingMap[selectedPositionIndex];
    }

    public void TriggerFeedingOrRunning()
    {
        if (_state != DeerState.PoisedToRun)
        {
            float v = UnityEngine.Random.value;

            if (v > 0.0f && v < 0.2f)
            {
                _animator.SetBool("Feeding", true);
            }
        }
        else if (!_animator.GetBool("Feeding"))
        {
            StartRunning();
        }
    }

    public void EndFeedingOrStartRunning()
    {
        if (_state != DeerState.PoisedToRun)
        {
            float v = UnityEngine.Random.value;

            if (v > 0.0f && v < 0.3f)
            {
                _animator.SetBool("Feeding", false);
            }
        }
        else
        {
            StartRunning();
        }
    }

    private void StartRunning()
    {
        _targetPositionExcludingHeight = NewRandomTargetPosition(ref _targetPositionIndex);

        _state = DeerState.Running;
    }
}