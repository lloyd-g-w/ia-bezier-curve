using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class Bezier_Splines_Viz : MonoBehaviour
{
    //NOTE ABOUT index.html
    // WIDTH = 80% (and Style Width)
    // heigh = 45%  
    // width=80% height=45% style="width: 80%; height: 45%

    public bool Mirror;
    private int ControlPointsQuantity;
    private int SplineCount;
    public List<Tuple<LineRenderer[], List<GameObject>>> Outputs = new List<Tuple<LineRenderer[], List<GameObject>>>(); //These are the gameobjects and linerenders reference objects
    public List<LineRenderer> FirstOutputs = new List<LineRenderer>();
    public List<LineRenderer> SecondOutputs = new List<LineRenderer>();
    public GameObject PointPrefab; 
    public UnityEngine.Color LineColour;
    public float LineWidth;
    public UnityEngine.Color BeziercurveColour;
    public List<Vector2> Bezierpts;
    System.Random rand = new System.Random();
    public List<List<Vector2>> AllBezierDerivativepts;



    private LineRenderer CreateLine()
    {
        GameObject obj = new GameObject();
        LineRenderer lr = obj.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = LineColour;
        lr.endColor = LineColour;
        lr.startWidth = LineWidth;
        lr.endWidth = LineWidth;
        return lr;
    }



    public Tuple<LineRenderer[], List<GameObject>> InitalRenderer(string lineName, string BezierName, int j)
    {
        List<Vector3> ControlPoints = new List<Vector3>();
        for (int g = 0; g < ControlPointsQuantity; g++)
        {
            ControlPoints.Add(new Vector3(rand.Next(-950, 950), rand.Next(-950, 950), 0));
        }
        LineRenderer[] mLineRenderers = new LineRenderer[2];
        List<GameObject> mPointGameObjects = new List<GameObject>();
        // Create the two LineRenderers.
        mLineRenderers[0] = CreateLine(); //Straight lines
        mLineRenderers[1] = CreateLine(); //Bezier Curve

        // set a name to the game objects for the LineRenderers
        // to distingush them.
        mLineRenderers[0].gameObject.name = lineName;
        mLineRenderers[1].gameObject.name = BezierName;

        // Create the instances of PointPrefab
        // to show the control points.


        for (int i = 0; i < ControlPoints.Count; ++i)
        {
            GameObject obj = Instantiate(PointPrefab, ControlPoints[i], Quaternion.identity);
            obj.name = "ControlPoint_" + j.ToString() + "_" + i.ToString();
            mPointGameObjects.Add(obj);
        }

        return Tuple.Create(mLineRenderers, mPointGameObjects); //This is the same as "new Tuple<LineRenderer[], List<GameObject>>(mLineRenderers, mPointGameObjects)";
    }


    public Tuple<LineRenderer[], List<GameObject>> InitalRendererAfter(string lineName, string BezierName, int j)
    {
        List<Vector3> ControlPoints = new List<Vector3>();
        for (int g = 0; g < ControlPointsQuantity; g++)
        {
            ControlPoints.Add(new Vector3(rand.Next(-950, 950), rand.Next(-950, 950), 0));
        }

        LineRenderer[] mLineRenderers = new LineRenderer[2];
        List<GameObject> mPointGameObjects = new List<GameObject>();
        // Create the two LineRenderers.
        mLineRenderers[0] = CreateLine(); //Straight lines
        mLineRenderers[1] = CreateLine(); //Bezier Curve

        // set a name to the game objects for the LineRenderers
        // to distingush them.
        mLineRenderers[0].gameObject.name = lineName;
        mLineRenderers[1].gameObject.name = BezierName;

        // Create the instances of PointPrefab
        // to show the control points.


        for (int i = 0; i < ControlPoints.Count-1; ++i)
        {
            GameObject obj = Instantiate(PointPrefab, ControlPoints[i], Quaternion.identity);
            obj.name = "ControlPoint_" + j.ToString() + "_" + i.ToString();
            mPointGameObjects.Add(obj);
        }

        return Tuple.Create(mLineRenderers, mPointGameObjects); //This is the same as "new Tuple<LineRenderer[], List<GameObject>>(mLineRenderers, mPointGameObjects)";
    }

    public void RendererUpdate(LineRenderer[] mLineRenderers, List<GameObject> mPointGameObjects)
    {
        LineRenderer lineRenderer = mLineRenderers[0];
        LineRenderer curveRenderer = mLineRenderers[1];

        List<Vector2> pts = new List<Vector2>();
        

        for (int k = 0; k < mPointGameObjects.Count; ++k)
        {
            pts.Add(mPointGameObjects[k].transform.position);
        }

        // create a line renderer for showing the straight
        //lines between control points.
        lineRenderer.positionCount = pts.Count;
        for (int i = 0; i < pts.Count; ++i)
        {
            lineRenderer.SetPosition(i, pts[i]);
        }

        // we take the control points from the list of points in the scene.
        // recalculate points every frame.
        List<Vector2> curve = Beziercurve.PointList2(pts, 0.01f);
        foreach (Vector2 pos in curve){
            Bezierpts.Add(pos);
        }
        curveRenderer.startColor = BeziercurveColour;
        curveRenderer.endColor = BeziercurveColour;
        curveRenderer.positionCount = curve.Count;
        for (int i = 0; i < curve.Count; ++i)
        {
            curveRenderer.SetPosition(i, curve[i]);
        }
    }

    public void RendererUpdateAfter(LineRenderer[] mLineRenderers, List<GameObject> mPointGameObjects, GameObject Point)
    {

        List<GameObject> mPointGameObjectsFinal = new List<GameObject>
        {
            Point
        };
        foreach (GameObject mPointGameObject in mPointGameObjects)
        {
            mPointGameObjectsFinal.Add(mPointGameObject);
        }

        LineRenderer lineRenderer = mLineRenderers[0];
        LineRenderer curveRenderer = mLineRenderers[1];

        List<Vector2> pts = new List<Vector2>();
        
        for (int k = 0; k < mPointGameObjectsFinal.Count; ++k)
        {
            pts.Add(mPointGameObjectsFinal[k].transform.position);
        }


        // create a line renderer for showing the straight
        //lines between control points.
        lineRenderer.positionCount = pts.Count;
        for (int i = 0; i < pts.Count; ++i)
        {
            lineRenderer.SetPosition(i, pts[i]);
        }

        // we take the control points from the list of points in the scene.
        // recalculate points every frame.
        List<Vector2> curve = Beziercurve.PointList2(pts, 0.01f);
        foreach (Vector2 pos in curve.Skip(1))
        {
            Bezierpts.Add(pos);
        }
        curveRenderer.startColor = BeziercurveColour;
        curveRenderer.endColor = BeziercurveColour;
        curveRenderer.positionCount = curve.Count;
        for (int i = 0; i < curve.Count; ++i)
        {
            curveRenderer.SetPosition(i, curve[i]);
        }
    }



    public LineRenderer RenderDerivative(string BezierName)
    {
        GameObject obj = new GameObject();
        LineRenderer lr = obj.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startWidth = LineWidth/2;
        lr.endWidth = LineWidth/2;
        lr.gameObject.name = BezierName;
        return lr;
    }


    public void updateDerivative(LineRenderer mLineRenderer, List<GameObject> controlPoints, bool first)
    {
        List<Vector2> pts = new List<Vector2>();
        foreach (GameObject controlPoint in controlPoints)
        {
            pts.Add(controlPoint.transform.position);
        }

        // we take the control points from the list of points in the scene.
        // recalculate points every frame.
        if (first == true)
        {
            List<Vector2> curve = Beziercurve.PointList2FirstDerivative(pts, 0.01f);
            mLineRenderer.startColor = BeziercurveColour;
            mLineRenderer.endColor = BeziercurveColour;
            mLineRenderer.positionCount = curve.Count;
            for (int i = 0; i < curve.Count; ++i)
            {
                mLineRenderer.SetPosition(i, curve[i]);
            }
        }
        else
        {
            List<Vector2> curve = Beziercurve.PointList2SecondDerivative(pts, 0.01f);
            mLineRenderer.startColor = BeziercurveColour;
            mLineRenderer.endColor = BeziercurveColour;
            mLineRenderer.positionCount = curve.Count;
            for (int i = 0; i < curve.Count; ++i)
            {
                mLineRenderer.SetPosition(i, curve[i]);
            }
        }
    }
        





    // Start is called before the first frame update
    void Start()
    {
        SplineCount = Navigation.SplineCount;
        ControlPointsQuantity = Navigation.ControlPointsQuantity;
        Mirror = Navigation.Mirror;
        
        for (int i = 0; i < SplineCount; i++)
        {
            string lineName = "Line_" + i.ToString();
            string BezierName = "Bezier_" + i.ToString();
            if (i == 0)
            {
                Tuple<LineRenderer[], List<GameObject>> Output = InitalRenderer(lineName, BezierName, i);
                Outputs.Add(Output);
            }
            else
            {
                Tuple<LineRenderer[], List<GameObject>> Output = InitalRendererAfter(lineName, BezierName, i);
                Outputs.Add(Output);
            }

            FirstOutputs.Add(RenderDerivative("FirstDerivative_" + i.ToString())); //Initialise Derivative Line
            SecondOutputs.Add(RenderDerivative("SecondDerivative_" + i.ToString())); //Initialise 2nd Derivative Line
            AllBezierDerivativepts = new List<List<Vector2>>();
        }

        //Bounding box 
            
            GameObject obj1 = new GameObject();
            LineRenderer topside = obj1.AddComponent<LineRenderer>();
            obj1.name = "topside";
            GameObject obj2 = new GameObject();
            LineRenderer bottomside = obj2.AddComponent<LineRenderer>();
            obj2.name = "bottomside";
            GameObject obj3 = new GameObject();
            LineRenderer leftside = obj3.AddComponent<LineRenderer>();
            obj3.name = "leftside";
            GameObject obj4 = new GameObject();
            LineRenderer rightside = obj4.AddComponent<LineRenderer>();
            obj4.name = "rightside";

            List<LineRenderer> sides = new List<LineRenderer> {topside, bottomside, leftside, rightside };
            
            foreach (LineRenderer side in sides)
            {
                side.material = new Material(Shader.Find("Sprites/Default"));
                side.startColor = UnityEngine.Color.green;
                side.endColor = UnityEngine.Color.green;
                side.startWidth = 5;
                side.endWidth = 5;
                side.positionCount = 2;
            }


        

    }

    // Update is called once per frame
    void Update()
    {
        Bezierpts = new List<Vector2>();
        for (int i = 0; i < SplineCount; i++)
        {
            LineRenderer[] mLineRenderer = Outputs[i].Item1;
            

            if (Mirror == true && SplineCount > 1)
            {
                if (i == 0) //First spline requires an extra point then every other one
                {

                    RendererUpdate(mLineRenderer, Outputs[i].Item2);

                }
                else
                {
                    List<GameObject> mPointGameObjects = new List<GameObject>();
                    for (int k = 0; k < Outputs[i].Item2.Count; k++)
                    {
                        if (k == 0)
                        {

                            var correspondingPoint = Outputs[i - 1].Item2[Outputs[i - 1].Item2.Count - 2].transform.position;
                            var pointBetween = Outputs[i - 1].Item2[Outputs[i - 1].Item2.Count - 1].transform.position;


                            var diff = pointBetween - correspondingPoint;
                            

                            Outputs[i].Item2[k].transform.position = pointBetween + diff;
                            mPointGameObjects.Add(Outputs[i].Item2[k]);

                        }
                        else
                        {
                            mPointGameObjects.Add(Outputs[i].Item2[k]);
                        }

                    }
                    RendererUpdateAfter(mLineRenderer, mPointGameObjects, Outputs[i - 1].Item2.Last());
                }
            }

            else
            {
                List<GameObject> mPointGameObjects = Outputs[i].Item2;
                if (i == 0)
                {
                    RendererUpdate(mLineRenderer, mPointGameObjects); 
                }
                else
                {
                    GameObject point = Outputs[i - 1].Item2.Last();
                    RendererUpdateAfter(mLineRenderer, mPointGameObjects, point);
                }
            }

            

            //Derivatives
            if (i==0)
            {
                //First
                List<GameObject> origControlPoints = Outputs[i].Item2;
                LineRenderer firstDevcurve = FirstOutputs[i];
                updateDerivative(firstDevcurve, origControlPoints, true);
                
                //Second
                LineRenderer SecondDevcurve = SecondOutputs[i];
                updateDerivative(SecondDevcurve, origControlPoints, false);
            }
            else
            {
                //First
                List<GameObject> origControlPoints = new List<GameObject>(Outputs[i].Item2);
                origControlPoints.Insert(0, Outputs[i - 1].Item2.Last());
                LineRenderer firstDevcurve = FirstOutputs[i];
                updateDerivative(firstDevcurve, origControlPoints, true);

                //Second
                LineRenderer SecondDevcurve = SecondOutputs[i];
                updateDerivative(SecondDevcurve, origControlPoints, false);
            }

            AllBezierDerivativepts.Add(Beziercurve.uneditedPoints);

        }

        List<Vector2> BezierDerivativepts = new List<Vector2>();
        foreach (List<Vector2> list in AllBezierDerivativepts)
        {
            foreach (Vector2 vector in list)
            {
                BezierDerivativepts.Add(vector);
            }
        }


        //Boundary Box
        LineRenderer topside = GameObject.Find("topside").GetComponent<LineRenderer>();
        LineRenderer bottomside = GameObject.Find("bottomside").GetComponent<LineRenderer>();
        LineRenderer leftside = GameObject.Find("leftside").GetComponent<LineRenderer>();
        LineRenderer rightside = GameObject.Find("rightside").GetComponent<LineRenderer>();


        List<Vector2> points = BoundaryBox.extremities(BezierDerivativepts, Bezierpts);
        topside.SetPosition(0, points[0]);
        topside.SetPosition(1, points[1]);
        bottomside.SetPosition(0, points[2]);
        bottomside.SetPosition(1, points[3]);
        leftside.SetPosition(0, points[0]);
        leftside.SetPosition(1, points[2]);
        rightside.SetPosition(0, points[1]);
        rightside.SetPosition(1, points[3]);

        AllBezierDerivativepts = new List<List<Vector2>>();
    }
}
