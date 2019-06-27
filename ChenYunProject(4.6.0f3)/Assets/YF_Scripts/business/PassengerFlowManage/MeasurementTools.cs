using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public sealed class MeasurementTools:MonoBehaviour
{
	public delegate void VoidDelegate();
	public delegate float FloatDelegate();
	public VoidDelegate AddCurrentPoint;
	public VoidDelegate RemoveLastPoint;
	private VoidDelegate DrawAllLines;
	public VoidDelegate Draw;
	private VoidDelegate EndDrawArea;
	private List<Vector3> points = new List<Vector3>();
	private List<GameObject> lines = new List<GameObject>();
	private GameObject tmpLine;
	private GameObject auxLine;
	[HideInInspector]
	public PassengerFlowAreaListItem pfaItem = null;
	private GameObject go;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			if(AddCurrentPoint == null)
			{
				StartDraw();
			}

			if(AddCurrentPoint != null)
			{
				AddCurrentPoint();
			}
		
		}

		if(Input.GetKeyDown(KeyCode.Mouse1))
		{
			if(RemoveLastPoint != null)
			{
				RemoveLastPoint();
			}
		}

		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			if(points.Count >= 3)
			{
				if(CheckLinesIntersect(points,points[0]))
				{
					return;		
				}
				if(Draw != null)
				{
					Draw();
					if(pfaItem != null)
					{
						pfaItem.AreaModify(go, GetVertexesString());
					}
					else 
					{
						gameObject.GetComponent<PassengerFlowAreaManage> ()._AddPassnegerFlowArea (GetVertexesString (), go);
					}
					go = null;
				}
				if(EndDrawArea != null)
				{
					EndDrawArea();
				}
			}
		}

		if(DrawAllLines != null)
		{
			DrawAllLines();
		}
			
	}
	/// <summary>
	/// 按顺序绘制出所有点之间的连线
	/// </summary>
	private void DrawLines()
	{
		if(lines.Count < points.Count - 1)
		{
			GameObject go = new GameObject();
			LineRenderer line = go.AddComponent<LineRenderer>();
			line.material = new Material(Shader.Find("Particles/Alpha Blended"));
			line.material.SetColor("_TintColor",Color.red);
			line.SetWidth(0.5f,0.5f);
			line.SetPosition(0,points[points.Count - 2]);
			line.SetPosition(1,points[points.Count - 1]);
			lines.Add(go);
		}
	}
	/// <summary>
	/// 绘制最后一个选择点，跟第一个点的连线，并进行线段相交性检查。
	/// </summary>
	private void DrawAuxLine()
	{
		if(points.Count >= 3)
		{
			if(auxLine == null)
			{
				auxLine = new GameObject();
				LineRenderer linerend = auxLine.AddComponent<LineRenderer>();
				linerend.material = new Material(Shader.Find("Particles/Alpha Blended"));
				linerend.SetWidth(0.2f,0.2f);
			}
			if(CheckLinesIntersect(points,points[0]))
			{
				auxLine.GetComponent<LineRenderer>().material.SetColor("_TintColor",Color.yellow);
			}
			else
			{
				auxLine.GetComponent<LineRenderer>().material.SetColor("_TintColor",Color.green);
			}
			auxLine.GetComponent<LineRenderer>().SetPosition(0,points[points.Count - 1]);
			auxLine.GetComponent<LineRenderer>().SetPosition(1,points[0]);
		}
	}

	private void DrawTmpLineNotCheckIntersect()
	{
		DrawTmpLine(false);
	}
	private void DrawTmpLineCheckIntersect()
	{
		DrawTmpLine(true);
	}
	/// <summary>
	/// 绘制最后选择的一个点，跟当前鼠标所在位置的连线。
	/// CheckIntersect用来判断是否进行线段相交性检查
	/// </summary>
	/// <param name="CheckIntersect">If set to <c>true</c> check intersect.</param>
	private void DrawTmpLine(bool CheckIntersect)
	{
		if(points.Count >= 1)
		{
			if(tmpLine == null)
			{
				tmpLine = new GameObject();
				tmpLine.AddComponent<LineRenderer>();
				tmpLine.GetComponent<LineRenderer>().material = new Material(Shader.Find("Particles/Alpha Blended"));
			}
			tmpLine.GetComponent<LineRenderer>().SetPosition(0,points[points.Count - 1]);
		}

		if(points.Count >=1)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit,1000,1<<LayerMask.NameToLayer("Ground")))
			{
				tmpLine.GetComponent<LineRenderer>().SetPosition(1,hit.point);
				if(CheckLinesIntersect(points,hit.point) && CheckIntersect)
				{
					tmpLine.GetComponent<LineRenderer>().material.SetColor("_TintColor",Color.yellow);
				}
				else
				{
					tmpLine.GetComponent<LineRenderer>().material.SetColor("_TintColor",Color.red);
				}
			}
		}
	}
	
	/// <summary>
	/// 绘制多边形
	/// </summary>
	/// <param name="vertices">Vertices.</param>
	private void DrawPolygon(List<Vector3> points)
	{
		List<Vector3> vertices = SplitPolygonToTriangles(points);
		Vector3[] vertice = vertices.ToArray();
		int[] triangles = new int[vertice.Length];
		for(int i = 0; i < triangles.Length;i++)
		{
			triangles[i] = i;
		}
		go = new GameObject();
		go.transform.position = new Vector3(0,0.2f,0);
		go.layer = LayerMask.NameToLayer("PassengerFlowArea");
		go.AddComponent<MeshCollider> ();
		MeshRenderer meshrend = go.AddComponent<MeshRenderer>();
		meshrend.material.shader = Shader.Find("Particles/Alpha Blended");
		meshrend.material.SetColor("_TintColor",new Color(0,1,0,0.2f));
		MeshFilter meshFilter = go.AddComponent<MeshFilter>();
		meshFilter.mesh.vertices = vertice;
		meshFilter.mesh.triangles = triangles;
		meshFilter.mesh.RecalculateNormals();
		vertices.Clear();
	}



	public void StartDraw()
	{
		points.Clear();
		AddCurrentPoint = AddAreaPoint;
		Draw += DrawArea;
		EndDrawArea += EndDraw;
		RemoveLastPoint = PrivateRemoveLastPoint;

		DrawAllLines += DrawLines;
		DrawAllLines += DrawAuxLine;
		DrawAllLines += DrawTmpLineCheckIntersect;
	}

	private void AddAreaPoint()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit,1000,1<<LayerMask.NameToLayer("Ground")))
		{


			if(CheckLinesIntersect(points,hit.point))
			{
				return;		
			}
			points.Add(hit.point);

			if(points.Count > 0 && CheckLinesIntersect(points,points[0]))
			{
				points.RemoveAt(points.Count - 1);
				return;		
			}
		}
	}

	private void DrawArea()
	{
		DrawPolygon(points);
	}

	private void EndDraw()
	{
		points.Clear();
		foreach(GameObject go in lines)
		{
			Destroy(go);
		}
		lines.Clear();
		if(tmpLine)Destroy(tmpLine);
		if(auxLine)Destroy(auxLine);
		tmpLine = null;
		auxLine = null;
		AddCurrentPoint = null;
		Draw = null;
		EndDrawArea = null;
		RemoveLastPoint = null;

		this.enabled = false;

	}



	
	/// <summary>
	/// 判断给定的点与指定的多变最后一个点的连线是否与指定的多边形的边相交。
	/// 除去与给定的点有相同的端点的边。
	/// </summary>
	/// <returns><c>true</c>, if lines intersect was checked, <c>false</c> otherwise.</returns>
	/// <param name="points">Points.</param>
	/// <param name="extra">Extra.</param>
	private bool CheckLinesIntersect(List<Vector3> points,Vector3 extra)
	{
		if(points.Count < 3)
		{
			return false;
		}
		for(int i = 0; i <= points.Count - 3; i++)
		{
			if(points[i] == extra)continue;
			if(LinesIntersect(points[i],points[i + 1],points[points.Count - 1],extra))
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 判断给定的连段是否与指定的多边形的边相交。
	/// 除去与给定的线段有相同的端点的边。
	/// </summary>
	/// <returns><c>true</c>, if lines intersect was checked, <c>false</c> otherwise.</returns>
	/// <param name="points">Points.</param>
	/// <param name="lineStart">Line start.</param>
	/// <param name="lineEnd">Line end.</param>
	private bool CheckLinesIntersect(List<Vector3> points,Vector3 lineStart,Vector3 lineEnd)
	{
		for(int i = 0; i < points.Count - 1; i++)
		{
			if(points[i] == lineStart || points[i + 1] == lineStart || points[i] == lineEnd || points[i + 1] == lineEnd)
			{
				continue;
			}
			if(LinesIntersect(points[i],points[i + 1],lineStart,lineEnd))
			{
				return true;
			}
		}
		if(points[points.Count - 1] == lineStart || points[0] == lineStart || points[points.Count - 1] == lineEnd || points[0] == lineEnd)
		{
			return false;
		}
		if(LinesIntersect(points[points.Count - 1],points[0],lineStart,lineEnd))
		{
			return true;
		}
		return false;
	}
	/// <summary>
	/// 判断给定的两条线段是否相交
	/// </summary>
	/// <returns><c>true</c>, if intersect was linesed, <c>false</c> otherwise.</returns>
	/// <param name="line1Start">Line1 start.</param>
	/// <param name="line1End">Line1 end.</param>
	/// <param name="line2Start">Line2 start.</param>
	/// <param name="line2End">Line2 end.</param>
	private bool LinesIntersect(Vector3 line1Start,Vector3 line1End,Vector3 line2Start,Vector3 line2End)
	{
		float value1 = ValueInLine(line1Start,line1End,line2Start);
		float value2 = ValueInLine(line1Start,line1End,line2End);
		if(Mathf.Sign(value1) == Mathf.Sign(value2) && value1 != 0 && value2 != 0)
		{
			return false;
		}
		value1 = ValueInLine(line2Start,line2End,line1Start);
		value2 = ValueInLine(line2Start,line2End,line1End);
		if(Mathf.Sign(value1) == Mathf.Sign(value2) && value1 != 0 && value2 != 0)
		{
			return false;
		}

		return true;
	}
	/// <summary>
	/// 计算点point 在直线line中的值
	/// </summary>
	/// <returns>The in line.</returns>
	/// <param name="lineStart">Line start.</param>
	/// <param name="lineEnd">Line end.</param>
	/// <param name="point">Point.</param>
	private float ValueInLine(Vector3 lineStart,Vector3 lineEnd,Vector3 point)
	{
		float x = point.x;
		float y = point.z;
		float value = (x - lineStart.x)*(lineStart.z - lineEnd.z) - (y - lineStart.z)*(lineStart.x - lineEnd.x);
		return value;
	}
	/// <summary>
	/// 把多边形切割成多个三角形
	/// </summary>
	/// <returns>The polygon to triangles.</returns>
	/// <param name="points">Points.</param>
	private List<Vector3> SplitPolygonToTriangles(List<Vector3> points)
	{
		List<Vector3> vertexes = new List<Vector3>();
		List<Vector3> triangles = new List<Vector3>();
		vertexes.AddRange(points);
		while(true)
		{
			if(vertexes.Count < 3)
			{
				break;
			}
			if(vertexes.Count == 3)
			{
				triangles.Add(vertexes[0]);
				triangles.Add(vertexes[1]);
				triangles.Add(vertexes[2]);
				break;
			}
			for(int i = 2; i < vertexes.Count; i++)
			{
				if(LineOnPolygonInner(points,vertexes,vertexes[0],vertexes[i]))
				{
					triangles.Add(vertexes[0]);
					triangles.Add(vertexes[i - 1]);
					triangles.Add(vertexes[i]);
				}
				else
				{
					if(i > 2)
					{
						vertexes.RemoveRange(1, i - 2);
					}
					vertexes.Add(vertexes[0]);
					vertexes.RemoveAt(0);
					break;
				}
				if(i >= vertexes.Count - 1)
				{
					vertexes.Clear();
					break;
				}
			}
		}

		return triangles;
	}
	/// <summary>
	/// 判断给定的线段是否在指定的多边形内部
	/// </summary>
	/// <returns><c>true</c>, 给定的线段在指定的多边形内部, <c>false</c> otherwise.</returns>
	/// <param name="points">Points.</param>
	/// <param name="lineStart">Line start.</param>
	/// <param name="lineEnd">Line end.</param>
	private bool LineOnPolygonInner(List<Vector3> points,List<Vector3> vertexes,Vector3 lineStart,Vector3 lineEnd)
	{
		if(CheckLinesIntersect(points,lineStart,lineEnd))
		{
			return false;
		}
		if(PointOnPolygonInner(vertexes,(lineStart + lineEnd) * 0.5f))
		{
			return true;
		}
		return false;
	}
	/// <summary>
	/// 判断给定的点是否在指定的多边形内部
	/// </summary>
	/// <returns><c>true</c>, 给定的点在指定的多边形内部, <c>false</c> otherwise.</returns>
	/// <param name="points">Points.</param>
	/// <param name="point">Point.</param>
	private bool PointOnPolygonInner(List<Vector3> points, Vector3 point)
	{
		int crossPointCout = 0;
		Vector3 lineStart = new Vector3(float.MaxValue,0,0);
		int count = 0;
		for(int i = 0; i < points.Count - 1;i++)
		{
			if(LinesIntersect(points[i],points[i + 1],lineStart,point))
			{
				crossPointCout++;
				count++;
			}
			else
			{
				count = 0;
			}
			if(count >= 2)
			{
				crossPointCout -= (count - 1);
				count = 1;
			}
		}
		if(LinesIntersect(points[points.Count - 1],points[0],lineStart,point))
		{
			crossPointCout++;
		}
		if(crossPointCout % 2 != 0)
		{
			return true;
		}

		return false;
	}
	/// <summary>
	/// 移除最后一个选择的点
	/// </summary>
	private void PrivateRemoveLastPoint()
	{
		if(points.Count > 0)
		{
			points.RemoveAt(points.Count - 1);
			if( points.Count < 1)
			{
				Destroy(tmpLine);
				tmpLine = null;
			}
			if( points.Count < 3 && auxLine != null)
			{
				Destroy(auxLine);
				auxLine = null;
			}

		}
		if(lines.Count > 0)
		{
			Destroy(lines[lines.Count - 1]);
			lines.RemoveAt(lines.Count - 1);
		}
	}

	void OnDisable()
	{
		EndDraw();
	}
	/// <summary>
	/// 把多边形的顶点坐标转换成用‘|’分割的字符串
	/// </summary>
	/// <returns>The vertexes string.</returns>
	private string GetVertexesString()
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		foreach(var point in SplitPolygonToTriangles(points))
		{
			sb.Append(point.x);
			sb.Append('|');
			sb.Append(point.y);
			sb.Append('|');
			sb.Append(point.z);
			sb.Append('|');
		}
		if(sb.Length > 0)
		{
			sb.Remove(sb.Length - 1,1);
		}
		return sb.ToString();
	}
}
