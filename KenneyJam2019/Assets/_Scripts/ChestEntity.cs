using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestEntity : MonoBehaviour
{
    public ChestSettings ChestSettings;

    private Renderer _renderer;
    private Collider _collider;
    private Renderer _lidRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
        _lidRenderer = transform.Find("lid").GetComponent<Renderer>();

        SetChestStatus(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.value <= ChestSettings.ChanceForRespawn)
        {
            SetChestStatus(true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        var shipsLayer = LayerMask.NameToLayer("Ships");
        if (collision.gameObject.layer == shipsLayer)
        {
            SetChestStatus(false);
        }
    }

    void SetChestStatus(bool status)
    {
        _renderer.enabled = status;
        _collider.enabled = status;
        _lidRenderer.enabled = status;
    }
}
