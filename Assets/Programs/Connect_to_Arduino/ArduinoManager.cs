using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;
using System;

public class ArduinoManager : MonoBehaviour
{
    // ------------------ シリアル & LED ------------------
    public string portName = "COM9";
    public int baudRate = 9600;
    private SerialPort port;

    // ------------------ UI ------------------
    public Text logText; // シリアルログ表示用

    // ------------------ Buff/Debuff ------------------
    public float eventInterval = 10f; // イベント発生間隔（秒）
    public Area buffArea;
    public Area debuffArea;

    private System.Random rnd = new System.Random();

    void Start()
    {
        try
        {
            port = new SerialPort(portName, baudRate);
            port.NewLine = "\n";
            port.ReadTimeout = 500;
            port.Open();
            System.Threading.Thread.Sleep(2000); // Arduino 起動待ち
            Log("Serial Port Opened: " + portName);
        }
        catch (Exception e)
        {
            Log("Failed to open serial port: " + e.Message);
        }

        InvokeRepeating(nameof(SetBuffDebuffAreas), 0f, eventInterval);
    }

    void Update()
    {
        if (port != null && port.IsOpen)
        {
            try
            {
                while (port.BytesToRead > 0)
                {
                    string line = port.ReadLine();
                    Log(line);
                }
            }
            catch { }
        }
    }

    void Log(string message)
    {
        if (logText != null)
        {
            logText.text += message + "\n";
        }
        Debug.Log(message);
    }

    // enum → 0～3のインデックスに変換
    int AreaToIndex(Area area)
    {
        switch (area)
        {
            case Area.TopLeft: return 0;
            case Area.TopRight: return 1;
            case Area.BottomLeft: return 2;
            case Area.BottomRight: return 3;
            default: return -1; // None は送信しない
        }
    }

    void SetBuffDebuffAreas()
    {
        Area[] allAreas = (Area[])Enum.GetValues(typeof(Area));

        // バフエリアをランダム決定（Noneを除く）
        buffArea = allAreas[rnd.Next(1, allAreas.Length)];

        // デバフエリアはバフと被らないように決定
        Area temp;
        do
        {
            temp = allAreas[rnd.Next(1, allAreas.Length)];
        } while (temp == buffArea);

        debuffArea = temp;
        Log($"Buff Area: {buffArea}, Debuff Area: {debuffArea}");

        if (port != null && port.IsOpen)
        {
            // 4本分の情報をまとめて 1 バイトに変換
            int[] colors = new int[4]; // 0=黄, 1=緑, 2=赤
            Area[] areas = { Area.TopLeft, Area.TopRight, Area.BottomLeft, Area.BottomRight };
            for (int i = 0; i < 4; i++)
            {
                if (areas[i] == buffArea) colors[i] = 1;
                else if (areas[i] == debuffArea) colors[i] = 2;
                else colors[i] = 0;
            }

            int valueToSend = colors[0] * 27 + colors[1] * 9 + colors[2] * 3 + colors[3]; // 3進数展開
            port.Write(new byte[] { (byte)valueToSend }, 0, 1);
            Log($"[ArduinoLED] Sent combined value: {valueToSend}");
        }
        else
        {
            Log("Serial port not open. Cannot send LED command.");
        }
    }

    void OnApplicationQuit()
    {
        if (port != null && port.IsOpen)
        {
            port.Close();
            Log("Serial port closed.");
        }
    }
}
