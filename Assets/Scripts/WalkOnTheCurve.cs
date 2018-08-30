using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkOnTheCurve : MonoBehaviour {

    public class Point
    {
        Vector3 T, B, N, acc, coordinate, nextPoint, v0, v1;
        float curveLenght, intervalLenght, step, interval;
        int numberOfPoints;
        WalkOnTheCurve curve;


        //constructor
        Point(Vector3 coordinate, Vector3 v0, float curveLenght,  int numberOfPoints, float interval, float step)
        {
            this.coordinate = coordinate; //coordinate of the point
            this.curveLenght = curveLenght; //total length
            this.numberOfPoints = numberOfPoints;
            this.interval = interval; //size of the interval
            this.step = step; //which t is equivalent to this coordinate
            //find the next point
            //calculate the v1 vector
            calculateVelocity();
            //calculate acceleration
            calculateAcc();
            //calculate the T vector
            calculateT();
            //calculate B vector
            calculateB();
            //calculate N vector
            calculateN();
        }
        public void findNextPoint()
        {
            // set points of Hermite curve
            Vector3 position = coordinate;
            float length = 0;
   
            this.nextPoint = this.coordinate;
   
            //calculate points untill finding the next one with lenght equal or greater than the interval defined
            while(length < interval)
            {
                position = curve.generatePoints(step);
                length = Vector3.Magnitude(position - coordinate);
            }
            this.nextPoint = position;

        }

        public void calculateVelocity()
        {
            this.v1 = this.nextPoint - this.coordinate;
        }
        public void calculateAcc()
        {
            this.acc = v1 - v0;
        }

        public void calculateT()
        {
            this.T = Vector3.Normalize(acc);
        }

        public void calculateB()
        {
            this.B = Vector3.Normalize(Vector3.Cross(this.v1, this.acc));
        }

        public void calculateN()
        {
            this.N = Vector3.Cross(this.T, this.B);

        }

};
    // Use this for initialization
    public GameObject curve;
    public GameObject startPoint, startTangentPoint, endPoint, endTangentPoint;
    public float deltaX, deltaY, deltaZ;
    public int numberOfPoints, countPosition;
    Vector3 newPosition;
    Point []listOfPoints;



    Quaternion rotation;// Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
    Vector3 currentPoint, previousPoint, nextPoint, v0, v1, acceleration, B, N, T;
    void Start () {
        
        countPosition = 2; //As the sphere starts at the first position, the count position is set to 2 to control how far the nextPoint variable will be calculated
        numberOfPoints = 50;

                    /* Initialize the points for the first step */
        previousPoint = startPoint.transform.position; //set the previous point as the first point of the curve
        currentPoint = generatePoints(1 / (numberOfPoints - 1.0f)); //calculate the position of the first point as the start point + one step
        transform.position = currentPoint; //set the object at the first step of the curve
        nextPoint = generatePoints(2 / (numberOfPoints - 1.0f)); //calculate the next point (step) 
        v0 = derivate(previousPoint, currentPoint); //Tangent vector for the first point (step)

    }

   
    void Update ()
    {

                
         
        if (countPosition < numberOfPoints )
        {
            FrenetSerret();
            //Update the values for the next step
            previousPoint = currentPoint;
            currentPoint = nextPoint;
            nextPoint = generatePoints((countPosition + 1) / (numberOfPoints - 1.0f));

            v0 = v1; //T vector

            countPosition++;
        }
    }


    void FrenetSerret()
    { 
        Matrix4x4 TRSMatrix = new Matrix4x4();
        v1 = derivate(currentPoint, nextPoint);
        T = Vector3.Normalize(v1); //First order derivate normalized
        acceleration = (v1 - v0); //N Vector
        B = Vector3.Normalize(Vector3.Cross(v1, acceleration)); //Cross product between first and second derivates (velocity and acceleration)
        N = Vector3.Normalize(Vector3.Cross(B, T));



        TRSMatrix.SetColumn(0, new Vector4(N.x, N.y, N.z, 0.0f)); //transform.up);
        TRSMatrix.SetColumn(1, new Vector4(B.x, B.y, B.z, 0.0f)); //transform.right);
        TRSMatrix.SetColumn(2, new Vector4(T.x, T.y, T.z, 0.0f)); // transform.forward);
        TRSMatrix.SetColumn(3, new Vector4(nextPoint.x, nextPoint.y, nextPoint.z, 1.0f));
        

       /* TRSMatrix.SetRow(0, new Vector4(N.x, N.y, N.z, 0.0f)); //transform.up);
        TRSMatrix.SetRow(1, new Vector4(B.x, B.y, B.z, 0.0f)); //transform.right);
        TRSMatrix.SetRow(2, new Vector4(T.x, T.y, T.z, 0.0f)); // transform.forward);
        TRSMatrix.SetRow(3, new Vector4(nextPoint.x, nextPoint.y, nextPoint.z, 1.0f));
        */
        transform.position = TRSMatrix.MultiplyPoint3x4(nextPoint);
       // Debug.Log(transform.position);

    }

    Vector3 generatePoints(float t)
    {
            // set points of Hermite curve
            Vector3 p0 = startPoint.transform.position;
            Vector3 p1 = endPoint.transform.position;
            Vector3 m0 = startTangentPoint.transform.position - startPoint.transform.position;
            Vector3 m1 = endTangentPoint.transform.position - endPoint.transform.position;
           
            Vector3 position;

            position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * p0
                + (t * t * t - 2.0f * t * t + t) * m0
                + (-2.0f * t * t * t + 3.0f * t * t) * p1
                + (t * t * t - t * t) * m1;

        //   Debug.Log(position);
        return position;         
            
     }

    Vector3 derivate(Vector3 previous, Vector3 current)
    {
        Vector3 velocity = current - previous;
        return velocity;
    }

    public float curveSize()
    {


        return 0;

    }

    
}
