using UnityEngine;
using System.Collections;
using UC;
using System.Text;
using RT;
using System.IO;
using System.Diagnostics;
using System;

public class RTConDTW : MonoBehaviour
{

    private string[] pathsEntrenamiento = new string[] { "Assets/Trayectorias/Entrenamiento/Circulo/", 
                                                         "Assets/Trayectorias/Entrenamiento/Estiramiento/",
                                                         "Assets/Trayectorias/Entrenamiento/Nadar/",
                                                         "Assets/Trayectorias/Entrenamiento/Smash/"};

    private string[] pathsExperimento = new string[] { "Assets/Trayectorias/Experimentos/Circulo/", 
                                                       "Assets/Trayectorias/Experimentos/Estiramiento/",
                                                       "Assets/Trayectorias/Experimentos/Nadar/",
                                                       "Assets/Trayectorias/Experimentos/Smash/"};

    private StringBuilder cantReconocidos = new StringBuilder();

    // Use this for initialization
    void Start()
    {
        Test1CantidadDeGestosReconocidos();
        //Test2TiemposDeReconocimiento();
    }

    public void Test1CantidadDeGestosReconocidos()
    {
        RTDTW3D reconocedor = new RTDTW3D();
        entrenarReconocedor(reconocedor);
        evaluarReconocedor(reconocedor);

        //StreamWriter writer = new StreamWriter("CantReconocidos.txt");
        //writer.Write(cantReconocidos.ToString());
        //writer.Close();
    }

    public void Test2TiemposDeReconocimiento()
    {
        const int repetitions = 50;
        long[] times = new long[repetitions];
        RTDTW3D reconocedor = new RTDTW3D();
        entrenarReconocedor(reconocedor);
        for (int i = 0; i < repetitions; i++)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            evaluarReconocedor(reconocedor);

            stopwatch.Stop();
            times[i] = stopwatch.ElapsedMilliseconds;
        }

        // Sort the elapsed times for all test runs
        Array.Sort(times);

        // Calculate the total times discarding
        // the 5% min and 5% max test times
        long totalTime = 0;
        int discardCount = (int)Math.Round(repetitions * 0.05);
        int count = repetitions - discardCount;
        for (int i = discardCount; i < count; i++)
        {
            totalTime += times[i];
        }

        double averageTime = ((double)totalTime) / (count - discardCount);

        UnityEngine.Debug.Log("Tiempo promedio: " + averageTime / 1000);
    }

    private void entrenarReconocedor(RTDTW3D reconocedor)
    {
        for (int e = 0; e < pathsEntrenamiento.Length; e++)
        {
            Entrenamiento3D entrenamiento = Entrenamiento3D.getEntrenamiento(pathsEntrenamiento[e]);
            reconocedor.addEntrenamiento(entrenamiento);
        }
        reconocedor.entrenar();
    }

    private void evaluarReconocedor(RTDTW3D reconocedor)
    {
        int cantCirculos = 0;
        int cantEstiramientos = 0;
        int cantNadar = 0;
        int cantSmash = 0;
        int reconocidosCorrectos = 0;
        int noReconocidos = 0;
        float distancia;

        for (int e = 0; e < pathsExperimento.Length; e++)
        {
            Entrenamiento3D experimento = Entrenamiento3D.getEntrenamiento(pathsExperimento[e]);
            for (int t = 0; t < experimento.Count; t++)
            {
                int gestoReconocido = reconocedor.evaluar(experimento[t], out distancia);


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
        //cantReconocidos.Append(reconocidosCorrectos + "\t");
		UnityEngine.Debug.Log("RECONOCIDO: " + reconocidosCorrectos);
		UnityEngine.Debug.Log("NORECONOCIDO: " + noReconocidos);
		UnityEngine.Debug.Log("CIRCULO: " + cantCirculos);
		UnityEngine.Debug.Log("ESTIRAMIENTO: " + cantEstiramientos);
		UnityEngine.Debug.Log("NADAR: " + cantNadar);
		UnityEngine.Debug.Log("SMASH: " + cantSmash);
    }
}
