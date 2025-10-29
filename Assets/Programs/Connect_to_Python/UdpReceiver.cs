using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using UnityEngine;

[Serializable]

public class MarkerData
{
    public int id;
    public string area;
    public float cx;
    public float cy;
}

[Serializable]
public class MarkerPacket
{
public MarkerData[] markers;
}

public class UdpReceiver : MonoBehaviour
{
public int listenPort = 5005; // Python側と合わせる
private UdpClient udpClient;
private Thread receiveThread;
private bool running = true;

private ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

void Start()
{
    try
    {
        udpClient = new UdpClient(listenPort);
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        Debug.Log($"[UdpReceiver] Listening on port {listenPort}");
    }
    catch (Exception e)
    {
        Debug.LogError("[UdpReceiver] Start error: " + e.Message);
    }
}

void ReceiveData()
{
    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, listenPort);
    while (running)
    {
        try
        {
            byte[] data = udpClient.Receive(ref remoteEP);
            string json = Encoding.UTF8.GetString(data);
            Debug.Log($"[UdpReceiver] Received JSON: {json}");
            queue.Enqueue(json);
        }
        catch (Exception e)
        {
            if (running)
                Debug.LogError("[UdpReceiver] Receive error: " + e.Message);
        }
    }
}

    void Update()
    {
        while (queue.TryDequeue(out string json))
        {
            try
            {
                MarkerPacket packet = JsonUtility.FromJson<MarkerPacket>(json);
                if (packet?.markers == null)
                {
                    Debug.LogWarning("[UdpReceiver] Invalid or empty JSON packet");
                    continue;
                }

                foreach (var m in packet.markers)
                {
                    Debug.Log($"[UdpReceiver] Marker {m.id} in {m.area} (x={m.cx:F1}, y={m.cy:F1})");

                    // --- Python文字列 → Area列挙型に変換 ---
                    Area area = Area.None;
                    switch (m.area)
                    {
                        case "Top-Left": area = Area.TopLeft; break;
                        case "Top-Right": area = Area.TopRight; break;
                        case "Bottom-Left": area = Area.BottomLeft; break;
                        case "Bottom-Right": area = Area.BottomRight; break;
                        default:
                            Debug.LogWarning($"[UdpReceiver] Unknown area string '{m.area}'");
                            break;
                    }

                    // --- マーカーID → プレイヤー番号対応 ---
                    int playerNum = 0;
                    switch (m.id)
                    {
                        case 1: playerNum = 1; break;
                        case 2: playerNum = 2; break;
                        case 3: playerNum = 3; break;
                        case 4: playerNum = 4; break;
                        default:
                            Debug.LogWarning($"[UdpReceiver] Unknown Marker ID: {m.id}");
                            continue;
                    }

                    // --- PlayerAreaTracker に登録（存在すれば）---
                    UdpReceiver.SetPlayerArea(playerNum, area);

                    Debug.Log($"[UdpReceiver] SetPlayerArea: Player{playerNum} -> {area}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("[UdpReceiver] JSON parse error: " + e.Message + "\nJSON: " + json);
            }
        }
    }
// プレイヤーごとの現在エリアを保存
private static Area[] playerAreas = new Area[5]; // 1～4P使用

public static void SetPlayerArea(int playerNum, Area area)
{
    if (playerNum >= 1 && playerNum <= 4)
        playerAreas[playerNum] = area;
}

public static Area GetPlayerArea(int playerNum)
{
    if (playerNum >= 1 && playerNum <= 4)
        return playerAreas[playerNum];
    return Area.None;
}

void OnApplicationQuit()
{
    running = false;
    udpClient?.Close();
    if (receiveThread != null && receiveThread.IsAlive)
    {
        receiveThread.Abort();
    }
    Debug.Log("[UdpReceiver] Stopped.");
}


}