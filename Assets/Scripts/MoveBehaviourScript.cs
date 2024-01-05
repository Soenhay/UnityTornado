using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class MoveBehaviourScript : MonoBehaviour
{
    private float speed = 1.0f;

    private float xSign, ySign, zSign;

    float angle, radius, height;
    float angleSpeed = 2f;
    float angleSpeedRandom = 2f;
    float radialSpeed = 1f;
    bool radiusExpand = true; 
    float objectRotateSpeed = -35.0f; 
    public Vector3 direction;



    float xSpeed;
    float ySpeed;
    float zSpeed;


    // Scales the speed of the spiral motion,
    public float TimeScale = 2f;

    float m_Time;


    // Controls the direction/speed the spiral translates
    private Vector3 LocalRotationVelocity;

    TrailRenderer trailRenderer;

    GameObject tornadoBase;

    // Start is called before the first frame update
    void Start()
    {
        int seed = 0;
        foreach (char c in gameObject.name)
        {
            seed += char.ToUpper(c) - 64;
        }
        Debug.Log($"Seed: {seed}");

        Random.InitState(seed);

        xSign = Random.value < .5 ? 1 : -1;
        ySign = 1.0f;// Random.value < .5 ? 1 : -1;
        zSign = Random.value < .5 ? 1 : -1;

        xSpeed = Random.value;
        ySpeed = Random.value;
        zSpeed = Random.value;
        LocalRotationVelocity = new Vector3(xSign * xSpeed, ySign * ySpeed, zSign * zSpeed);
        LocalRotationVelocity *= objectRotateSpeed;

        Debug.Log($"name={gameObject.name}, xSign={xSign}, xSpeed={xSpeed}, ySign={ySign} ySpeed={ySpeed}, zSign={zSign}, zSpeed={zSpeed}, LinearVelocity={LocalRotationVelocity}");

        #region trailRenderer
        gameObject.AddComponent<TrailRenderer>();
        trailRenderer = gameObject.GetComponent<TrailRenderer>();
        trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        trailRenderer.generateLightingData = true;
        trailRenderer.time = 23;
        trailRenderer.startWidth = .069f;
        trailRenderer.endWidth = 0.31f;
        float alpha = zSpeed;// 1.0f;
        trailRenderer.startColor = Color.yellow;
        trailRenderer.endColor = Color.green;
        trailRenderer.colorGradient = new Gradient()
        {
            colorKeys = new GradientColorKey[] {
                new GradientColorKey(new Color(0.1f, Random.value, 0.01f), 0.0f ),
                new GradientColorKey(new Color(0.1f, 0.69f, Random.value), .69f )
            },
            alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        };
        #endregion

        tornadoBase = GameObject.Find("TornadoBase");

        direction = (transform.position - tornadoBase.transform.position).normalized;
        radius = Vector3.Distance(tornadoBase.transform.position, transform.position);
        angleSpeedRandom = Random.Range(1.5f, 2.5f);
        height = (transform.position - tornadoBase.transform.position).y;
    }

    // Update is called once per frame
    void Update()
    {
        //float xPos = speed * x * Time.deltaTime;
        //float yPos = speed * y * Time.deltaTime;
        //float zPos = speed * z * Time.deltaTime;


        //float xPos = Mathf.Sin(Time.time * circleSpeed) * circleSize;
        //float yPos = Mathf.Cos(Time.time * circleSpeed) * circleSize;
        //float zPos = 0;// speed * Time.deltaTime;
        Vector3 position;
        float xPos = 0.0f;
        float yPos = 0.0f;
        float zPos = 0.0f;

        int method = 6;
        switch (method)
        {
            case 1:
                #region 1
                {
                    //angle = angleSpeed * m_Time * xSign;
                    radius = radialSpeed * m_Time;

                    height += Time.deltaTime;

                    position = LocalRotationVelocity * m_Time;

                    xPos = radius * Mathf.Cos(angleSpeed * m_Time);
                    yPos = height;//radius * Mathf.Cos(angleSpeed * m_Time * ySign);
                    zPos = radius * Mathf.Sin(angleSpeed * m_Time);
                    position += new Vector3(xPos, yPos, zPos);


                    transform.Rotate(LocalRotationVelocity * objectRotateSpeed * Time.deltaTime);//Rotate around it's own axis.
                                                                                          //transform.RotateAround(tornadoBase.transform.position, Vector3.up, 0.69f);


                    transform.position = tornadoBase.transform.position + position;
                    //transform.Translate(xPos, yPos, zPos);

                    //transform.position += new Vector3(distanceX * Time.deltaTime, distanceY * Time.deltaTime, 0);
                }
                #endregion
                break;
            case 2:
                #region 2
                {
                    height += Time.deltaTime;
                    radius += Time.deltaTime;
                    angle += radialSpeed * Time.deltaTime;
                    if (angle > 360)
                    {
                        angle -= 360;
                    }

                    xPos = radius * Mathf.Cos(angle);
                    yPos = height;//radius * Mathf.Cos(angleSpeed * m_Time * ySign);
                    zPos = radius * Mathf.Sin(angle);

                    Vector3 orbit = Vector3.forward * radius;
                    //Vector3 orbit = LinearVelocity * radius;
                    //orbit = Quaternion.LookRotation(direction) * Quaternion.Euler(0, angle, 0) * orbit;
                    orbit = Quaternion.LookRotation(direction) * Quaternion.Euler(xPos, yPos, zPos) * orbit;
                    transform.position = tornadoBase.transform.position + orbit;
                }
                #endregion
                break;
            case 3:
                #region 3  - planetary orbit
                {
                    angle += 20 * Time.deltaTime;
                    if (angle > 360)
                    {
                        angle -= 360;
                    }
                    Vector3 orbit = Vector3.forward * radius;
                    orbit = Quaternion.LookRotation(direction) * Quaternion.Euler(0, angle, 0) * orbit;
                    transform.position = tornadoBase.transform.position + orbit;
                    transform.Rotate(LocalRotationVelocity * objectRotateSpeed * Time.deltaTime);//Rotate around it's own axis.
                }
                #endregion
                break;
            case 4:
                #region 4
                {
                    //angle = angleSpeed * m_Time * xSign;
                    radius += radialSpeed * Time.deltaTime;
                    height += Time.deltaTime;

                    position = LocalRotationVelocity * m_Time;

                    xPos = tornadoBase.transform.position.x + radius * Mathf.Cos(angleSpeed * m_Time);
                    yPos = tornadoBase.transform.position.y + height;//radius * Mathf.Cos(angleSpeed * m_Time * ySign);
                    zPos = tornadoBase.transform.position.z + radius * Mathf.Sin(angleSpeed * m_Time);
                    position += new Vector3(xPos, yPos, zPos);

                    transform.Rotate(LocalRotationVelocity * objectRotateSpeed * Time.deltaTime);//Rotate around it's own axis.
                    transform.position = position;
                }
                #endregion
                break;
            case 5:
                #region 5 basic spiral centered
                {
                    //angle = angleSpeed * m_Time * xSign;
                    radius += radialSpeed * Time.deltaTime;
                    height += Time.deltaTime;

                    position = tornadoBase.transform.position;

                    xPos = radius * Mathf.Cos(angleSpeed * m_Time);
                    yPos = height;//radius * Mathf.Cos(angleSpeed * m_Time * ySign);
                    zPos = radius * Mathf.Sin(angleSpeed * m_Time);
                    position += new Vector3(xPos, yPos, zPos);

                    transform.Rotate(LocalRotationVelocity * objectRotateSpeed * Time.deltaTime);//Rotate around it's own axis.
                    transform.position = position;
                }
                #endregion
                break;
            case 6:
                #region 6 craziness
                {
                    //angle = angleSpeed * m_Time * xSign;
                    radius += radialSpeed * Time.deltaTime;
                    height += Time.deltaTime;

                    position = tornadoBase.transform.position;

                    xPos = radius * Mathf.Cos(m_Time * angleSpeed * xSpeed);
                    yPos = height;//radius * Mathf.Cos(angleSpeed * m_Time * ySign);
                    zPos = radius * Mathf.Sin(m_Time * angleSpeed * zSpeed);
                    position += new Vector3(xPos, yPos, zPos);

                    transform.Rotate(LocalRotationVelocity * Time.deltaTime);//Rotate around it's own axis.
                    transform.position = position;
                }
                #endregion
                break;
            case 7:
                #region 7 Mostly working as a tornado.
                {
                    //angle = angleSpeed * m_Time * xSign;
                    radius += radialSpeed * Time.deltaTime;
                    height += Time.deltaTime;

                    position = tornadoBase.transform.position;

                    xPos = radius * Mathf.Cos(m_Time * angleSpeedRandom);
                    yPos = height;//radius * Mathf.Cos(angleSpeed * m_Time * ySign);
                    zPos = radius * Mathf.Sin(m_Time * angleSpeedRandom);
                    position += new Vector3(xPos, yPos, zPos);

                    transform.Rotate(LocalRotationVelocity * Time.deltaTime);//Rotate around it's own axis.
                    transform.position = position;
                }
                #endregion
                break;
        }


        //Debug.Log(transform.position);
        if (gameObject.name == "rock_m")
        {
            Debug.Log($"name={gameObject.name}, angle={angle}, radius={radius}, height={height}, position={transform.position}, x={xPos}, y={yPos}, z={zPos}, timeDelta={Time.deltaTime}, m_Time={m_Time}");
        }

        // Adjust time.
        //m_Time += TimeScale * Time.deltaTime;
        m_Time += TimeScale * Time.deltaTime;
    }



    //// Controls the radius of the circular motion for X/Y axis.
    //public Vector2 Radii = new Vector2(2f, 3f);

    //// Controls the direction/speed the spiral translates
    //public Vector2 LinearVelocity = new Vector2(0.5f, 0f);

    //// Controls the rotational speed of the spiral.
    //public float AngularVelocity = 1f;

    //// Scales the speed of the spiral motion,
    //public float TimeScale = 2f;

    //float m_Time;

    //void Update()
    //{
    //    // Calculate the angle at this time.
    //    var angle = AngularVelocity * m_Time;

    //    // Calculate the linear translation at this time.
    //    var position = LinearVelocity * m_Time;

    //    // Add in the circular motion.
    //    position += new Vector2(Mathf.Cos(angle) * Radii.x, Mathf.Sin(angle) * Radii.y);

    //    // Set the transform to this spiral position.
    //    transform.position = position;

    //    // Adjust time.
    //    m_Time += TimeScale * Time.deltaTime;
    //}
}
