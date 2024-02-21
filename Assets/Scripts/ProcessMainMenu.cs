using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class ProcessMainMenu : MonoBehaviour
{
    [SerializeField] private Button condition1Button, condition2Button, condition3Button, quitButton;
    [SerializeField] private Button setConfigButton;

    private void Start()
    {
        condition1Button.onClick.AddListener(CreateProcessCondition1);
        condition2Button.onClick.AddListener(CreateProcessCondition2);
        condition3Button.onClick.AddListener(CreateProcessCondition3);
        setConfigButton.onClick.AddListener(CreateProcessSetConfig);
        quitButton.onClick.AddListener(exitApplication);
    }

    private void CreateProcessCondition1()
    {
        string workingDirectory = "C:\\BCI2000_v3_6\\batch\\rsvp_unity";
        string command = "/C start signalGenerator_c1.bat";
        CreateProcess(workingDirectory, command);
    }

    private void CreateProcessCondition2()
    {
        string workingDirectory = "C:\\BCI2000_v3_6\\batch\\rsvp_unity";
        string command = "/C start signalGenerator_c2.bat";
        CreateProcess(workingDirectory, command);
    }

    private void CreateProcessCondition3()
    {
        string workingDirectory = "C:\\BCI2000_v3_6\\batch\\rsvp_unity";
        string command = "/C start signalGenerator_c3.bat";
        CreateProcess(workingDirectory, command);
    }

    private void CreateProcessSetConfig()
    {
        string workingDirectory = "C:\\BCI2000_v3_6\\prog";
        string command = "/C BCI2000Command SetConfig";
        CreateProcess(workingDirectory, command);
    }
    private void CreateProcess(string workingDirectory, string command)
    {
        var processInfo = new ProcessStartInfo
        {
            WorkingDirectory = workingDirectory,
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = "cmd.exe",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = false,
            Arguments = command
        };
        var process = Process.Start(processInfo);
        process.WaitForExit();
    }

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            exitApplication();
        }
    }

    void exitApplication()
    {
        Application.Quit();
    }
}
