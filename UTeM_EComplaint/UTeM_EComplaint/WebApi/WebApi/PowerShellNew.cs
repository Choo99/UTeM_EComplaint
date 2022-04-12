using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Reflection;
using System.IO;

namespace WebApi
{
    public class PowerShellNew
    {
        private static void ExecPowerShell(string qrcode, string jenis)
        {


            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                // use "AddScript" to add the contents of a script file to the end of the execution pipeline.
                // use "AddCommand" to add individual commands/cmdlets to the end of the execution pipeline.



                PowerShellInstance.AddScript("$Username = 'admin@utem.edu.my'");
                PowerShellInstance.AddScript("$Password = ConvertTo - SecureString 'poiu0987' - AsPlainText - Force");
                PowerShellInstance.AddScript("Set - ExecutionPolicy RemoteSigned");
                PowerShellInstance.AddScript("$Livecred = New - Object System.Management.Automation.PSCredential $Username, $Password");
                PowerShellInstance.AddScript("$Session = New - PSSession - ConfigurationName Microsoft.Exchange - ConnectionUri https://pod51000.outlook.com/powershell-liveid/ -Credential $LiveCred -Authentication Basic -AllowRedirection Import - PSSession $Session");



             //   PowerShellInstance.AddScript("param($param1) $d = get-date; $s = 'test string value'; " +
              //          "$d; $s; $param1; get-service");

                // use "AddParameter" to add a single parameter to the last command/script on the pipeline.
             //   PowerShellInstance.AddParameter("param1", "parameter 1 value!");
                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();

                // loop through each output object item
                foreach (PSObject outputItem in PSOutput)
                {
                    // if null object was dumped to the pipeline during the script then a null
                    // object may be present here. check for null to prevent potential NRE.
                    if (outputItem != null)
                    {
                        //TODO: do something with the output item 
                        // outputItem.BaseOBject
                    }
                }




            }

            var fileName = Path.GetFileName(Assembly.GetCallingAssembly().CodeBase);
            var configFileName = fileName + ".ps1";
            PowerShell  ps = PowerShell.Create();

            if (File.Exists(configFileName))
            {
                const string initScript = @"
                        function Add-ConfigItem($name, $value)
                        {
                            Set-Variable -Name $name -Value $value -Scope global
                        }
                    ";

                ps.AddScript(initScript);
                ps.Invoke();

                var profileScript = File.ReadAllText(configFileName);
                ps.AddScript(profileScript);
                ps.Invoke();
            }


        }




    }
    
}