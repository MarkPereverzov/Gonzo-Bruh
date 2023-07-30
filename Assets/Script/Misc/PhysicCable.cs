using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace HPhysic
{
    public class PhysicCable : MonoBehaviour
    {
        private int numberOfPoints = 1;
        private float space = 0.9f;
        private float size = 25.5f;

        public int flag = 0;
            
        private float springForce = 200;
        private float brakeLengthMultiplier = 2f;
        private float minBrakeTime = 1f;
        private float brakeLength;
        private float timeToBrake = 1f;

        public GameObject start;
        public GameObject end;
        public GameObject connector0; 
        public GameObject point0;

        private List<Transform> points;
        private List<Transform> connectors;

        private const string cloneText = "Wire";

        private Connector startConnector;
        private Connector endConnector;
        private void UpdatePoints()
        {
            if (!start || !end || !point0 || !connector0)
            {
                Debug.LogWarning("Can't update because one of objects to set is null!");
                return;
            }

            // delete old
            int length = transform.childCount;
            for (int i = 0; i < length; i++)
                if (transform.GetChild(i).name.StartsWith(cloneText))
                {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                    length--;
                    i--;
                }

            // set new
            Vector3 lastPos = start.transform.position;
            Rigidbody lasBody = start.GetComponent<Rigidbody>();
            for (int i = 0; i < numberOfPoints; i++)
            {
                GameObject cConnector = i == 0 ? connector0 : CreateNewCon(i);
                GameObject cPoint = i == 0 ? point0 : CreateNewPoint(i);

                Vector3 newPos = CountNewPointPos(lastPos);
                cPoint.transform.position = newPos;
                cPoint.transform.localScale = Vector3.one * size;
                cPoint.transform.rotation = transform.rotation;

                SetSpirng(cPoint.GetComponent<SpringJoint>(), lasBody);

                lasBody = cPoint.GetComponent<Rigidbody>();

                cConnector.transform.position = CountConPos(lastPos, newPos);
                Vector3 tmp = CountSizeOfCon(lastPos, newPos);
                Debug.Log(tmp.z);
                cConnector.transform.localScale = tmp;
                cConnector.transform.rotation = CountRoationOfCon(lastPos, newPos);
                lastPos = newPos;
            }

            Vector3 endPos = CountNewPointPos(lastPos);
            end.transform.position = endPos;
            SetSpirng(lasBody.gameObject.AddComponent<SpringJoint>(), end.GetComponent<Rigidbody>());

            GameObject endConnector = CreateNewCon(numberOfPoints);
            endConnector.transform.position = CountConPos(lastPos, endPos);
            endConnector.transform.rotation = CountRoationOfCon(lastPos, endPos);


            Vector3 CountNewPointPos(Vector3 lastPos) => lastPos + transform.forward * space;
        }
        private void AddPoint()
        {
            Transform lastprevPoint = GetPoint(numberOfPoints - 1);
            if (lastprevPoint == null)
            {
                Debug.LogWarning("Dont found point number " + (numberOfPoints - 1));
                return;
            }

            Rigidbody endRB = end.GetComponent<Rigidbody>();
            foreach (var spring in lastprevPoint.GetComponents<SpringJoint>())
                if (spring.connectedBody == endRB)
                    DestroyImmediate(spring);

            GameObject cPoint = CreateNewPoint(numberOfPoints);
            GameObject cConnector = CreateNewCon(numberOfPoints + 1);

            cPoint.transform.position = end.transform.position;
            cPoint.transform.rotation = end.transform.rotation;
            cPoint.transform.localScale = Vector3.one * size;

            SetSpirng(cPoint.GetComponent<SpringJoint>(), lastprevPoint.GetComponent<Rigidbody>());
            SetSpirng(cPoint.AddComponent<SpringJoint>(), endRB);

            // fix end
            end.transform.position += end.transform.forward * space;

            cConnector.transform.position = CountConPos(cPoint.transform.position, end.transform.position);
            //cConnector.transform.localScale = CountSizeOfCon(cPoint.transform.position, end.transform.position);
            Vector3 tmp = CountSizeOfCon(cPoint.transform.position, end.transform.position);
            Debug.Log(tmp.z);
            cConnector.transform.localScale = tmp;
            cConnector.transform.rotation = CountRoationOfCon(cPoint.transform.position, end.transform.position);

            points.Add(cPoint.transform);
            connectors.Add(cConnector.transform);

            numberOfPoints++;
        }
        private void RemovePoint()
        {
            if (numberOfPoints < 2)
            {
                Debug.LogWarning("Cable can't be shorter then 1");
                return;
            }

            Transform lastprevPoint = GetPoint(numberOfPoints - 1);
            if (lastprevPoint == null)
            {
                Debug.LogWarning("Dont found point number " + (numberOfPoints - 1));
                return;
            }

            Transform lastprevCon = GetConnector(numberOfPoints);
            if (lastprevCon == null)
            {
                Debug.LogWarning("Dont found connector number " + (numberOfPoints));
                return;
            }

            Transform lastlastprevPoint = GetPoint(numberOfPoints - 2);
            if (lastlastprevPoint == null)
            {
                Debug.LogWarning("Dont found point number " + (numberOfPoints - 2));
                return;
            }


            Rigidbody endRB = end.GetComponent<Rigidbody>();
            SetSpirng(lastlastprevPoint.gameObject.AddComponent<SpringJoint>(), endRB);

            end.transform.position = lastprevPoint.position;
            end.transform.rotation = lastprevPoint.rotation;

            DestroyImmediate(lastprevPoint.gameObject);
            DestroyImmediate(lastprevCon.gameObject);

            numberOfPoints--;
        }


        private void Start()
        {
            points = new List<Transform>();
            connectors = new List<Transform>();
            points.Add(point0.transform);
            connectors.Add(connector0.transform);
            for (int i = 0; i < 5; i++)
            {
                AddPoint();
            }
            Debug.Log("Suka");
        }

        private void FixedUpdate()
        {    
            float cableLength = 0f;
            bool isConnected = false;//startConnector.IsConnected || endConnector.IsConnected;

            int numOfParts = connectors.Count;
            Debug.Log(numOfParts);
            Transform lastPoint = points[0];
            for (int i = 0; i < numOfParts-1; i++)
            {
                Transform nextPoint = points[i + 1];
                Transform connector = connectors[i].transform;
                connector.position = CountConPos(lastPoint.position, nextPoint.position);
                //Debug.Log(connector.position);
                if (lastPoint.position == nextPoint.position || nextPoint.position == connector.position)
                {
                    connector.localScale = Vector3.zero;
                }
                else
                {
                    connector.rotation = Quaternion.LookRotation(nextPoint.position - connector.position);
                    Vector3 tmp = CountSizeOfCon(lastPoint.position, nextPoint.position);
                    Debug.Log(tmp.z);
                    Debug.Log(lastPoint.position);
                    Debug.Log(nextPoint.position);
                    connector.localScale = tmp;
                }

                if (isConnected)
                    cableLength += (lastPoint.position - nextPoint.position).magnitude;

                lastPoint = nextPoint;
            }

            if (isConnected)
            {
                if (cableLength > brakeLength)
                {
                    timeToBrake -= Time.deltaTime;
                    if (timeToBrake < 0f)
                    {
                        startConnector.Disconnect();
                        endConnector.Disconnect();
                        timeToBrake = minBrakeTime;
                    }
                }
                else
                {
                    timeToBrake = minBrakeTime;
                }
            }
        }
        private Vector3 CountConPos(Vector3 start, Vector3 end) => (start + end) / 2f;
        private Vector3 CountSizeOfCon(Vector3 start, Vector3 end) => new Vector3(size, size, ((start - end).magnitude +1)*33.3f);
        private Quaternion CountRoationOfCon(Vector3 start, Vector3 end) => Quaternion.LookRotation(end - start, Vector3.right);
        private string ConnectorName(int index) => $"{cloneText}Connector ({index})";
        private string PointName(int index) => $"{cloneText}Point ({index})";
        private Transform GetConnector(int index) => index > 0 ? transform.Find(ConnectorName(index)) : connector0.transform;
        private Transform GetPoint(int index) => index > 0 ? transform.Find(PointName(index)) : point0.transform;


        public void SetSpirng(SpringJoint spring, Rigidbody connectedBody)
        {
            spring.connectedBody = connectedBody;
            spring.spring = springForce;
            spring.damper = 0.2f;
            spring.autoConfigureConnectedAnchor = false;
            spring.anchor = Vector3.zero;
            spring.connectedAnchor = Vector3.zero;
            spring.minDistance = space;
            spring.maxDistance = space;
        }
        private GameObject CreateNewPoint(int index)
        {
            GameObject temp = Instantiate(point0);
            temp.name = PointName(index);
            temp.transform.parent = transform;
            //points.Add(temp.transform);
            return temp;
        }
        private GameObject CreateNewCon(int index)
        {
            GameObject temp = Instantiate(connector0);
            temp.name = ConnectorName(index);
            temp.transform.parent = transform;
            return temp;
        }
    }
}