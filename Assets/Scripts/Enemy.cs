using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Hits")]
    public int hitsStart = 5;
    public int hitScore = 12;

    [Header("Hit Mode")]
    public float hitModeLength = 0.1f;
    public Color hitModeColor = Color.red;

    [Header("Respawn")]
    public int respawnDelay = 20;
    public float respawnHitGrowth = 1.5f;

    [Header("Death FX")]
    public GameObject explosion;
    public Transform fxContainer;

    // Hit Mode
    private bool hitMode = false;
    private float hitModeExpireTime;

    // Dead Mode
    private bool deadMode = false;
    private float respawnTime;

    // References
    ScoreBoard scoreBoard;
    List<Renderer> renderers;
    List<Material> materials;

    // Properties
    private int hitsRemaining;

    void Start()
    {
        scoreBoard = FindObjectOfType<ScoreBoard>();
        renderers = gameObject.transform.GetChild(0).gameObject.GetComponentsInChildren<Renderer>().ToList();
        materials = renderers.Select(r => r.material).ToList();
        hitsRemaining = hitsStart;
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
        if (hitsRemaining <= 0)
        {
            KillEnemy();
        }
    }

    void Update()
    {
        // Respawn enemy
        if (deadMode && Time.time > respawnTime && !Visible(renderers[0].bounds))
        {
            SetEnabled(true);
            deadMode = false;
        }
        if (hitMode)
        {
            foreach (var material in materials)
            {
                // Change color
                if (material.color != hitModeColor)
                {
                    material.color = hitModeColor;
                }
            }

            // Disable hit mode
            if (Time.time >= hitModeExpireTime)
            {
                hitMode = false;
                foreach (var material in materials)
                {
                    material.color = Color.white;
                }
            }
        }
    }

    private bool Visible(Bounds bounds)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }

    private void ProcessHit()
    {
        hitMode = true;
        hitModeExpireTime = Time.time + hitModeLength;
        scoreBoard.Add(hitScore);
        hitsRemaining--;
    }

    private void KillEnemy()
    {
        SpawnExplosion();
        IncreaseHitValues();
        deadMode = true;
        SetEnabled(false);
        respawnTime = Time.time + respawnDelay;
    }

    private void SpawnExplosion()
    {
        var fx = Instantiate(explosion, transform.position, Quaternion.identity);
        fx.transform.parent = fxContainer;
    }
    private void IncreaseHitValues()
    {
        hitsStart = (int)Mathf.Round(hitsStart * respawnHitGrowth);
        hitsRemaining = hitsStart;
    }

    private void SetEnabled(bool enabled)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = enabled;
        }
        gameObject.GetComponent<Collider>().enabled = enabled;
    }
}
