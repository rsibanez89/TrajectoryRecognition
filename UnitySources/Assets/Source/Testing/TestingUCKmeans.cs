using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UC;
using System;

public class TestingUCKmeans : MonoBehaviour
{

    private string[] pathsEntrenamiento = new string[] { "Assets/Trayectorias/Entrenamiento/Circulo/", 
                                                         "Assets/Trayectorias/Entrenamiento/Estiramiento/",
                                                         "Assets/Trayectorias/Entrenamiento/Nadar/",
                                                         "Assets/Trayectorias/Entrenamiento/Smash/"};

    private List<GameObject> esferasClusters = new List<GameObject>();
    private bool redibujarEsferas = false;
    public int cantClusters = 10;
    public bool rehacerKMeans = false;
    private string clusters = "10";

    private Trayectoria3D trayectoria3D = new Trayectoria3D();
    private Kmeans3D Kmeans;
    private Trayectoria3D centroides = new Trayectoria3D();

    // Use this for initialization
    void Start()
    {
        Entrenamiento3D e = Entrenamiento3D.getEntrenamiento(pathsEntrenamiento[3]);

        Debug.Log(e.Count);

        trayectoria3D = e.planchar();

        Debug.Log(trayectoria3D.Count);

        Kmeans = new Kmeans3D(trayectoria3D);

        List<Trayectoria3D> t = trayectoria3D.Split(cantClusters);

        /*for (int j = 0; j < trayectoria3D.Count; j++)
        {
            Vector3D v = trayectoria3D[j];
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new UnityEngine.Vector3(0.1f, 0.1f, 0.1f);
            sphere.transform.position = new UnityEngine.Vector3(v.X, v.Y, v.Z); ;
            sphere.renderer.material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        }*/

        Kmeans.ejecutar(cantClusters);
        centroides = Kmeans.Centroides;

        redibujarEsferas = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (rehacerKMeans)
        {
            Kmeans = new Kmeans3D(trayectoria3D);
            Kmeans.ejecutar(cantClusters);
            centroides = Kmeans.Centroides;

            redibujarEsferas = true;
            rehacerKMeans = false;
        }

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
                    esferasClusters.Add(sphere);
                }
            }
            redibujarEsferas = false;
        }
    }


    void OnGUI()
    {
        #region GUI panel inferior
        GUI.BeginGroup(new Rect(0, Screen.height - 25, Screen.width, 25));
        GUI.Box(new Rect(0, 0, Screen.width, 100), "");

        GUILayout.BeginArea(new Rect(0, 0, 200, 25));
        GUILayout.BeginHorizontal();

        clusters = GUILayout.TextField(clusters);
        int aux = cantClusters;
        try
        {
            aux = System.Convert.ToInt32(clusters);
        }
        catch (Exception)
        {
            aux = cantClusters;
        }
        cantClusters = aux;

        if (GUILayout.Button("Genera k-means"))
            rehacerKMeans = true;

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        GUI.EndGroup();
        #endregion
    }
}
