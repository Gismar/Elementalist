using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class SaveHandler {

    [SerializeField] private GlobalDataHandler _GlobalData;

    public void SaveGame()
    {
        ClearFile();
        StreamWriter writer = new StreamWriter("Assets/Save.txt");
        writer.Write(EncodeToBase64(JsonUtility.ToJson(_GlobalData)));
        writer.Close();
    }

    public void LoadGame()
    {
        StreamReader reader = new StreamReader("Assets/Save.txt");
        string line = DecodeFromBase64(reader.ReadLine());
        //JsonUtility.FromJsonOverwrite(line, _WorldInfo);
        _GlobalData = JsonUtility.FromJson<GlobalDataHandler>(line);
        reader.Close();
    }

    public void ClearFile()
    {
        File.WriteAllText("Assets/Save.txt", string.Empty);
    }

	public string EncodeToBase64(string message)
    {
        return System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(message));
    }

    public string DecodeFromBase64(string message)
    {
        return System.Text.Encoding.ASCII.GetString(System.Convert.FromBase64String(message));
    }
}
