using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

// *********************
//      Author: Erika Kamptner
//      Created Date: 3/5/2017
//      Description: This is the button function that loads the MapForm on click. 
// **************************

namespace CreateMap
{
    public class CreateMap : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public CreateMap()
        {
        }

        protected override void OnClick()
        {
            MapForm myForm = new MapForm();
            myForm.ShowDialog();

            ArcMap.Application.CurrentTool = null;
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }

    }

}
