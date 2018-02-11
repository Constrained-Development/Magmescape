using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject skull;

    private List<Vector3> locations;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        locations = new List<Vector3>();
    }

    public void AddLocation(Vector3 loc)
    {
        locations.Add(loc);
    }

    public void SpawnSkulls()
    {
        foreach (var loc in locations)
        {
            Instantiate(skull, loc, skull.transform.rotation);
        }
    }
}
