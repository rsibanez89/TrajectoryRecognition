using UnityEngine;
using System.Collections;
using UC;
using RT;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class RTConStringMatching : MonoBehaviour
{
    private List<GameObject> esferasClusters = new List<GameObject>();
    private bool redibujarEsferas = false;
    private Trayectoria3D centroides = new Trayectoria3D();

    private string[] pathsEntrenamiento = new string[] { "Assets/Trayectorias/Entrenamiento/Circulo/", 
                                                         "Assets/Trayectorias/Entrenamiento/Estiramiento/",
                                                         "Assets/Trayectorias/Entrenamiento/Nadar/",
                                                         "Assets/Trayectorias/Entrenamiento/Smash/"};

    private string[] pathsExperimento = new string[] { "Assets/Trayectorias/Experimentos/Circulo/", 
                                                       "Assets/Trayectorias/Experimentos/Estiramiento/",
                                                       "Assets/Trayectorias/Experimentos/Nadar/",
                                                       "Assets/Trayectorias/Experimentos/Smash/"};

    private int cantClusters;
    private StringBuilder cantReconocidos = new StringBuilder();

    // Use this for initialization
    void Start()
    {
        for (cantClusters = 1; cantClusters < 100; cantClusters++)
        {
            //Entreno el reconocedor
            RTStringMatching3D reconocedor = new RTStringMatching3D();
            entrenarReconocedor(reconocedor, cantClusters);
            guardarTrayectoriasAceptadas(reconocedor);

            /*this.cantClusters = cantClusters;
            centroides = reconocedor.Centroides;
            redibujarEsferas = true;*/

            evaluarReconocedor(reconocedor);
        }

        //StreamWriter writer = new StreamWriter("CantReconocidos.txt");
        //writer.Write(cantReconocidos.ToString());
        //writer.Close();

		Debug.Log (cantReconocidos.ToString ());
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
                    if (j % cantClusters == 0)
                        random = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
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

    private void entrenarReconocedor(RTStringMatching3D reconocedor, int cantClusters)
    {
        for (int e = 0; e < pathsEntrenamiento.Length; e++)
        {
            Entrenamiento3D entrenamiento = Entrenamiento3D.getEntrenamiento(pathsEntrenamiento[e]);
            reconocedor.addEntrenamiento(entrenamiento, cantClusters);
        }
        reconocedor.entrenar();
    }

    private void guardarTrayectoriasAceptadas(RTStringMatching3D reconocedor)
    {
        StringBuilder ta = new StringBuilder();
        List<string>[] secuenciasAceptadasPorGesto = reconocedor.getSecuenciasAceptadasPorGesto();
        for (int nGesto = 0; nGesto < secuenciasAceptadasPorGesto.Length; nGesto++)
        {
            ta.Append("GESTO N " + nGesto + "\n");
            for (int s = 0; s < secuenciasAceptadasPorGesto[nGesto].Count; s++)
                ta.Append("\t" + secuenciasAceptadasPorGesto[nGesto][s] + "\n");
        }
        StreamWriter writer = new StreamWriter("TrayectoriasAceptadas.txt");
        writer.Write(ta.ToString());
        writer.Close();
    }

    private void evaluarReconocedor(RTStringMatching3D reconocedor)
    {
        int cantCirculos = 0;
        int cantEstiramientos = 0;
        int cantNadar = 0;
        int cantSmash = 0;
        int reconocidosCorrectos = 0;
        int noReconocidos = 0;

        for (int e = 0; e < pathsExperimento.Length; e++)
        {
            Entrenamiento3D experimento = Entrenamiento3D.getEntrenamiento(pathsExperimento[e]);
            for (int t = 0; t < experimento.Count; t++)
            {
                int gestoReconocido = reconocedor.evaluar(experimento[t]);
                
                if (gestoReconocido == e)
                    reconocidosCorrectos++;
                else
                    noReconocidos++;

                switch (gestoReconocido)
                {
                    case 0:
                        if (e == 0)
                            cantCirculos++;
                        break;
                    case 1:
                        if (e == 1)
                            cantEstiramientos++;
                        break;
                    case 2:
                        if (e == 2)
                            cantNadar++;
                        break;
                    case 3:
                        if (e == 3)
                            cantSmash++;
                        break;
                }
            }
        }
        cantReconocidos.Append(cantClusters + " clusters: " + reconocidosCorrectos + "\n");
        //Debug.Log("RECONOCIDO: " + reconocidosCorrectos);
        //Debug.Log("NORECONOCIDO: " + noReconocidos);
        //Debug.Log("CIRCULO: " + cantCirculos);
        //Debug.Log("ESTIRAMIENTO: " + cantEstiramientos);
        //Debug.Log("NADAR: " + cantNadar);
        //Debug.Log("SMASH: " + cantSmash);
    }
}
