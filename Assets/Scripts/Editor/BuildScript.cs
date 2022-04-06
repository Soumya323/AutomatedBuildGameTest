using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Collections.Generic;
using System;
using UnityEditor.SceneManagement;
using System.IO;

public class BuildScript
{

    /**************************************************************************************************************************************/
    #region 
    /**************************************************************************************************************************************/

    private const string BuildVersionCommandQuest = "-buildVersionQuest";
    private const string BuildVersionCodeCommandQuest = "-buildVersionCodeQuest";
    private const string BuildApkNameCommandQuest = "-buildNameForQuest";
    private const string BuildVersionCodeCommandPCVR = "-buildVersionCodePCVR";
    private const string BuildNameCommandPCVR = "-buildNameForPCVR";

    private const char CommandStartCharacter = '-';
    private static Dictionary<string, string> commandToValueDictionary = default;

    /**************************************************************************************************************************************/
    #endregion
    /**************************************************************************************************************************************/
    #region Final Build methods for Android and Windows
    /**************************************************************************************************************************************/

    public static void BuildAndroid()
    {
        string buildVersion = "1", buildVersionCode = "1", buildNameForQuestBase = "AutomatedBuildQuest_QA";
        string timeStamp = "_" + System.DateTime.Now.Month.ToString() + "M" + System.DateTime.Now.Day.ToString() + "D" + System.DateTime.Now.Hour.ToString() + "H" + System.DateTime.Now.Minute.ToString() + "m";

        List<string> scenePaths = new List<string>();
        foreach (EditorBuildSettingsScene scenePath in EditorBuildSettings.scenes)
        {
            scenePaths.Add(scenePath.path);
        }
        string[] allBuildScenes = scenePaths.ToArray();

        buildVersion = PlayerSettings.bundleVersion;
        buildVersionCode = PlayerSettings.Android.bundleVersionCode.ToString();
        BuildPlayerOptions buildPlayerOptionsAndroid = new BuildPlayerOptions();
        buildPlayerOptionsAndroid.scenes = allBuildScenes;
        string fullBuildNameForQuest = buildNameForQuestBase + "_" + buildVersion + "_" + buildVersionCode + timeStamp;
        buildPlayerOptionsAndroid.locationPathName = "BuildsOutput/Quest/Build/" + fullBuildNameForQuest + ".apk";
        buildPlayerOptionsAndroid.target = BuildTarget.Android;
        buildPlayerOptionsAndroid.options = BuildOptions.None;

        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = "Assets/Others/KeyStore/userAutoMatedBuild.keystore";
        PlayerSettings.Android.keystorePass = "8dsp?Q7mS87fHxRG";
        PlayerSettings.Android.keyaliasName = "district m key";
        PlayerSettings.Android.keyaliasPass = "8dsp?Q7mS87fHxRG";

        // Check and create the Directories if doesn't exist
        CreateBuildPaths();
        // Write the build name to file
        WriteBuildNameToFile(BuildTarget.Android, fullBuildNameForQuest);

        BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptionsAndroid);
        BuildSummary buildSummary = buildReport.summary;
    }

    public static void BuildPCVR()
    {
        string buildVersion = "1", baseBuildNameForPCVR = "AutomatedBuildPCVR";
        string timeStamp = "_" + System.DateTime.Now.Month.ToString() + "M" + System.DateTime.Now.Day.ToString() + "D" + System.DateTime.Now.Hour.ToString() + "H" + System.DateTime.Now.Minute.ToString() + "m";

        List<string> scenePaths = new List<string>();
        foreach (EditorBuildSettingsScene scenePath in EditorBuildSettings.scenes)
        {
            scenePaths.Add(scenePath.path);
        }
        string[] allBuildScenes = scenePaths.ToArray();

        buildVersion = PlayerSettings.bundleVersion;
        BuildPlayerOptions buildPlayerOptionsWindows = new BuildPlayerOptions();
        buildPlayerOptionsWindows.scenes = allBuildScenes;
        string fullBuildNameForPCVR = baseBuildNameForPCVR + "_" + buildVersion + "_" + timeStamp;
        buildPlayerOptionsWindows.locationPathName = "BuildsOutput/PCVR/Build/" + fullBuildNameForPCVR + ".exe";
        buildPlayerOptionsWindows.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptionsWindows.options = BuildOptions.None;

        // Check and create the Directories if doesn't exist
        CreateBuildPaths();
        // Write the build name to file
        WriteBuildNameToFile(BuildTarget.StandaloneWindows64, fullBuildNameForPCVR);

        BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptionsWindows);
        BuildSummary buildSummary = buildReport.summary;
    }

    /**************************************************************************************************************************************/
    #endregion
    /**************************************************************************************************************************************/
    #region Create build paths if they don't exist
    /**************************************************************************************************************************************/
    
    private static void CreateBuildPaths()
    {
        // In case the build paths are not created then create them before building
        // Otherwise throws error
        string rootBuildDirectory = "/BuildsOutput";
        string questBuildDirectory = "BuildsOutput/Quest";
        string pcvrBuildDirectory = "BuildsOutput/PCVR";

        if (Directory.Exists(rootBuildDirectory) == false)
            Directory.CreateDirectory(rootBuildDirectory);
        if (Directory.Exists(questBuildDirectory) == false)
            Directory.CreateDirectory(questBuildDirectory);
        if (Directory.Exists(pcvrBuildDirectory) == false)
            Directory.CreateDirectory(pcvrBuildDirectory);
    }

    /**************************************************************************************************************************************/
    #endregion
    /**************************************************************************************************************************************/
    #region Write the build name to text file
    /**************************************************************************************************************************************/

    private static void WriteBuildNameToFile(BuildTarget buildTarget, string buildName)
    {
        string path = "";
        if (buildTarget == BuildTarget.Android)
            path = "BuildsOutput/Quest/" + "BuildNameQuest.txt";
        else
            path = "BuildsOutput/PCVR/" + "BuildNamePCVR.txt";
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(buildName);
        writer.Close();
    }

    /**************************************************************************************************************************************/
    #endregion
    /**************************************************************************************************************************************/
    #region Read the commandline arguments
    /**************************************************************************************************************************************/

    private static Dictionary<string, string> GetCommandLineArguments()
    {
        Dictionary<string, string> commandToValueDictionary = new Dictionary<string, string>();

        string[] args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].StartsWith(CommandStartCharacter.ToString()))
            {
                string command = args[i];
                string value = string.Empty;

                if (i < args.Length - 1 && !args[i + 1].StartsWith(CommandStartCharacter.ToString()))
                {
                    value = args[i + 1];
                    i++;
                }

                if (!commandToValueDictionary.ContainsKey(command))
                {
                    commandToValueDictionary.Add(command, value);
                }
                else
                {
                    // BuildReporter.Current.Log("Duplicate command line argument " + command, BuildReporter.MessageSeverity.Warning);
                    // Duplicate commandline argument
                }
            }
        }

        return commandToValueDictionary;
    }

    /**************************************************************************************************************************************/
    #endregion
    /**************************************************************************************************************************************/
}


//https://github.com/Nordeus/UnityBuildPipeline/blob/master/CommandLineBuild.cs