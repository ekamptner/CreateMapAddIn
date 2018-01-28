using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;

namespace CreateMap
{
    class SDEConnection
    {
        public IWorkspace SDEWorkspace(string server, string instance, string user, string password, string database, string version)
        {
            IPropertySet propertySet = new PropertySetClass();
            propertySet.SetProperty("SERVER", server);
            propertySet.SetProperty("INSTANCE", instance);
            propertySet.SetProperty("DATABASE", database);
            propertySet.SetProperty("USER", user);
            propertySet.SetProperty("PASSWORD", password);
            propertySet.SetProperty("VERSION", version);

            IWorkspaceFactory workspaceFactory = new SdeWorkspaceFactoryClass();
            return workspaceFactory.Open(propertySet, 0);
        }
    }
}
