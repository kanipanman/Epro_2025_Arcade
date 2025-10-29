using UnityEngine;
using System.IO.Ports;

public class ArduinoLEDController : MonoBehaviour
{
    public string portName = "COM9";   // 接続するシリアルポート
    public int baudRate = 9600;

    private SerialPort port;

    void Start()
    {
        try
        {
            port = new SerialPort(portName, baudRate);
            port.Open();
            port.ReadTimeout = 50;
            Debug.Log($"Arduino Serial Port Opened: {portName}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to open serial port: {e.Message}");
        }
    }

    /// <summary>
    /// LED色の変更コマンドを送信
    /// areaIndex: 0=TopLeft,1=TopRight,2=BottomLeft,3=BottomRight
    /// colorCode: 'Y'=Yellow,'G'=Green,'R'=Red
    /// </summary>
    public void SetLED(int areaIndex, char colorCode)
    {
        if (port != null && port.IsOpen)
        {
            string command = $"{areaIndex}{colorCode}";
            try
            {
                port.WriteLine(command);

                // --- ログ出力追加 ---
                string areaName = "";
                switch(areaIndex)
                {
                    case 0: areaName = "TopLeft"; break;
                    case 1: areaName = "TopRight"; break;
                    case 2: areaName = "BottomLeft"; break;
                    case 3: areaName = "BottomRight"; break;
                    default: areaName = "Unknown"; break;
                }

                string colorName = "";
                switch(colorCode)
                {
                    case 'Y': colorName = "Yellow"; break;
                    case 'G': colorName = "Green"; break;
                    case 'R': colorName = "Red"; break;
                    default: colorName = "Unknown"; break;
                }

                Debug.Log($"[ArduinoLED] Area: {areaName}, Color: {colorName}, Command Sent: {command}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to write to Arduino: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Serial port not open. Cannot send command.");
        }
    }

    void OnApplicationQuit()
    {
        if (port != null && port.IsOpen)
        {
            port.Close();
            Debug.Log("Serial port closed.");
        }
    }
}
