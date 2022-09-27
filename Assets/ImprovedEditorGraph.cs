// heavaly based on https://gist.github.com/thelastpointer/c52c4b1f147dc47961798e39e3a7ea10#comments

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

/*
    USAGE:
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            ImprovedEditorGraph graph = new ImprovedEditorGraph(0f, -1, 1f, 1f, "RopeBehaviourViz");
			graph.AddLine(results);		// results is a List<Vector3> where x and y are mapped to the grapf
			graph.Draw(50, 300); // draws

        }
    
    MORE COMPLICATED USAGE:
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ImprovedEditorGraph graph = new ImprovedEditorGraph(0f, -1, 1f, 1f, "RopeBehaviourViz");

            // Edit some colors...
            graph.Colors.Background = new Color(0.2f, 0.2f, 0.2f, 1f);
            
            //add more lines
            graph.AddLine(results1);
            graph.AddLine(results2);
            ...
            
            graph.Draw();
        }
*/
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

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="_minX">Minimum X value in graph units.</param>
    /// <param name="_minY">Minimum Y value in graph units.</param>
    /// <param name="_maxX">Minimum X value in graph units.</param>
    /// <param name="_maxY">Maximum Y value in graph units.</param>
    /// <param name="_title">Title of the graph (optional).</param>
    /// <param name="_title">Resolution of the graphs (how many points are evaluated for each custom function).</param>
    public ImprovedEditorGraph(float _minX, float _minY, float _maxX, float _maxY, string _title = "") {
        if (_minX >= _maxX)
            throw new System.ArgumentException("Editor graph: minimum X value must be greater than maximum!", "_minX");
        if (_minY >= _maxY)
            throw new System.ArgumentException("Editor graph: minimum Y value must be greater than maximum!", "_minY");

        minX = _minX;
        maxX = _maxX;
        minY = _minY;
        maxY = _maxY;

        Title = _title;

        // Default graph colors
        Colors = new GraphColors {
            Background = new Color(0.15f, 0.15f, 0.15f, 1f),
            Outline = new Color(0.15f, 0.15f, 0.15f, 1f),
            GridLine = new Color(0.5f, 0.5f, 0.5f),
            Function = Color.red,
            CustomLine = Color.white
        };
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

    /// <summary>
    /// Draw the graph with the default size (128x80).
    /// </summary> 
    public void Draw() {
        Draw(128f, 200f);
    }

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