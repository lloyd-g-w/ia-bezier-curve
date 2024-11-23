using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Beziercurve
{
    public static List<Vector2> uneditedPoints = new List<Vector2>();


    public static float[] Factorial = new float[] //Look up table for Factorials up to 16
    {
        1.0f,
        1.0f,
        2.0f,
        6.0f,
        24.0f,
        120.0f,
        720.0f,
        5040.0f,
        40320.0f,
        362880.0f,
        3628800.0f,
        39916800.0f,
        479001600.0f,
        6227020800.0f,
        87178291200.0f,
        1307674368000.0f,
        20922789888000.0f,
    };

    public static float Binomial(int n, int i)
    {
        return Factorial[n] / (Factorial[i] * Factorial[n - i]); //Binomial Coefficient
    }

    public static float Bernstein(int n, int i, float t)
    {
        return Binomial(n, i) * Mathf.Pow(t, i) * Mathf.Pow(1 - t, n - i); //Bernstein Polynomial
    }


    public static float BernsteinDerivative(int n, int i, float t) //Hard Coded Derivatives
    {
        if (n == 0)
        {
            return 1;
        }
        else if (n == 1)
        {
            if (i == 0) return -1;
            else if (i == 1) return 1;
        }
        else if (n == 2)
        {
            if (i == 0) return -2 + 2 * t;
            else if (i == 1) return 2 - 4 * t;
            else if (i == 2) return 2 * t;
        }
        else if (n == 3)
        {
            if (i == 0) return -3 * Mathf.Pow(t, 2) + 6 * t - 3;
            else if (i == 1) return 9 * Mathf.Pow(t, 2) - 12 * t + 3;
            else if (i == 2) return -9 * Mathf.Pow(t, 2) + 6 * t;
            else if (i == 3) return 3 * Mathf.Pow(t, 2);

        }
        else if (n == 4)
        {
            if (i == 0) return -4 + 12 * t - 12 * Mathf.Pow(t, 2) + 4 * Mathf.Pow(t, 3);
            else if (i == 1) return 4 - 24 * t + 36 * Mathf.Pow(t, 2) - 16 * Mathf.Pow(t, 3);
            else if (i == 2) return 12 * t - 36 * Mathf.Pow(t, 2) + 24 * Mathf.Pow(t, 3);
            else if (i == 3) return 12 * Mathf.Pow(t, 2) - 16 * Mathf.Pow(t, 3);
            else if (i == 4) return 4 * Mathf.Pow(t, 3);
        }
        else if (n == 5)
        {
            if (i == 0) return -5 + 20 * t - 30 * Mathf.Pow(t, 2) + 20 * Mathf.Pow(t, 3) - 5 * Mathf.Pow(t, 4);
            else if (i == 1) return 5 - 40 * t + 90 * Mathf.Pow(t, 2) - 80 * Mathf.Pow(t, 3) + 25 * Mathf.Pow(t, 4);
            else if (i == 2) return 20 * t - 90 * Mathf.Pow(t, 2) + 120 * Mathf.Pow(t, 3) - 50 * Mathf.Pow(t, 4);
            else if (i == 3) return 30 * Mathf.Pow(t, 2) - 80 * Mathf.Pow(t, 3) + 50 * Mathf.Pow(t, 4);
            else if (i == 4) return 20 * Mathf.Pow(t, 3) - 25 * Mathf.Pow(t, 4);
            else if (i == 5) return 5 * Mathf.Pow(t, 4);
        }
        else if (n == 6)
        {
            if (i == 0) return -6 + 30 * t - 60 * Mathf.Pow(t, 2) + 60 * Mathf.Pow(t, 3) - 30 * Mathf.Pow(t, 4) + 6 * Mathf.Pow(t, 5);
            else if (i == 1) return 6 - 60 * t + 180 * Mathf.Pow(t, 2) - 240 * Mathf.Pow(t, 3) + 150 * Mathf.Pow(t, 4) - 36 * Mathf.Pow(t, 5);
            else if (i == 2) return 30 * t - 180 * Mathf.Pow(t, 2) + 360 * Mathf.Pow(t, 3) - 300 * Mathf.Pow(t, 4) + 90 * Mathf.Pow(t, 5);
            else if (i == 3) return 60 * Mathf.Pow(t, 2) - 240 * Mathf.Pow(t, 3) + 300 * Mathf.Pow(t, 4) - 120 * Mathf.Pow(t, 5);
            else if (i == 4) return 60 * Mathf.Pow(t, 3) - 150 * Mathf.Pow(t, 4) + 90 * Mathf.Pow(t, 5);
            else if (i == 5) return 30 * Mathf.Pow(t, 4) - 36 * Mathf.Pow(t, 5);
            else if (i == 6) return 6 * Mathf.Pow(t, 5);
        }
        return -1;
    }

    public static float BernsteinSecondDerivative(int n, int i, float t) //Hard Coded Derivatives
    {
        if (n == 0)
        {
            return 0;
        }
        else if (n == 1)
        {
            if (i == 0) return 0;
            else if (i == 1) return 0;
        }
        else if (n == 2)
        {
            if (i == 0) return 2;
            else if (i == 1) return 4;
            else if (i == 2) return 2;
        }
        else if (n == 3)
        {
            if (i == 0) return -3 * 2 * Mathf.Pow(t, 1) + 6;
            else if (i == 1) return 9 * 2 * Mathf.Pow(t, 1) - 12;
            else if (i == 2) return -9 * 2 * Mathf.Pow(t, 1) + 6;
            else if (i == 3) return 3 * 2 * Mathf.Pow(t, 1);

        }
        else if (n == 4)
        {
            if (i == 0) return 12 - 12 * 2 * Mathf.Pow(t, 1) + 4 * 3 * Mathf.Pow(t, 2);
            else if (i == 1) return -24 + 36 * 2 * Mathf.Pow(t, 1) - 16 * 3 * Mathf.Pow(t, 2);
            else if (i == 2) return 12 - 36 * 2 * Mathf.Pow(t, 1) + 24 * 3 * Mathf.Pow(t, 2);
            else if (i == 3) return 12 * 2 * Mathf.Pow(t, 1) - 16 * 3 * Mathf.Pow(t, 2);
            else if (i == 4) return 4 * 3 * Mathf.Pow(t, 2);
        }
        else if (n == 5)
        {
            if (i == 0) return 20 - 30 * 2 * Mathf.Pow(t, 1) + 20 * 3 * Mathf.Pow(t, 2) - 5 * 4 * Mathf.Pow(t, 3);
            else if (i == 1) return -40 + 90 * 2 * Mathf.Pow(t, 1) - 80 * 3 * Mathf.Pow(t, 2) + 25 * 4 * Mathf.Pow(t, 3);
            else if (i == 2) return 20 - 90 * 2 * Mathf.Pow(t, 1) + 120 * 3 * Mathf.Pow(t, 2) - 50 * 4 * Mathf.Pow(t, 3);
            else if (i == 3) return 30 * 2 * Mathf.Pow(t, 1) - 80 * 3 * Mathf.Pow(t, 2) + 50 * 4 * Mathf.Pow(t, 3);
            else if (i == 4) return 20 * 3 * Mathf.Pow(t, 2) - 25 * 4 * Mathf.Pow(t, 3);
            else if (i == 5) return 5 * 4 * Mathf.Pow(t, 3);
        }
        else if (n == 6)
        {
            if (i == 0) return 30 - 60 * 2 * Mathf.Pow(t, 1) + 60 * 3 * Mathf.Pow(t, 2) - 30 * 4 * Mathf.Pow(t, 3) + 6 * 5 * Mathf.Pow(t, 4);
            else if (i == 1) return -60 + 180 * 2 * Mathf.Pow(t, 1) - 240 * 3 * Mathf.Pow(t, 2) + 150 * 4 * Mathf.Pow(t, 3) - 36 * 5 * Mathf.Pow(t, 4);
            else if (i == 2) return 30 - 180 * 2 * Mathf.Pow(t, 1) + 360 * 3 * Mathf.Pow(t, 2) - 300 * 4 * Mathf.Pow(t, 3) + 90 * 4 * Mathf.Pow(t, 4);
            else if (i == 3) return 60 * 2 * Mathf.Pow(t, 1) - 240 * 3 * Mathf.Pow(t, 2) + 300 * 4 * Mathf.Pow(t, 3) - 120 * 5 * Mathf.Pow(t, 4);
            else if (i == 4) return 60 * 2 * Mathf.Pow(t, 2) - 150 * 3 * Mathf.Pow(t, 3) + 90 * 5 * Mathf.Pow(t, 4);
            else if (i == 5) return 30 * 4 * Mathf.Pow(t, 3) - 36 * 5 * Mathf.Pow(t, 4);
            else if (i == 6) return 30 * Mathf.Pow(t, 4);
        }
        return -1;
    }

    public static List<Vector3> PointList3(
      List<Vector3> controlPoints,
      float interval = 0.01f)
    {
        int N = controlPoints.Count - 1;
        if (N > 16)
        {
            Debug.Log("You have used more than 16 control points. " +
              "The maximum control points allowed is 16.");
            controlPoints.RemoveRange(16, controlPoints.Count - 16);
        }

        List<Vector3> points = new List<Vector3>();
        for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t += interval)
        {
            Vector3 p = new Vector3();
            for (int i = 0; i < controlPoints.Count; ++i)
            {
                Vector3 bn = Bernstein(N, i, t) * controlPoints[i];
                p += bn;
            }
            points.Add(p);
        }

        return points;
    }


    public static List<Vector2> PointList2(
      List<Vector2> controlPoints,
      float interval = 0.01f)
    {
        int N = controlPoints.Count - 1;
        if (N > 16)
        {
            Debug.Log("You have used more than 16 control points. " +
              "The maximum control points allowed is 16.");
            controlPoints.RemoveRange(16, controlPoints.Count - 16);
        }

        List<Vector2> points = new List<Vector2>();
        for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t += interval)
        {
            Vector2 p = new Vector2();
            for (int i = 0; i < controlPoints.Count; ++i)
            {
                Vector2 bn = Bernstein(N, i, t) * controlPoints[i];
                p += bn;
            }
            points.Add(p);
        }

        return points;
    }


    public static List<Vector2> PointList2FirstDerivative(
      List<Vector2> controlPoints,
      float interval = 0.01f)
    {

        int zoom = Navigation.zoom1;
        int N = controlPoints.Count - 1; //Degree of curve

        List<Vector2> points = new List<Vector2>();
        List<Vector2> preUneditedpoints = new List<Vector2>();
        for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t += interval)
        {
            Vector2 p = new Vector2();
            for (int i = 0; i < controlPoints.Count; ++i)
            {
                    Vector2 bn = BernsteinDerivative(N, i, t) * controlPoints[i];
                    p += bn;   
            }

            preUneditedpoints.Add(p);
            Vector2 adjustedPoint = new Vector2(); //HERE WE ARE ADJUSTING THE POSITION OF THE POINT TO FIT IN THE BOX ON THE SCREEN
            adjustedPoint.x = p.x / 100*zoom - 1407.269f;
            adjustedPoint.y = p.y / 100*zoom + 1900/3;
            points.Add(adjustedPoint);
        }
        uneditedPoints = preUneditedpoints;
        return points;
    }

    public static List<Vector2> PointList2SecondDerivative(
      List<Vector2> controlPoints,
      float interval = 0.01f)
    {
        int zoom = Navigation.zoom2;

        int N = controlPoints.Count - 1; //Degree of curve

        List<Vector2> points = new List<Vector2>();
        for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t += interval)
        {
            Vector2 p = new Vector2();
            for (int i = 0; i < controlPoints.Count; ++i)
            {
                Vector2 bn = BernsteinSecondDerivative(N, i, t) * controlPoints[i];
                p += bn;
            }

            Vector2 adjustedPoint = new Vector2(); //HERE WE ARE ADJUSTING THE POSITION OF THE POINT TO FIT IN THE BOX ON THE SCREEN
            adjustedPoint.x = p.x / 250 * zoom - 1407.269f;
            adjustedPoint.y = p.y / 250 * zoom - 1900 / 3;
            points.Add(adjustedPoint);
        }

        return points;
    }


    void start() { }
    void update() { }
}