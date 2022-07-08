// heavaly based on https://gist.github.com/thelastpointer/c52c4b1f147dc47961798e39e3a7ea10#comments

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ImprovedEditorGraph {
    #region publicState

    public string Title = "";
    public GraphColors Colors;
    public float yAxis = 1f;
    public float xAxis = 1f;

    #endregion

    #region internalState

    float minX, maxX, minY, maxY;
    Rect rect;
    float rangeX = 10;
    float rangeY = 10;

    private List<List<Vector3>> lines = new List<List<Vector3>>(); 

    #endregion

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="_minX">Minimum X value in graph units.</param>
    /// <param name="_minY">Minimum Y value in graph units.</param>
    /// <param name="_maxX">Maximum X value in graph units.</param>
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

        rangeX = maxX - minX;
        rangeY = maxY - minY;

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

        List<Vector3> points = new List<Vector3>(2);
        points.Add(Vector3.zero);
        points.Add(Vector3.zero);
        
        
        //implements axis
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
        _point.x = Mathf.Lerp(rect.xMin, rect.xMax, (_point.x - minX) / (rangeX)); // the t in this case is the basic lerp function solved for t
        _point.y = Mathf.Lerp(rect.yMax, rect.yMin, (_point.y - minY) / (rangeY)); // --||-- exept we invert the points so its right side up
        return new Vector3(_point.x, _point.y, 0f);
    }
}