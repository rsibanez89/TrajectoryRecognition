using UnityEngine;
using System.Collections.Generic;
using UC;

public class TestingTrayectoria3D : MonoBehaviour {

	private string pathTrayectoia3D = "Assets/Trayectorias/test.dat";
	private string pathTrayectoia3DNormalizada = "Assets/Trayectorias/test-normalizada.dat";

	private List<GameObject> trayectoriaOriginal = new List<GameObject>();
	private List<GameObject> trayectoriaNormalizada = new List<GameObject>();

	// Use this for initialization
	void Start () {
		GameObject centro = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		centro.name = "Origen de Coordenadas";
		centro.transform.localScale = new UnityEngine.Vector3(0.2f, 0.2f, 0.2f);
		centro.transform.position = new UnityEngine.Vector3(0, 0, 0);
		centro.GetComponent<Renderer>().material.color = new Color(1,0,0);


		Trayectoria3D trayectoria = Trayectoria3D.getTrayectoria (pathTrayectoia3D);
		dibujarTrayectoria(trayectoria, new Color(0,0,1), trayectoriaOriginal, "Trayectoria Original");
		trayectoria.normalizar ();
		dibujarTrayectoria(trayectoria, new Color(0,1,0), trayectoriaOriginal, "Trayectoria Normalizada");
		trayectoria.salvar (pathTrayectoia3DNormalizada);

		dibujarExtremos (trayectoria);
	}
	

	private void dibujarTrayectoria (Trayectoria3D trayectoria, Color c, List<GameObject> lista, string nombreLista) {
		GameObject paret = new GameObject();
		paret.name = nombreLista;
		for (int i = 0; i < trayectoria.Count; i++) {
			Vector3D v = trayectoria[i];
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.transform.localScale = new UnityEngine.Vector3(0.1f, 0.1f, 0.1f);
			sphere.transform.position = new UnityEngine.Vector3(v.X, v.Y, v.Z); ;
			sphere.GetComponent<Renderer>().material.color = c;
			sphere.transform.parent = paret.transform;
			lista.Add(sphere);
		}
	}

	private void dibujarExtremos(Trayectoria3D trayectoria)
	{
		Vector3 p1 = new Vector3 (getMin (trayectoria, "x"), getMin (trayectoria, "y"), getMin (trayectoria, "z"));
		Vector3 p2 = new Vector3 (getMin (trayectoria, "x"), getMin (trayectoria, "y"), getMax (trayectoria, "z"));
		Vector3 p3 = new Vector3 (getMin (trayectoria, "x"), getMax (trayectoria, "y"), getMin (trayectoria, "z"));
		Vector3 p4 = new Vector3 (getMin (trayectoria, "x"), getMax (trayectoria, "y"), getMax (trayectoria, "z"));

		Vector3 p5 = new Vector3 (getMax (trayectoria, "x"), getMin (trayectoria, "y"), getMin (trayectoria, "z"));
		Vector3 p6 = new Vector3 (getMax (trayectoria, "x"), getMin (trayectoria, "y"), getMax (trayectoria, "z"));
		Vector3 p7 = new Vector3 (getMax (trayectoria, "x"), getMax (trayectoria, "y"), getMin (trayectoria, "z"));
		Vector3 p8 = new Vector3 (getMax (trayectoria, "x"), getMax (trayectoria, "y"), getMax (trayectoria, "z"));

		dibujarEsfera (p1);
		dibujarEsfera (p2);
		dibujarEsfera (p3);
		dibujarEsfera (p4);

		dibujarEsfera (p5);
		dibujarEsfera (p6);
		dibujarEsfera (p7);
		dibujarEsfera (p8);
	}

	private void dibujarEsfera(Vector3 punto)
	{
		GameObject extremo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		extremo.transform.localScale = new UnityEngine.Vector3(0.2f, 0.2f, 0.2f);
		extremo.transform.position = punto;
		extremo.GetComponent<Renderer>().material.color = new Color(1,1,1);
	}

	private float getMax(Trayectoria3D trayectoria, string coordenada)
	{
		float max = -1000000;
		float value = 0;
		for (int i = 0; i < trayectoria.Count; i++) {
			switch (coordenada) {
				case "x":
						value = trayectoria [i].X;
						break;
				case "y":
						value = trayectoria [i].Y;
						break;
				case "z":
						value = trayectoria [i].Z;
						break;
				}
			if(value > max)
				max = value;
			}
		return max;
	}

	private float getMin(Trayectoria3D trayectoria, string coordenada)
	{
		float min = 1000000;
		float value = 0;
		for (int i = 0; i < trayectoria.Count; i++) {
			switch (coordenada) {
			case "x":
				value = trayectoria [i].X;
				break;
			case "y":
				value = trayectoria [i].Y;
				break;
			case "z":
				value = trayectoria [i].Z;
				break;
			}
			if(value < min)
				min = value;
		}
		return min;
	}


}
