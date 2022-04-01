using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Collections.Generic;
using System;

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


    public static void PerformBuild()
    {
        commandToValueDictionary = GetCommandLineArguments();

        BuildAndroid();
        //BuildWindows();
    }

    /**************************************************************************************************************************************/
    #region Final Build methods for Android and Windows
    /**************************************************************************************************************************************/

    public static void BuildAndroid()
    {
        string buildVersion = "1", buildVersionCode = "1", buildNameForQuest = "AutomatedBuildQuest";

        /*
        // Dictionary<string, string> commandToValueDictionary = GetCommandLineArguments();
        commandToValueDictionary.TryGetValue(BuildVersionCommandQuest, out buildVersion);
        commandToValueDictionary.TryGetValue(BuildVersionCodeCommandQuest, out buildVersionCode);
        commandToValueDictionary.TryGetValue(BuildApkNameCommandQuest, out buildNameForQuest);
        */

        string[] defaultScene = { "Assets/Scenes/Scene1.unity", "Assets/Scenes/Scene2.unity", "Assets/Scenes/Scene3.unity" };
        // BuildPipeline.BuildPlayer(defaultScene, "../BuildsOutput/Quest/AutomatedBuildQuest.apk", BuildTarget.Android, BuildOptions.None);



        BuildPlayerOptions buildPlayerOptionsAndroid = new BuildPlayerOptions();
        buildPlayerOptionsAndroid.scenes = defaultScene;
        //buildPlayerOptionsAndroid.locationPathName = "../BuildsOutput/Quest/" + buildNameForQuest + ".apk";
        buildPlayerOptionsAndroid.locationPathName = "/BuildsOutput/Quest/AutomatedBuildQuest02.apk";
        buildPlayerOptionsAndroid.target = BuildTarget.Android;
        buildPlayerOptionsAndroid.options = BuildOptions.None;

        PlayerSettings.Android.useCustomKeystore = false;
        /*
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = "Assets/Others/KeyStore/userAutoMatedBuild.keystore";
        PlayerSettings.Android.keystorePass = "8dsp?Q7mS87fHxRG";
        PlayerSettings.Android.keyaliasName = "district m key";
        PlayerSettings.Android.keyaliasPass = "8dsp?Q7mS87fHxRG";

        PlayerSettings.bundleVersion = buildVersion;
        PlayerSettings.Android.bundleVersionCode = int.Parse(buildVersionCode);

        */

        BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptionsAndroid);
        BuildSummary buildSummary = buildReport.summary;
    }

    public static void BuildWindows()
    {
        string buildVersion = "1", buildNameForPCVR = "AutomatedBuildWindows";

        string[] defaultScene = { "Assets/Scenes/Scene1.unity", "Assets/Scenes/Scene2.unity", "Assets/Scenes/Scene3.unity" };

        commandToValueDictionary.TryGetValue(BuildVersionCodeCommandPCVR, out buildVersion);
        commandToValueDictionary.TryGetValue(BuildNameCommandPCVR, out buildNameForPCVR);

        BuildPlayerOptions buildPlayerOptionsWindows = new BuildPlayerOptions();
        buildPlayerOptionsWindows.scenes = defaultScene;
        buildPlayerOptionsWindows.locationPathName = "../BuildsOutput/Windows/" + buildNameForPCVR + ".exe";
        buildPlayerOptionsWindows.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptionsWindows.options = BuildOptions.None;

        PlayerSettings.bundleVersion = buildVersion;

        BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptionsWindows);
        BuildSummary buildSummary = buildReport.summary;
        // BuildPipeline.BuildPlayer(defaultScene, "./Builds/Windows/WindowsTestBuild.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
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