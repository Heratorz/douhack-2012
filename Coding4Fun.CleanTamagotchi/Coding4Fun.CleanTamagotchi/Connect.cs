using System;
using System.Linq;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Serialization;
using System.Text;
using System.Net;
using Microsoft.VisualStudio.CommandBars;
using NUnit.Core;
using System.Security;
using System.Security.Permissions;

namespace Coding4Fun.CleanTamagotchi
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        private EnvDTE.SolutionEvents globalSolutionEvents;
        private EnvDTE.BuildEvents globalBuildEvents;
        private string[] logs = new[] { "1.xml", "2.xml" };
        private string target;

        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _applicationObject = (DTE2)application;
            _addInInstance = (AddIn)addInInst;

            globalSolutionEvents = (EnvDTE.SolutionEvents)((Events2)_applicationObject.Events).SolutionEvents;
            globalSolutionEvents.Opened += new _dispSolutionEvents_OpenedEventHandler(SolutionOpened);
            globalSolutionEvents.BeforeClosing += new _dispSolutionEvents_BeforeClosingEventHandler(SolutionBeforeClosing);
            globalBuildEvents = (EnvDTE.BuildEvents)((Events2)_applicationObject.Events).BuildEvents;
            globalBuildEvents.OnBuildDone += new _dispBuildEvents_OnBuildDoneEventHandler(SolutionBuildDone);

            //if (connectMode == ext_ConnectMode.ext_cm_UISetup)
            //{
            //    object[] contextGUIDS = new object[] { };
            //    Commands2 commands = (Commands2)_applicationObject.Commands;
            //    string toolsMenuName = "Tools";

            //    //Place the command on the tools menu.
            //    //Find the MenuBar command bar, which is the top-level command bar holding all the main menu items:
            //    Microsoft.VisualStudio.CommandBars.CommandBar menuBarCommandBar = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)["MenuBar"];

            //    //Find the Tools command bar on the MenuBar command bar:
            //    CommandBarControl toolsControl = menuBarCommandBar.Controls[toolsMenuName];
            //    CommandBarPopup toolsPopup = (CommandBarPopup)toolsControl;

            //    //This try/catch block can be duplicated if you wish to add multiple commands to be handled by your Add-in,
            //    //  just make sure you also update the QueryStatus/Exec method to include the new command names.
            //    try
            //    {
            //        //Add a command to the Commands collection:
            //        Command command = commands.AddNamedCommand2(_addInInstance, "Coding4Fun.CleanTamagotchi", "CleanTamagotchi", "Executes the command for CleanTamagotchi", true, 59, ref contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);

            //        //Add a control for the command to the tools menu:
            //        if ((command != null) && (toolsPopup != null))
            //        {
            //            command.AddControl(toolsPopup.CommandBar, 1);
            //        }
            //    }
            //    catch (System.ArgumentException ex)
            //    {
            //        //If we are here, then the exception is probably because a command with that name
            //        //  already exists. If so there is no need to recreate the command and we can 
            //        //  safely ignore the exception.
            //    }
            //}
        }

        private void SolutionOpened()
        {
            target = GetTargetPath();
            foreach (var log in logs)
            {
                LogManager.RemoveLog(target + log);
            }
            LogManager.CreateLog(target, logs[0], GetXslPath(), TestRun());
        }

        private void SolutionBeforeClosing()
        {
            LogManager.CreateLog(target, logs[1], GetXslPath(), TestRun());
            var loyalty = GetDiffMessageValue();
            SendLoyalty(loyalty);
        }

        private void SolutionBuildDone(vsBuildScope Scope, vsBuildAction Action)
        {
            SolutionBeforeClosing();
            SolutionOpened();
        }

        private string GetXslPath()
        {
            var fullPath = this.GetType().Assembly.Location;
            string theDirectory = Path.GetDirectoryName(fullPath);
            return Path.Combine(theDirectory, "MessageTransformator.xslt");
        }

        private string GetTargetPath()
        {
            Project project = ((Array)_applicationObject.ActiveSolutionProjects).GetValue(0) as Project;
            Properties projectProperties = project.Properties;
            Properties configurationProperties = project.ConfigurationManager.ActiveConfiguration.Properties;
            string projectDirectory = Path.GetDirectoryName(project.FullName);
            string outputPath = configurationProperties.Item("OutputPath").Value.ToString();
            string outputFile = projectProperties.Item("OutputFileName").Value.ToString();

            string outDir = Path.Combine(projectDirectory, outputPath);
            return Path.Combine(outDir, outputFile);
        }

        private int GetDiffMessageValue()
        {
            var log1 = LogManager.GetLog(target + logs[0]);
            var log2 = LogManager.GetLog(target + logs[1]);
            var messages1 = log1.Messages;
            var messages2 = log2.Messages;
            var addedMessages = messages2.Except(messages1, new MessageComparer());
            var fixedMessages = messages1.Except(messages2, new MessageComparer());
            return AccounterOfCoef.GetPositiveCoef(fixedMessages.ToArray())
                + AccounterOfCoef.GetNegativeCoef(addedMessages.ToArray())
                + AccounterOfCoef.GetTestCoef(log1.IsGreen, log2.IsGreen);
        }

        private bool TestRun()
        {
            //TestResult testResult = null;
            //RemoteTestRunner remoteTestRunner = null;
            //try
            //{
            //    remoteTestRunner = new RemoteTestRunner();
            //    remoteTestRunner.Load(new TestPackage(@"c:\Users\Aquila\Documents\PrimeNumberGeneratorTests.dll"));
            //    testResult = remoteTestRunner.Run(new NullListener(), TestFilter.Empty, true, LoggingThreshold.All);
            //}
            //catch
            //{
            //    return true;
            //}
            //finally
            //{
            //    remoteTestRunner.Dispose();
            //}
            //return testResult.IsSuccess;
            return true;
        }

        private void SendLoyalty(int loyalty)
        {
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create("http://192.168.9.104:42424/loyalty/");

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(string.Format("loyalty={0}", loyalty));

            httpWReq.Method = "PUT";
            httpWReq.ContentType = "application/x-www-form-urlencoded;";
            httpWReq.ContentLength = data.Length;

            using (Stream newStream = httpWReq.GetRequestStream())
            {
                newStream.Write(data, 0, data.Length);
            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        private DTE2 _applicationObject;
        private AddIn _addInInstance;

        public void Exec(string CmdName, vsCommandExecOption ExecuteOption, ref object VariantIn, ref object VariantOut, ref bool Handled)
        {
            Handled = false;
            if (ExecuteOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                if (CmdName == "Coding4Fun.CleanTamagotchi")
                {
                    Handled = true;
                    var form = new SettingsForm();
                    form.Show();
                    return;
                }
            }
        }

        public void QueryStatus(string CmdName, vsCommandStatusTextWanted NeededText, ref vsCommandStatus StatusOption, ref object CommandText)
        {
            if (NeededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                if (CmdName == "Coding4Fun.CleanTamagotchi")
                {
                    StatusOption = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
            }
        }
    }
}