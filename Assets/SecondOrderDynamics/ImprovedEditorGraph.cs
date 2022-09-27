// heavaly based on https://gist.github.com/thelastpointer/c52c4b1f147dc47961798e39e3a7ea10#comments

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
public class ImprovedEditorGraph {
    #region publicState

    public string Title = "";
    public GraphColors Colors;
    public float xAxisLines = 1f;
    public float yAxisLines = 1f;

    #endregion

    #region internalState

    Rect rect;
    float minX, maxX, minY, maxY;
    float rangeX { get { return maxX - minX; } }
    private float rangeY { get { return maxY - minY; } }

    private List<List<Vector3>> lines = new List<List<Vector3>>();

    #endregion

    public ImprovedEditorGraph(Rect rect/*, float minX, float maxX, float minY, float maxY*/)
    {

        this.rect = rect;
        
        Colors = new GraphColors {
            Background = new Color(0.15f, 0.15f, 0.15f, 1f),
            Outline = new Color(0.15f, 0.15f, 0.15f, 1f),
            GridLine = new Color(0.5f, 0.5f, 0.5f),
            Function = Color.red,
            CustomLine = Color.white
        };
    }

    public void DrawNew(in List<Vector3> points)
    {
        if (Event.current.type != EventType.Repaint)
            return;
        
        SetGraphBounds(in points, out minX, out maxX, out minY, out maxY);
        
        EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f));
        DrawLine(points, Color.red);
        
        DrawAxisLines();
        
    }
    
    void SetGraphBounds(in List<Vector3> points, out float minX, out float maxX, out float minY, out float maxY)
    {
        minX = minY = Single.PositiveInfinity;
        maxX = maxY = Single.NegativeInfinity;
        foreach (var point in points)
        {
            // x axis
            if (point.x < minX)
                minX = point.x;
            if (point.x > maxX)
                maxX = point.x;
            
            // y axis
            if (point.y < minY)
                minY = point.y;
            if (point.y > maxY)
                maxY = point.y;
        }
    }

    void DrawAxisLines()
    {
        List<Vector3> axisPoints = new List<Vector3>(2);
        axisPoints.Add(Vector3.zero);
        axisPoints.Add(Vector3.zero);


        //implements axis and grid
        //------------------------------------------------
        
        //x axis lines
        //positive
        for (float i = 0; i < maxY; i++) {
            axisPoints[0] = new Vector3( minX, i);
            axisPoints[1] = new Vector3(maxX, i);
            DrawLine(axisPoints, Colors.GridLine);
             
            GUI.Label(new Rect(PointToGraph(new Vector3(minX, i)), Vector2.one * 24), i.ToString());
        }

         
        //minus
        for (float i = -1; i > minY; i--) {
            axisPoints[0] = new Vector3( minX, i);
            axisPoints[1] = new Vector3(maxX, i);
            DrawLine(axisPoints, Colors.GridLine);
             
            GUI.Label(new Rect(PointToGraph(new Vector3(minX, i)), Vector2.one * 24), i.ToString());
        }
        
        
        //y axis lines
        //positive
        for (float i = 0; i < maxX; i++) {
            axisPoints[0] = new Vector3( i, minY);
            axisPoints[1] = new Vector3(i, maxY);
            DrawLine(axisPoints, Colors.GridLine);
            
            GUI.Label(new Rect(PointToGraph(new Vector3(i, maxY)), Vector2.one * 24), i.ToString());
        }

        
        //minus
        for (float i = -1; i > minX; i--) {
            axisPoints[0] = new Vector3( i, minY);
            axisPoints[1] = new Vector3(i, maxY);
            DrawLine(axisPoints, Colors.GridLine);
            
            GUI.Label(new Rect(PointToGraph(new Vector3(i, maxY)) , Vector2.one * 24), i.ToString());
        }
    }
    
    /// <summary>
    /// Colors used to draw the graph.
    /// </summary>
    public struct GraphColors {
        /// <summary>
        /// Background color.
        /// </summary>
        public Color Background;

        /// <summary>
        /// Outline color for the graph.
        /// </summary>
        public Color Outline;

        /// <summary>
        /// Helper line color.
        /// </summary>
        public Color GridLine;

        /// <summary>
        /// Default color for custom functions.
        /// </summary>
        public Color Function;

        /// <summary>
        /// Default color for custom lines.
        /// </summary>
        public Color CustomLine;
    }
    
    // old currently not in use

    /*
    /// <summary>
    /// Draw the graph with the specified minimum size.
    /// </summary>
    /// <param name="width">Minimum width of the graph in pixels.</param>
    /// <param name="height">Minimum height of the graph in pixels.</param>
    public void Draw(float width, float height) {
        // Get rect
        if (!string.IsNullOrEmpty(Title)) {
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
                GUILayout.Label(Title);
        }

        using (new GUILayout.HorizontalScope()) {
            GUILayout.Space(EditorGUI.indentLevel * 15f);
            rect = GUILayoutUtility.GetRect(0, 250);
        }

        //eventual click logic here

        // Only continue if we're repainting the graph
        if (Event.current.type != EventType.Repaint)
            return;

        Handles.DrawSolidRectangleWithOutline(rect, Colors.Background, Colors.Outline);

        //eventual helper lines x and y

        


        //draws lines
        //------------------------------------------------

        //helper func
        float[] AxisArray(List<Vector3> list, Axis axis) {
            float[] returnValue = new float[list.Count];
            switch (axis) {
                case Axis.X:
                    for (int i = 0; i < list.Count; i++) {
                        returnValue[i] = list[i].x;
                    }

                    break;
                case Axis.Y:
                    for (int i = 0; i < list.Count; i++) {
                        returnValue[i] = list[i].y;
                    }

                    break;
                case Axis.Z:
                    for (int i = 0; i < list.Count; i++) {
                        returnValue[i] = list[i].z;
                    }

                    break;
                default:
                    for (int i = 0; i < list.Count; i++) {
                        returnValue[i] = list[i].y;
                    }

                    break;
            }

            // foreach (var varr in returnValue) {
            //     Debug.Log(varr);
            //     
            // }
            return returnValue;
        }

        //finds min max values so entire graph is shown 
        foreach (var line in lines) {
            minX = Mathf.Min(minX, Mathf.Min(AxisArray(line, Axis.X)));
            maxX = Mathf.Max(maxX, Mathf.Max(AxisArray(line, Axis.X)));
            minY = Mathf.Min(minY, Mathf.Min(AxisArray(line, Axis.Y)));
            maxY = Mathf.Max(maxY, Mathf.Max(AxisArray(line, Axis.Y)));
        }


        foreach (var line in lines) {
            DrawLine(line, Colors.CustomLine);
        }
        
        List<Vector3> points = new List<Vector3>(2);
        points.Add(Vector3.zero);
        points.Add(Vector3.zero);


        //implements axis and grid
        //------------------------------------------------
        
        //x axis lines
        //positive
        for (float i = 0; i < maxY; i++) {
            points[0] = new Vector3( minX, i);
            points[1] = new Vector3(maxX, i);
            DrawLine(points, Colors.GridLine);
             
            GUI.Label(new Rect(PointToGraph(new Vector3(minX, i)), Vector2.one * 24), i.ToString());
        }

         
        //minus
        for (float i = -1; i > minY; i--) {
            points[0] = new Vector3( minX, i);
            points[1] = new Vector3(maxX, i);
            DrawLine(points, Colors.GridLine);
             
            GUI.Label(new Rect(PointToGraph(new Vector3(minX, i)), Vector2.one * 24), i.ToString());
        }
        
        
        //y axis lines
        //positive
        for (float i = 0; i < maxX; i++) {
            points[0] = new Vector3( i, minY);
            points[1] = new Vector3(i, maxY);
            DrawLine(points, Colors.GridLine);
            
            GUI.Label(new Rect(PointToGraph(new Vector3(i, maxY)), Vector2.one * 24), i.ToString());
        }

        
        //minus
        for (float i = -1; i > minX; i--) {
            points[0] = new Vector3( i, minY);
            points[1] = new Vector3(i, maxY);
            DrawLine(points, Colors.GridLine);
            
            GUI.Label(new Rect(PointToGraph(new Vector3(i, maxY)) , Vector2.one * 24), i.ToString());
        }
        
        
        
        
        
        
    }
    */


    public void AddLine(List<Vector3> _points) {
        lines.Add(_points);
    }

    public void DrawLine(List<Vector3> _points, Color _color) {
        //converting to the correct space
        for (int i = 0; i < _points.Count; i++) {
            _points[i] = PointToGraph(_points[i]);
        }

        Handles.color = _color;
        Handles.DrawAAPolyLine(2.0f, _points.Count, _points.ToArray());
    }

    private Vector3 PointToGraph(Vector3 _point) {
        _point.x = Mathf.Lerp(rect.xMin, rect.xMax,
            (_point.x - minX) / (rangeX)); // the t in this case is the basic lerp function solved for t
        _point.y = Mathf.Lerp(rect.yMax, rect.yMin,
            (_point.y - minY) / (rangeY)); // --||-- exept we invert the points so its right side up
        return new Vector3(_point.x, _point.y, 0f);
    }

}