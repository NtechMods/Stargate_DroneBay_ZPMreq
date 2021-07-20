using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Library;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.ModAPI.Interfaces.Terminal;

namespace Sherbert.ZPM.RecieverNoMsg
{

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_LargeMissileTurret), false, "DroneBay")]
    public class ZPMReciever1 : MyGameLogicComponent
    {
        private IMyFunctionalBlock block;

        private bool started = false;
        
        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            block = Entity as IMyFunctionalBlock;    

            NeedsUpdate = MyEntityUpdateEnum.EACH_100TH_FRAME;

            block.EnabledChanged += CheckZPM;

        }
        
        public override void UpdateBeforeSimulation100()
        {
            started = true;

            if (block.Enabled)
                CheckZPMon();
            
        }

        private void CheckZPMon()
        {
        
      
            List<VRage.Game.ModAPI.IMySlimBlock> temp = new List<VRage.Game.ModAPI.IMySlimBlock>();
            block.CubeGrid.GetBlocks(temp, IsZPM);
            if(temp.Count != 0)
                foreach (var i in temp)
                {
                    MyObjectBuilder_BatteryBlock j = i.GetObjectBuilder() as MyObjectBuilder_BatteryBlock;

                    if (j.Enabled && j.CurrentStoredPower > 0 && j.IntegrityPercent > 0.75f)
                    {
                        return;
                    }                    

                }
              
            block.Enabled = false;
           
 
        }


        private void CheckZPM(IMyTerminalBlock thrust)
        {
            IMyFunctionalBlock block = thrust as IMyFunctionalBlock;
            if (block.Enabled )
            {
                List<VRage.Game.ModAPI.IMySlimBlock> temp = new List<VRage.Game.ModAPI.IMySlimBlock>();
                block.CubeGrid.GetBlocks(temp, IsZPM);
          //      MyAPIGateway.Utilities.ShowMessage("Me", temp.Count.ToString());
                if (temp.Count != 0)
                    foreach (var i in temp)
                    {
                       
                        MyObjectBuilder_BatteryBlock j = i.GetObjectBuilder() as MyObjectBuilder_BatteryBlock;                   
                 
                       // MyAPIGateway.Utilities.ShowMessage("percent", j.IntegrityPercent.ToString());

                        if (j.Enabled&&j.CurrentStoredPower>0&&j.IntegrityPercent>0.75f)
                        {
                            return;
                        }

                    }
                            block.Enabled = false;
            }
        }

        bool IsZPM (VRage.Game.ModAPI.IMySlimBlock block)
        {
            string testname = block.BlockDefinition.Id.SubtypeName;
            if (testname == "Phoenix_Stargate_ZPM")
                return true;
            return false;
        }


        public override void Close()
        {
            block = null;
        }
    }
}