using Locomotion.AI;
using UnityEditor;
using UnityEngine;
using Utils.MyOffMeshLink;

namespace Editor
{
    [CustomEditor(typeof(OffMeshLinkController))]
    public class OffMeshLinkControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var offMeshLinkController = (OffMeshLinkController) target;
            
            if (GUILayout.Button("Recenter controller"))
            {
                offMeshLinkController.RecenterControllerPosition();
            }

            if (GUILayout.Button("Align start/end in X"))
            {
                offMeshLinkController.AlignStartEndPositionInX();
            }
            
            if (GUILayout.Button("Align start/end in Y"))
            {
                offMeshLinkController.AlignStartEndPositionInY();
            }
            
            if (GUILayout.Button("Align start/end in Z"))
            {
                offMeshLinkController.AlignStartEndPositionInZ();
            }
        }
    }
}
