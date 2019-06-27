using UnityEngine;
using System.Collections;

public class DrawSector : MonoBehaviour {

	public int Scope = 45;
	public int Radio = 1;
	public int Offset = 0;
	Mesh newMesh;
	Vector3[] vert;
	Vector2[] uvs;
	int[] tri;

	void OnEnable()
	{
		ReDrawSector();

	}

	public void ReDrawSector()
	{
		//if(Scope <=0 || Radio <=0) return;
		transform.localRotation = Quaternion.Euler(0,0,0);
		newMesh = new Mesh();
		vert = new Vector3[Scope + 1];
		uvs = new Vector2[Scope + 1];
		GetComponent<MeshFilter>().mesh = newMesh;
		vert[0] = new Vector3(0,0,0);
		for(int i = 0; i < Scope; i++)
		{
			vert[i+1] = new Vector3(Mathf.Cos(Mathf.Deg2Rad * i) * Radio,0,Mathf.Sin(Mathf.Deg2Rad * i) * Radio);
		}
		
		uvs[0] = new Vector2(0,0);
		for(int i = 0; i < Scope; i++)
		{
			uvs[i+1] = new Vector2(Mathf.Cos(Mathf.Deg2Rad * i),Mathf.Sin(Mathf.Deg2Rad * i));
		}
		if(Scope < 360)
		{
			tri = new int[Scope * 3];
			for(int i = 0; i < Scope - 1;i++)
			{
				tri[i * 3] = 0;
				tri[i * 3 + 1] = 2 + i;
				tri[i * 3 + 2] = 1 + i;
			}
		}
		else
		{
			tri = new int[Scope * 3 + 3];
			for(int i = 0; i < Scope - 1;i++)
			{
				tri[i * 3] = 0;
				tri[i * 3 + 1] = 2 + i;
				tri[i * 3 + 2] = 1 + i;
			}

			tri[(Scope - 1) * 3] = 0;
			tri[(Scope - 1) * 3 + 1] = 1;
			tri[(Scope - 1) * 3 + 2] = Scope;
		}
		
		newMesh.vertices = vert;
		newMesh.uv = uvs;
		newMesh.triangles = tri;

		transform.localRotation = Quaternion.Euler(0,0 - Offset,0);
	}
}
