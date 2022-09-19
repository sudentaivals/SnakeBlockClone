using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] int _startStacks;
    [SerializeField] TMP_Text _text;
    [SerializeField] float _dissolveTimer = 0.15f;
    [SerializeField][Range(0, 1)] float _dissolveMax;
    [SerializeField][Range(-1, 0)] float _dissolveMin;
    [Header("VFX/SFX")]
    [SerializeField] AudioClip _blockDestoySfx;
    [SerializeField][Range(0, 1)] float _blockDestroySfxVolume;
    [SerializeField] GameObject _deathVfx;
    [SerializeField] Material _dissolveMaterial;

    private float _currentTimer = 0;
    private MaterialPropertyBlock _dissolveBlock;
    private Renderer _renderer;

    private int _currentStacks;
    private int CurrentStacks
    {
        get
        {
            return _currentStacks;
        }
        set
        {
            _currentStacks = value;
            UpdateMaterial();
            UpdateText(_currentStacks);
        }
    }

    private readonly int _maxStacks = 30;
    private Material _blockMaterial;
    private MeshRenderer _meshRenderer;
    private readonly string _thickness = "_Thickness";
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _dissolveBlock = new MaterialPropertyBlock();
        _meshRenderer = GetComponent<MeshRenderer>();
        _blockMaterial = new Material(_meshRenderer.material);
        _meshRenderer.material = _blockMaterial;
        SetStacks(_startStacks);
    }

    private void SetStacks(int newToughness)
    {
        if(newToughness <= 0)
        {
            DestroyCube();
        }
        else
        {
            CurrentStacks = newToughness;
        }
    }

    private void UpdateMaterial()
    {
        float newThickness = _currentStacks / (float)_maxStacks;
        _blockMaterial.SetFloat(_thickness, newThickness);

    }

    private void UpdateText(int stacaks)
    {
        _text.text = stacaks.ToString();
    }

    public void DecreaseStacks()
    {
        var newToughness = CurrentStacks - 1;
        SetStacks(newToughness);
    }

    private void DestroyCube()
    {
        //var vfx = Instantiate(_deathVfx, transform.position, _deathVfx.transform.rotation);
        //Destroy(vfx, 1.5f);
        EventBus.Publish(EventBusEvent.PlaySound, this, new PlaySoundEventArgs(_blockDestroySfxVolume, _blockDestoySfx));
        Destroy(gameObject, _dissolveTimer + 0.02f);
        _renderer.material = _dissolveMaterial;
        _currentTimer = _dissolveTimer;
        Destroy(GetComponent<BoxCollider>());
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }

    }

    private void OnValidate()
    {
        UpdateText(_startStacks);
    }

    void Update()
    {
        if(_currentTimer > 0)
        {
            _currentTimer -= Time.deltaTime;
            var newDissolve = Mathf.Lerp(_dissolveMin, _dissolveMax, _currentTimer / _dissolveTimer);
            UpdateDissolve(newDissolve);
        }
    }

    private void UpdateDissolve(float newDissolve)
    {
        _renderer.GetPropertyBlock(_dissolveBlock);
        _dissolveBlock.SetFloat("_CutoffHeight", newDissolve);
        _renderer.SetPropertyBlock(_dissolveBlock);
    }
}
