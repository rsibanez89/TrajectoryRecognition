using UnityEngine;
using System.Collections;
using UC;
using System.Collections.Generic;
using RG;

public class TestingEntrenamiento3D : MonoBehaviour
{

    private string[] pathsEntrenamiento = new string[] { "Assets/BaseDeGestos/Entrenamiento/Circulo/", 
                                                         "Assets/BaseDeGestos/Entrenamiento/Estiramiento/",
                                                         "Assets/BaseDeGestos/Entrenamiento/Nadar/",
                                                         "Assets/BaseDeGestos/Entrenamiento/Smash/"};

    // Use this for initialization
    void Start()
    {
        Entrenamiento entrenamiento = Entrenamiento.obtenerEntrenamiento(pathsEntrenamiento[0]);
        Debug.Log(entrenamiento.Count);
        Entrenamiento3D entrenamiento3D = entrenamiento.obtenerEntrenamiento3D(EnumeradorPartesCuerpo.ManoIzq, true);

        for (int i = 0; i < entrenamiento3D.Count; i++)
            for (int j = 0; j < entrenamiento3D[i].Count; j++)
            {
                Vector3D v = entrenamiento3D[i][j];
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.localScale = new UnityEngine.Vector3(0.1f, 0.1f, 0.1f);
                sphere.transform.position = new UnityEngine.Vector3(v.X, v.Y, v.Z); ;
                sphere.GetComponent<Renderer>().material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            }
    }
}
