using UnityEngine;
using System.Collections;
using UC;
using System.Collections.Generic;

public class GuardarTrayectoriaDeCentroides : MonoBehaviour
{
    private string[] pathsTrayectorias = new string[] { "Assets/Trayectorias/Paper/Smash/T1.dat", 
                                                        "Assets/Trayectorias/Paper/Smash/T3.dat",
                                                        "Assets/Trayectorias/Paper/Smash/T4.dat"};

    private Trayectoria3D centroides;
    private List<GameObject> esferasClusters = new List<GameObject>();
    private bool redibujarEsferas = false;
    public bool rehacerKMeans = false;
    // Use this for initialization
    void Start()
    {
        Trayectoria3D trayectoria3D = new Trayectoria3D();
        
        for(int p = 0; p < pathsTrayectorias.Length; p++)
        {
             trayectoria3D.Add(Trayectoria3D.getTrayectoria(pathsTrayectorias[p]));
        }
        Kmeans3D kmeans = new Kmeans3D(trayectoria3D);

        kmeans.ejecutar(6);

        kmeans.Centroides.salvar("Assets/Trayectorias/Paper/Smash/Centroides.dat");

        trayectoria3D = Trayectoria3D.getTrayectoria(pathsTrayectorias[0]);
        int [] a = kmeans.nearest(trayectoria3D);
        for (int i = 0; i < a.Length; i++)
            Debug.Log(a[i]);

        centroides = kmeans.Centroides;
        redibujarEsferas = true;
    }

    // Update is called once per frame
    void Update()
    {
        if ((esferasClusters != null) && (redibujarEsferas))
        {
            int cant = esferasClusters.Count;
            for (int i = 0; i < cant; i++)
            {
                GameObject eliminar = ((GameObject)esferasClusters[0]);
                esferasClusters.RemoveAt(0);
                Destroy(eliminar);
            }

            if (centroides != null)
            {
                Color random = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                for (int j = 0; j < centroides.Count; j++)
                {
                    Vector3D v = centroides[j];
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.localScale = new UnityEngine.Vector3(0.1f, 0.1f, 0.1f);
                    sphere.transform.position = new UnityEngine.Vector3(v.X, v.Y, v.Z); ;
                    sphere.GetComponent<Renderer>().material.color = random;
                    sphere.name = j.ToString();
                    esferasClusters.Add(sphere);
                }
            }
            redibujarEsferas = false;
        }
    }
}
