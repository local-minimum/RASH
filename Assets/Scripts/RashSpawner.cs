using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RashSpawner : MonoBehaviour
{
    [SerializeField]
    Rash Prefab;

    List<Rash> Rashes = new List<Rash>();


    Rash GetRash()
    {
        var unused = Rashes.FirstOrDefault(r => !r.gameObject.activeSelf);
        if (unused != null) return unused;

        var rash = Instantiate(Prefab);
        Rashes.Add(rash);
        return rash;
    }

    float nextSpawn;

    [SerializeField]
    float minSpawnInterval = 0.4f;
    [SerializeField]
    float maxSpawnInterval = 2f;

    [SerializeField, Range(0, 0.1f)]
    float xMargin = 0.05f;

    [SerializeField, Range(0, 0.1f)]
    float yMargin = 0.02f;

    int downSample = 10;

    Vector2Int GetSpawnPixel()
    {
        var maxY = Screen.height / downSample;
        var maxX = Screen.width / downSample;
        int startFill = Mathf.Min(maxY, maxX) / 4;

        var img = new int[maxY, maxX];
        Debug.Log($"{maxX} x {maxY} ({startFill})");

        var cam = Camera.main;
        var places = new List<Vector2Int>();
        foreach (var rash in Rashes)
        {
            if (!rash.gameObject.activeSelf) continue;

            var pos = cam.WorldToScreenPoint(rash.transform.position) / downSample;
            var coords = new Vector2Int(
                Mathf.Clamp(Mathf.RoundToInt(pos.x), 0, maxX - 1), 
                Mathf.Clamp(Mathf.RoundToInt(pos.y), 0, maxY - 1));
            places.Add(coords);
            img[coords.y, coords.x] = startFill;
        }

        int n = 0;

        for (int i = startFill - 1; i > 0; i--)
        {
            for (int j = n, l = places.Count; j < l; j++)
            {
                var c = places[j];
                var c1 = c + Vector2Int.up;
                if (c1.y < maxY && img[c1.y, c1.x] == 0)
                {
                    img[c1.y, c1.x] = i;
                    places.Add(c1);
                }
                var c2 = c + Vector2Int.down;
                if (c2.y >= 0 && img[c2.y, c2.x] == 0)
                {
                    img[c2.y, c2.x] = i;
                    places.Add(c2);
                }
                var c3 = c + Vector2Int.left;
                if (c3.x >= 0 && img[c3.y, c3.x] == 0)
                {
                    img[c3.y, c3.x] = i;
                    places.Add(c3);
                }
                var c4 = c + Vector2Int.right;
                if (c4.x < maxX && img[c4.y, c4.x] == 0)
                {
                    img[c4.y, c4.x] = i;
                    places.Add(c4);
                }
                n++;
            }
        }

        int lowest = startFill;

        for (int y = 0; y <maxY; y++)
        {
            for (int x = 0; x <maxX; x++)
            {
                var val = img[y, x];
                if (val == 0)
                {
                    lowest = 0;
                    Debug.Log("Lowest was 0");
                    break;
                } else if (val < lowest)
                {
                    lowest = val;
                }
            }

            if (lowest == 0)
            {
                break;
            }
        }


        places.Clear();
        lowest++;
        for (int y = 0; y <maxY; y++)
        {
            for (int x = 0; x <maxX; x++)
            {
                if (img[y, x] <= lowest)
                {
                    places.Add(new Vector2Int(x, y));
                }
            }
        }

        Debug.Log($"{lowest}: {string.Join(", ", places)}");

        var place = places[Random.Range(0, places.Count)];
        return new Vector2Int(
           Mathf.RoundToInt((float) place.x/(maxX - 1f) * Screen.width),
           Mathf.RoundToInt((float) place.y/(maxY - 1f) * Screen.height)
        );
    }

    [SerializeField]
    int spawnNoise = 40;

    void Spawn()
    {
        var rash = GetRash();

        var coords = Input.mousePosition + new Vector3(
            Random.Range(-Screen.width / spawnNoise, Screen.width / spawnNoise),
            Random.Range(-Screen.height / spawnNoise * 2, Screen.height / spawnNoise * 2)
            );
        coords.x = Mathf.Clamp(coords.x, 0, Screen.width);
        coords.y = Mathf.Clamp(coords.y, 0, Screen.height);

        var pos = Camera.main.ScreenToWorldPoint(coords);
        pos.z = 0;


        rash.transform.position = pos;
        rash.gameObject.SetActive(true);
        rash.StartItch();

        nextSpawn = Time.timeSinceLevelLoad + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    int ActiveRashes => Rashes.Where(r => r.gameObject.activeSelf).Count();

    [SerializeField]
    int maxRashes = 10;

    private void Update()
    {
        if (Time.timeSinceLevelLoad > nextSpawn && ActiveRashes < maxRashes )
        {
            Spawn();
        }
    }

    private void OnEnable()
    {
        Rash.OnScratch += Rash_OnScratch;
    }

    private void OnDisable()
    {
        Rash.OnScratch -= Rash_OnScratch;
    }

    private void Rash_OnScratch(Rash rash)
    {
        maxRashes = Mathf.Min(maxRashes + Random.Range(0, 2), 20);
        maxSpawnInterval = Mathf.Max(minSpawnInterval, maxSpawnInterval * Random.Range(0.95f, 0.99f));
    }
}
