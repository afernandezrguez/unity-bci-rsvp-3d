using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;

public class ProcessMainMenu : MonoBehaviour
{
    [SerializeField] private Button[] signalButtons;
    [SerializeField] private Button signal1Button, signal2Button;
    [SerializeField] private Button participantButton, quitButton, setConfigButton;
    [SerializeField] private GameObject participantNumber;
    [SerializeField] private Toggle feedbackToggle;
    [SerializeField] private Toggle condition1Toggle, condition2Toggle;

    public VirtualKeyboard virtualKeyboard;
    public SequenceController sequenceController;
    public bool feedbackMode, testingMode;
    private string participantCode, numberOfSequences, conditionSelected;
    private const string BCI2000Directory = "C:/BCI2000_v3_6";

    private void Start()
    {
        InitializeMenu();
        CloseBCI2000();
        CloseAllCmdWindows();
    }

    private void InitializeMenu()
    {
        condition1Toggle.onValueChanged.AddListener((isOn) => OnToggleChanged());
        condition2Toggle.onValueChanged.AddListener((isOn) => OnToggleChanged());
        signal1Button.onClick.AddListener(ActionForSignal1);
        signal2Button.onClick.AddListener(ActionForSignal2);
        participantButton.onClick.AddListener(OpenParticipantPanel);
        setConfigButton.onClick.AddListener(CreateProcessSetConfig);
        quitButton.onClick.AddListener(ExitApplication);
    }

    private void OnToggleChanged()
    {
        conditionSelected = condition1Toggle.isOn ? "1" : condition2Toggle.isOn ? "2" : null;
    }

    private void OpenParticipantPanel()
    {
        participantNumber.GetComponent<Text>().text = string.Empty;
        virtualKeyboard.participantNumber = string.Empty;
    }

    private void ActionForSignal1()
    {
        StartBCI2000Process("signalGenerator.bat", true);
    }

    private void ActionForSignal2()
    {
        StartBCI2000Process("actichamp.bat", false);
    }

    private void StartBCI2000Process(string batchFileName, bool isTesting)
    {
        //CloseBCI2000();
        //CloseAllCmdWindows();

        //string workingDirectory = $"{BCI2000Directory}/batch/rsvp_unity_voice";
        //string command = $"/C start {batchFileName}";

        // Si lo dejo así, no me funciona el comando de voz en el primer menú (i.e., el de setConfig).
        string workingDirectory = $"{BCI2000Directory}/batch/rsvp_unity_voice";
        string batchFilePath = $"{workingDirectory}/{batchFileName}";
        string command = $"/C \"{batchFilePath}\"";  // Sin 'start'

        ExecuteCommand(workingDirectory, command);
        testingMode = isTesting;
        //CloseAllCmdWindows();
    }

    private void CreateProcessSetConfig()
    {
        participantCode = testingMode ? "test" : $"UV{virtualKeyboard.participantNumber}";
        numberOfSequences = sequenceController.currentNumber.ToString();
        string workingDirectory = $"{BCI2000Directory}/prog";
        string displayResults = feedbackToggle.isOn ? "1" : "0";
        feedbackMode = feedbackToggle.isOn;

        // Comandos individuales
        string commandSubjectName = $"/C BCI2000Command SetParameter SubjectName {participantCode}";
        string commandSubjectSession = $"/C BCI2000Command SetParameter SubjectSession {conditionSelected}";
        string commandNumberOfSequences = $"/C BCI2000Command SetParameter NumberOfSequences {numberOfSequences}";
        string commandEpochsToAverage = $"/C BCI2000Command SetParameter EpochsToAverage {numberOfSequences}";
        string commandDisplayResults = $"/C BCI2000Command SetParameter DisplayResults {displayResults}";
        string commandSetConfig = $"/C BCI2000Command SetConfig";

        // Ejecutar cada comando individualmente
        ExecuteCommand(workingDirectory, commandSubjectName);
        ExecuteCommand(workingDirectory, commandSubjectSession);
        ExecuteCommand(workingDirectory, commandNumberOfSequences);
        ExecuteCommand(workingDirectory, commandEpochsToAverage);
        ExecuteCommand(workingDirectory, commandDisplayResults);
        ExecuteCommand(workingDirectory, commandSetConfig);

    }

    private void ExecuteCommand(string workingDirectory, string command)
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

        using Process process = Process.Start(processInfo);
        process.WaitForExit();
    }

    private void CloseBCI2000()
    {
        string workingDirectory = $"{BCI2000Directory}/prog";
        ExecuteCommand(workingDirectory, "/C BCI2000Command Quit");
    }

    private void CloseAllCmdWindows()
    {
        foreach (Process process in Process.GetProcessesByName("cmd"))
        {
            process.Kill();
        }
    }

    private void ExitApplication()
    {
        Application.Quit();
    }
}
