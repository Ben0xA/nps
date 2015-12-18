using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace nps
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                if (args[0].ToLower() == "-encode")
                {
                    if(args.Length == 2)
                    {
                        Byte[] bytes = System.Text.Encoding.UTF8.GetBytes(args[1]);
                        Console.WriteLine(System.Convert.ToBase64String(bytes));
                    }
                    else 
                    {
                        Console.WriteLine("usage: nps.exe -encode \"& commands; separated; by; semicolons;\"");
                    }
                }
                else if (args[0].ToLower() == "-decode")
                {
                    if (args.Length == 2)
                    {
                        String cmd = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(args[1]));
                        Console.WriteLine(cmd);
                    }
                    else
                    {
                        Console.WriteLine("usage: nps.exe -decode {base_64_string}");
                    }
                }
                else 
                {
                    PowerShell ps = PowerShell.Create();
                    if (args[0].ToLower() == "-encodedcommand")
                    {
                        ps.AddScript(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(args[1])));
                    }
                    else
                    {
                        ps.AddScript(args[0]);
                    }                

                    Collection<PSObject> output = ps.Invoke();
                    if (output != null)
                    {
                        foreach (PSObject rtnItem in output)
                        {
                            Console.WriteLine(rtnItem.ToString());
                        }
                    }
                }                
            }
            else
            {
                Console.WriteLine("usage:\r\nnps.exe \"{powershell single command}\"\r\nnps.exe \"& {commands; semi-colon; separated}\"\r\nnps.exe -encodedcommand {base64_encoded_command}\r\nnps.exe -encode \"commands to encode to base64\"\r\nnps.exe -decode {base64_encoded_command}");
            }
        }
    }
}
