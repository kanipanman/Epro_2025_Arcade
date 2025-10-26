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
                Debug.Log($"Sent command to Arduino: {command}");
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
