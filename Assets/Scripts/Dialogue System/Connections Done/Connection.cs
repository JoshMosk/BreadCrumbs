using System;
using UnityEditor;
using UnityEngine;

public class Connection {

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public Action<Connection> OnClickRemoveConnection;

    public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection) {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
        this.OnClickRemoveConnection = OnClickRemoveConnection;
    }

    public void Draw() {

        // Get the default line colour.
        Color lineColor = new Color(91 / 255f, 169 / 255f, 193 / 255f);

        // If the line is coming from a Boolean node, recolour it so that there is a green and red line.
        if (outPoint.node.type == "Boolean") {
            if (outPoint.boolType) lineColor = new Color(65 / 255f, 216 / 255f, 62 / 255f);
            else lineColor = new Color(216 / 255f, 62 / 255f, 73 / 255f);
        }

        // Draw the line.
        Handles.DrawBezier(inPoint.rect.center, outPoint.rect.center, inPoint.rect.center + Vector2.left * 70f, outPoint.rect.center - Vector2.left * 70f, lineColor, null, 4f);

        // Draw the button on the middle of the line that will delete the connection.
        if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleCap)) {
            if (OnClickRemoveConnection != null) OnClickRemoveConnection(this);
        }
    }
}