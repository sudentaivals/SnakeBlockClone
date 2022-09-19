using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SegmentManager : MonoBehaviour
{
    [SerializeField] Transform _snake;
    [SerializeField] GameObject _segment;
    [SerializeField][Range(1, 30)] int _startSegmentQuantity = 3;
    [SerializeField] Material _headMaterial;
    [SerializeField] Material _segmentMaterial;
    [SerializeField] float _segmentRange = 1f;
    [Header("Collision Shape")]
    [SerializeField][Range(0f, 10f)] float _sphereRadius;
    [SerializeField] Vector3 _offset;
    [SerializeField] LayerMask _boxMask;
    [Header("Collision stats")]
    [SerializeField] float _blockInteractionCooldown = 0.1f;
    [SerializeField] GameObject _segmentPopVfx;
    [SerializeField] AudioClip _segmentPopSfx;
    [SerializeField][Range(0f, 1f)] float _segmentPopSfxVolume = 1;

    private float _currentCooldown = 0;
    private bool IsEatingAvailable => _currentCooldown <= 0;

    private Collider[] _overlappedBoxes = new Collider[4];
    private bool IsBoxNearby => Physics.OverlapSphereNonAlloc(_snake.position + _offset, _sphereRadius, _overlappedBoxes, _boxMask) > 0;

    private List<GameObject> _segments;

    public int CurrentSegments => _segments.Count;
    private GameObject FirstSegment => _segments.FirstOrDefault();
    void Start()
    {
        _segments = new List<GameObject>();
        AddSegments(_startSegmentQuantity);
    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventBusEvent.FoodPickup, FoodPickup);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventBusEvent.FoodPickup, FoodPickup);
    }

    private void FoodPickup(UnityEngine.Object obj, EventArgs args)
    {
        var segmentsArgs = args as AddPlayerSegmentEventArgs;
        int segmentsToAdd = segmentsArgs.SegmentsToAdd;
        AddSegments(segmentsToAdd);
    }

    private void AddSegments(int chainsToSpawn)
    {
        for (int i = 0; i < chainsToSpawn; i++)
        {
            var segment = Instantiate(_segment);
            _segments.Add(segment);
            segment.transform.parent = transform;
            segment.transform.position = _segments.Count == 1 ? _snake.position : _segments[_segments.Count - 2].transform.position + Vector3.back;
        }
        EventBus.Publish(EventBusEvent.SegmentsQuntityChanged, this, null);
        SetHeadSegmentMaterial();
    }

    private void RemoveSegment()
    {
        var oldPositions = _segments.Select(a => a.transform.position).ToList();
        Destroy(FirstSegment.gameObject);
        _segments.RemoveAt(0);
        var vfx = Instantiate(_segmentPopVfx);
        vfx.transform.position = _snake.position;
        Destroy(vfx, 1f);
        EventBus.Publish(EventBusEvent.PlaySound, this, new PlaySoundEventArgs(_segmentPopSfxVolume, _segmentPopSfx));
        EventBus.Publish(EventBusEvent.SegmentsQuntityChanged, this, null);
        if(_segments.Count == 0)
        {
            GameManager.Instance.TriggerGameState(GameState.GameOver);
            return;
        }
        /*for (int i = 1; i < _segments.Count; i++)
        {
            _segments[i].transform.position = oldPositions[i];
        }*/
        SetHeadSegmentMaterial();
    }

    private void SetHeadSegmentMaterial()
    {
        if (_segments.Count == 0) return;
        var headMaterial = new Material(_headMaterial);
        FirstSegment.GetComponent<MeshRenderer>().material = headMaterial;

    }

    private void UpdatePosition()
    {
        if (_segments.Count == 0) return;
        var oldFirstSegmentPosition = _segments.First().transform.position;
        FirstSegment.transform.position = _snake.transform.position;
        //if (FirstSegment.transform.position == oldFirstSegmentPosition) return;

        MoveSegmentsDirection();
    }

    private void MoveSegmentsDirection()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            var direction = (_segments[i].transform.position - _segments[i - 1].transform.position).normalized;
            _segments[i].transform.position = _segments[i - 1].transform.position + direction * _segmentRange;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_snake.position + _offset, _sphereRadius);
    }

    void Update()
    {
        UpdatePosition();
        UpdateCooldown();
        BlockEatingLogic();
    }

    private void BlockEatingLogic()
    {
        if (_segments.Count == 0) return;
        if (!IsEatingAvailable) return;
        if (!IsBoxNearby) return;

        _currentCooldown = _blockInteractionCooldown;
        var firstBlock = _overlappedBoxes.Where(a => a != null).OrderBy(a => (a.transform.position - (_snake.position + _offset)).magnitude).First();
        firstBlock.gameObject.GetComponent<Block>().DecreaseStacks();
        RemoveSegment();
    }

    private void UpdateCooldown()
    {
        if(_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;
        }
    }
}
