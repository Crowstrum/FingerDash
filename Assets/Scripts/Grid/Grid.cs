using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class Grid : MonoBehaviour
{
    private Spring[] springs;
    private PointMass[,] points;
    private List<GameObject> vectorLineList; 
    public Rect size;
    public Vector2 spacing;
    public Camera cam;
    public GameObject go;
    

    void Start()
    {
        
        vectorLineList = new List<GameObject>();
        List<Spring> springList = new List<Spring>();

        int numColumns = (int) (size.width/spacing.x) + 1;
        int numRows = (int) (size.height/spacing.y) + 1;
        points = new PointMass[numColumns, numRows];

        // these fixed points will be used to anchor the grid to fixed positions on the screen
        PointMass[,] fixedPoints = new PointMass[numColumns, numRows];

        // create the point masses
        int column = 0, row = 0;
        for (float y = size.yMin; y <= size.yMax; y += spacing.y)
        {
            for (float x = size.xMin; x <= size.xMax; x += spacing.x)
            {
                points[column, row] = new PointMass(new Vector3(x, y, 0), 1,go);
                fixedPoints[column, row] = new PointMass(new Vector3(x, y, 0), 0,go);
                column++;
            }
            row++;
            column = 0;
        }

        // link the point masses with springs
        for (int y = 0; y < numRows; y++)
            for (int x = 0; x < numColumns; x++)
            {
                if (x == 0 || y == 0 || x == numColumns - 1 || y == numRows - 1) // anchor the border of the grid 
                    springList.Add(new Spring(fixedPoints[x, y], points[x, y], 0.1f, 0.1f));
                else if (x%3 == 0 && y%3 == 0) // loosely anchor 1/9th of the point masses 
                    springList.Add(new Spring(fixedPoints[x, y], points[x, y], 0.002f, 0.02f));

                const float stiffness = 0.28f;
                const float damping = 0.06f;
                if (x > 0)
                    springList.Add(new Spring(points[x - 1, y], points[x, y], stiffness, damping));
                if (y > 0)
                    springList.Add(new Spring(points[x, y - 1], points[x, y], stiffness, damping));
            }

        springs = springList.ToArray();

        
    }

    public void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray r = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r,out hit,100f))
            {
                if (hit.transform.gameObject.tag == "Cube")
                {
                    ApplyExplosiveForce(5f,hit.transform.position, 2f);
                }
            }
           
            
        }
        foreach (var spring in springs)
            spring.Update();

        foreach (var mass in points)
        {
            mass.Update();

          
        }

       
    }

    public void ApplyDirectedForce(Vector3 force, Vector3 position, float radius)
    {
        foreach (var mass in points)
            if (Vector3.Distance(position, mass.Position) < radius * radius)
                mass.ApplyForce(10 * force / (10 + Vector3.Distance(position, mass.Position)));
    }

    public void ApplyImplosiveForce(float force, Vector3 position, float radius)
    {
        foreach (var mass in points)
        {
            float dist2 = Vector3.Distance(position, mass.Position);
            if (dist2 < radius * radius)
            {
                mass.ApplyForce(10 * force * (position - mass.Position) / (100 + dist2));
                mass.IncreaseDamping(0.6f);
            }
        }
    }

    public void ApplyExplosiveForce(float force, Vector3 position, float radius)
    {
        foreach (var mass in points)
        {
            float dist2 = Vector3.Distance(position, mass.Position) * Vector3.Distance(position, mass.Position);
            if (dist2 < radius * radius)
            {
                mass.ApplyForce(100 * force * (mass.Position - position) / (10000 + dist2));
                mass.IncreaseDamping(0.6f);
            }
        }
    }


    public Vector2 ToVec2(Vector3 v)
    {
        // do a perspective projection
        float factor = (v.z + 2000) / 2000;
        return (new Vector2(v.x, v.y) - new Vector2(Screen.width,Screen.height) / 2f) * factor + new Vector2(Screen.width,Screen.height) / 2;
    }

   


}