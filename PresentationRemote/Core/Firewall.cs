using NetFwTypeLib;
using System;
using System.Windows;

namespace PresentationRemote.Core
{
    public static class Firewall
    {
        public static void Setup()
        {
            try
            {
                INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FWRule")!)!;

                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FwPolicy2")!)!;

                var appDir = AppContext.BaseDirectory;
                //firewallRule.ApplicationName = "//App Executable Path";
                if (firewallRule != null)
                {
                    firewallRule.ApplicationName = appDir;
                    firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                    firewallRule.Description = "Allow Presentation Remote to access firewall";
                    firewallRule.Enabled = true;
                  
                    firewallRule.InterfaceTypes = "All";
                    firewallRule.Name = $"Presentation Remote";
                    if (firewallPolicy != null)
                    {
                        try
                        {
                            firewallPolicy.Rules.Remove($"Presentation Remote");
                        }
                        catch { }
                        firewallPolicy.Rules.Add(firewallRule);
                    }
                }
            }
            catch (Exception e)
            {

                MessageBox.Show("Firewall Exception:\n" + e.Message.ToString());
            }


        }
    }
}
