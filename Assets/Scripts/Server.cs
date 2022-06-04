using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
public class Server : MonoBehaviour
{
    Thread mThread;
    public string connectionIP = "127.0.0.1:";
    public int connectionPort = 1024;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    Vector3 startPos;
    Vector3 receivedPos;
    NetworkStream nwStream;
    
    RaycastHit hit1;

    RaycastHit hit2;

    RaycastHit hit3;

    RaycastHit hit4;

    public float Rdistance;

    public float Ddistance;

    public float Ldistance;

    public float Udistance;

    bool running;
    bool move = false;
    bool quit = false;


    [SerializeField] GameObject shardPrefab;
    private void Start()
    {
        startPos = transform.position;
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
        
    }

    private void Update() 
    {
        if(quit)
        {
            Application.Quit();
        }
        if(move)
        {
            transform.position= startPos;
            move = false;
        }  
        RaycastHit hit1;
        Ray rightRay = new Ray(transform.position, Vector3.right);
        if (Physics.Raycast(rightRay, out hit1) && hit1.transform.tag == "Wall")
        {
            Rdistance = hit1.distance;
        }
        RaycastHit hit2;
        Ray downRay = new Ray(transform.position, -Vector3.up);
        if (Physics.Raycast(downRay, out hit2) && hit2.transform.tag == "Wall")
        {
            Ddistance = hit2.distance;
        }
        RaycastHit hit3;
        Ray leftRay = new Ray(transform.position, -Vector3.right);
        if (Physics.Raycast(leftRay, out hit3) && hit3.transform.tag == "Wall")
        {
            Ldistance = hit3.distance;
        }
        RaycastHit hit4;
        Ray upRay = new Ray(transform.position, Vector3.up);
        if (Physics.Raycast(upRay, out hit4) && hit4.transform.tag == "Wall")
        {
            Udistance = hit4.distance;
        }
    }
    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();
        client = listener.AcceptTcpClient();
        nwStream = client.GetStream();
        running = true;
        while (running)
        {
            SendAndReceiveData();
        }
        listener.Stop();
    }

    void SendAndReceiveData()
    {
        byte[] buffer = new byte[client.ReceiveBufferSize];
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); 
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); 
        if (dataReceived != null)
        {
            if(dataReceived == "data")
            {
                Debug.Log(dataReceived);
                SendString(nwStream, Rdistance + " " + Ddistance + " " + Ldistance + " " + Udistance);
            }
            else
            {
                Move(dataReceived);
            }
            
        }
        nwStream.Flush();
    }

    void Move(string s)
    {
        switch (s)
        {
            case "right":
            {
                startPos += new Vector3(1,0,0);
                move = true;
                break;
            }
            case "down":
            {
                startPos += new Vector3(0,-1,0);
                move = true;
                break;
            }
            case "left":
            {
                startPos += new Vector3(-1,0,0);
                move = true;
                break;
            }
            case "up":
            {
                startPos += new Vector3(0,1,0);
                move = true;
                break;
            }
            case "die":
            {
               quit= true;
               break;
            }
        }
    }

    void SendString(NetworkStream stream, string s)
    {
        byte[] msg = Encoding.UTF8.GetBytes(s);
        stream.Write(msg, 0, msg.Length);
        Debug.Log($"Sent {msg.Length} bytes: {s}");
    }


    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Trap" || other.tag == "Finish")
            quit= true;
    }
    
    private void Explode()
    {
        for (var x = 0;x < 7; x++)
        {
            for (var y = 0; y<5 ;y++)
            {
                for (var z = 0; z<5 ;z++)
                {
                    var shard = GameObject.Instantiate(shardPrefab);
                    var coordX =transform.position.x - 0.7f + 0.3f * x;
                    var coordY =transform.position.y - 0.2f + 0.1f *y;
                    var coordZ =transform.position.z - 0.2f + 0.1f *z;
                    shard.transform.position = new Vector3(coordX, coordY, coordZ);
                }
            }
        }
    }

}
