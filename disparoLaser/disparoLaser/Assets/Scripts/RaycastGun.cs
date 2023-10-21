using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class RaycastGun : MonoBehaviour
{   //
    public Camera playerCamera;
    public Transform laserOrigin;
    public float gunRange = 50f;
    public float fireRate = 0.2f;
    public float laserDuration = 0.05f;
    //
    public int cubosDestruidos = 0;
    public int esferasDestruidas = 0;
    //
    LineRenderer laserLine;
    float fireTimer;
    Dictionary<GameObject, int> cubeHits = new Dictionary<GameObject, int>();
    Dictionary<GameObject, int> sphereHits = new Dictionary<GameObject, int>();
    public Text contadorTexto;

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && fireTimer > fireRate)
        {
            fireTimer = 0;
            laserLine.SetPosition(0, laserOrigin.position);
            Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, gunRange))
            {
                laserLine.SetPosition(1, hit.point);

                if (hit.transform.name.Contains("Cube"))
                {
                    HandleHit(hit.transform.gameObject, cubeHits);
                }
                else if (hit.transform.name.Contains("Sphere"))
                {
                    HandleHit(hit.transform.gameObject, sphereHits);
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (playerCamera.transform.forward * gunRange));
            }

            StartCoroutine(ShootLaser());
        }
    }

    IEnumerator ShootLaser()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }

    void HandleHit(GameObject hitObject, Dictionary<GameObject, int> hitDict)
    {
        if (hitDict.ContainsKey(hitObject))
        {
            hitDict[hitObject]++;

            if (hitDict[hitObject] >= 2 && hitObject.name.Contains("Cube"))
            {
                Destroy(hitObject);
                hitDict.Remove(hitObject);
                cubosDestruidos++;
            }
            else if (hitDict[hitObject] >= 3 && hitObject.name.Contains("Sphere"))
            {
                Destroy(hitObject);
                hitDict.Remove(hitObject);
                esferasDestruidas++;
            }

            if (contadorTexto != null)
            {
                contadorTexto.text = "Cubos Destruidos: " + cubosDestruidos + "\nEsferas Destruidas: " + esferasDestruidas;
            }
        }
        else
        {
            hitDict.Add(hitObject, 1);
        }
    }
}
