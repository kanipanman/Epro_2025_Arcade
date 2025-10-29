using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;

public class ArduinoLogUI : MonoBehaviour
{
    public string portName = "COM9";   // 接続ポート
    public int baudRate = 9600;
    public Text logText;               // UIのTextコンポーネント

    private SerialPort port;

    void Start()
    {
        try
        {
            port = new SerialPort(portName, baudRate);
            port.Open();
            port.ReadTimeout = 50;
            Log("Serial Port Opened: " + portName);
        }
        catch (System.Exception e)
        {
            Log("Failed to open serial port: " + e.Message);
        }
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
            catch (System.Exception)
            {
                // 読み取り失敗は無視
            }
        }
    }

    void Log(string message)
    {
        if (logText != null)
        {
            logText.text += message + "\n";
        }
        Debug.Log(message); // コンソールにも出力
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
