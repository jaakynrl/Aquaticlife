using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ropeEdit : Rope
{
    public Transform startPosition;
    public void create_rope(Vector3 mousePosition)
    {
        nodes[0] = startPosition.position;
        nodes[1] = mousePosition;
        UpdateRope(this);
    }

    void UpdateRope(Rope rope)
    {
       
        if (rope.SegmentsPrefabs == null || rope.SegmentsPrefabs.Length == 0)
        {
            Debug.LogWarning("Rope Segments Prefabs is Empty");
            return;
        }
        float segmentHeight = rope.SegmentsPrefabs[0].bounds.size.y * (1 + rope.overlapFactor);
        List<Vector3> nodes = rope.nodes;
        int currentSegPrefIndex = 0;
        Rigidbody2D previousSegment = null;
        float previousTheta = 0;
        int currentSegment = 0;
        for (int i = 0; i < nodes.Count - 1; i++)
        {
            //construct line between nodes[i] and nodes[i+1]
            float theta = Mathf.Atan2(nodes[i + 1].y - nodes[i].y, nodes[i + 1].x - nodes[i].x);
            float dx = segmentHeight * Mathf.Cos(theta);
            float dy = segmentHeight * Mathf.Sin(theta);
            float startX = nodes[i].x + dx / 2;
            float startY = nodes[i].y + dy / 2;
            float lineLength = Vector2.Distance(nodes[i + 1], nodes[i]);
            int segmentCount = 0;
            switch (rope.OverflowMode)
            {
                case LineOverflowMode.Round:
                    segmentCount = Mathf.RoundToInt(lineLength / segmentHeight);
                    break;
                case LineOverflowMode.Shrink:
                    segmentCount = (int)(lineLength / segmentHeight);
                    break;
                case LineOverflowMode.Extend:
                    segmentCount = Mathf.CeilToInt(lineLength / segmentHeight);
                    break;
            }
            for (int j = 0; j < segmentCount; j++)
            {
                if (rope.SegmentsMode == SegmentSelectionMode.RoundRobin)
                {
                    currentSegPrefIndex++;
                    currentSegPrefIndex %= rope.SegmentsPrefabs.Length;
                }
                else if (rope.SegmentsMode == SegmentSelectionMode.Random)
                {
                    currentSegPrefIndex = Random.Range(0, rope.SegmentsPrefabs.Length);
                }
                GameObject segment = (Instantiate(rope.SegmentsPrefabs[currentSegPrefIndex]) as SpriteRenderer).gameObject;
                segment.name = "Segment_" + currentSegment;
                segment.transform.parent = rope.transform;
                segment.transform.localPosition = new Vector3(startX + dx * j, startY + dy * j);
                segment.transform.localRotation = Quaternion.Euler(0, 0, theta * Mathf.Rad2Deg - 90);
                if (rope.EnablePhysics)
                {
                    Rigidbody2D segRigidbody = segment.GetComponent<Rigidbody2D>();
                    if (segRigidbody == null)
                        segRigidbody = segment.AddComponent<Rigidbody2D>();
                    if (j == 0)
                    {
                        segRigidbody.freezeRotation = true;
                        segRigidbody.gravityScale = 0;
                    }
                    //if not the first segment, make a joint
                    if (currentSegment != 0)
                    {
                        float dtheta = 0;
                        if (j == 0)
                        {
                            //first segment in the line
                            dtheta = (theta - previousTheta) * Mathf.Rad2Deg;
                            if (dtheta > 180) dtheta -= 360;
                            else if (dtheta < -180) dtheta += 360;
                        }
                        //add Hinge
                        float yScale = rope.SegmentsPrefabs[0].transform.localScale.y;
                        AddJoint(rope, dtheta, segmentHeight / yScale, previousSegment, segment);
                    }
                    previousSegment = segRigidbody;
                }
                currentSegment++;
            }
            previousTheta = theta;
        }
        UpdateEndsJoints(rope);
    }

    private static void AddJoint(Rope rope, float dtheta, float segmentHeight, Rigidbody2D previousSegment, GameObject segment)
    {
        HingeJoint2D joint = segment.AddComponent<HingeJoint2D>();
        joint.connectedBody = previousSegment;
        joint.anchor = new Vector2(0, -segmentHeight / 2);
        joint.connectedAnchor = new Vector2(0, segmentHeight / 2);
        if (rope.useBendLimit)
        {
            joint.useLimits = true;
            joint.limits = new JointAngleLimits2D()
            {
                min = dtheta - rope.bendLimit,
                max = dtheta + rope.bendLimit
            };
        }

    }


    private void UpdateEndsJoints(Rope rope)
    {
        Transform firstSegment = rope.transform.GetChild(0);
        if (rope.EnablePhysics &&
            rope.HangFirstSegment &&
            rope.transform.childCount > 0)
        {

            HingeJoint2D joint = firstSegment.gameObject.GetComponent<HingeJoint2D>();
            if (!joint)
                joint = firstSegment.gameObject.AddComponent<HingeJoint2D>();
            Vector2 hingePositionInWorldSpace = rope.transform.TransformPoint(rope.FirstSegmentConnectionAnchor);
            joint.connectedAnchor = hingePositionInWorldSpace;
            joint.anchor = firstSegment.transform.InverseTransformPoint(hingePositionInWorldSpace);
            joint.connectedBody = GetConnectedObject(hingePositionInWorldSpace, firstSegment.GetComponent<Rigidbody2D>());
            if (joint.connectedBody)
            {
                joint.connectedAnchor = joint.connectedBody.transform.InverseTransformPoint(hingePositionInWorldSpace);
            }
        }
        else
        {
            HingeJoint2D joint = firstSegment.gameObject.GetComponent<HingeJoint2D>();
            if (joint) DestroyImmediate(joint);
        }
        Transform lastSegment = rope.transform.GetChild(rope.transform.childCount - 1);
        if (rope.EnablePhysics && rope.HangLastSegment)
        {
            HingeJoint2D[] joints = lastSegment.gameObject.GetComponents<HingeJoint2D>();
            HingeJoint2D joint = null;
            if (joints.Length > 1)
                joint = joints[1];
            else
                joint = lastSegment.gameObject.AddComponent<HingeJoint2D>();
            Vector2 hingePositionInWorldSpace = rope.transform.TransformPoint(rope.LastSegmentConnectionAnchor);
            joint.connectedAnchor = hingePositionInWorldSpace;
            joint.anchor = lastSegment.transform.InverseTransformPoint(hingePositionInWorldSpace);
            joint.connectedBody = GetConnectedObject(hingePositionInWorldSpace, lastSegment.GetComponent<Rigidbody2D>());
            lastSegment.transform.localScale = Vector2.one * 2;
            lastSegment.GetComponent<Rigidbody2D>().freezeRotation = true;
            if (joint.connectedBody)
            {
                joint.connectedAnchor = joint.connectedBody.transform.InverseTransformPoint(hingePositionInWorldSpace);
            }
        }
        else
        {
            HingeJoint2D[] joints = lastSegment.gameObject.GetComponents<HingeJoint2D>();
            if (joints.Length > 1)
                for (int i = 1; i < joints.Length; i++)
                    DestroyImmediate(joints[i]);
        }
    }


    Rigidbody2D GetConnectedObject(Vector2 position, Rigidbody2D originalObj)
    {
        Rigidbody2D[] sceneRigidbodies = GameObject.FindObjectsOfType<Rigidbody2D>();
        for (int i = 0; i < sceneRigidbodies.Length; i++)
        {
            SpriteRenderer sprite = sceneRigidbodies[i].GetComponent<SpriteRenderer>();
            if (originalObj != sceneRigidbodies[i] && sprite && sprite.bounds.Contains(position))
            {
                return sceneRigidbodies[i];
            }
        }
        return null;
    }
}
