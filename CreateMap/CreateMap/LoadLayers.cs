using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;

// *********************
//      Author: Erika Kamptner
//      Created Date: 3/5/2017
//      Description: The loadlayers.cs includes several methods for loading layers for the CSCL map tool. 
//
// **************************


namespace CreateMap
{
    class LoadLayers
    {
        public static void BuildMap(string m_pBUILDINGSDE, string m_pCSCLSDE, string m_pDOFTAXMAPSDE, bool isCSCL, bool isCustom,
                                    bool isAddressPoint, bool isCenterline, bool isBuilding, bool isTaxLot, bool isSave)

        { 
            //Point to map document
            IMxDocument pMxDoc;
            pMxDoc = (IMxDocument)ArcMap.Application.Document;

            IMap pMap;
            pMap = pMxDoc.FocusMap;

            //Zoom in so that layers don't take too long to load
            double x = 988217;
            double y = 192020;
            double border = 200;
            IEnvelope pEnvelope;
            pEnvelope = new EnvelopeClass { XMin = x - border, YMin = y - border, XMax = x + border, YMax = y + border };

            pMxDoc.ActiveView.Extent = pEnvelope;

            //Create layer groups
            IGroupLayer pBoundaryLayerGroup = new GroupLayerClass();
            pBoundaryLayerGroup.Name = "Boundary";
            pBoundaryLayerGroup.Expanded = true;
            pBoundaryLayerGroup.Visible = true;

            IGroupLayer pTransportationLayerGroup = new GroupLayerClass();
            pTransportationLayerGroup.Name = "Transportation";
            pTransportationLayerGroup.Expanded = true;
            pTransportationLayerGroup.Visible = true;

            IGroupLayer pPlacesLayerGroup = new GroupLayerClass();
            pPlacesLayerGroup.Name = "Places";
            pPlacesLayerGroup.Expanded = true;
            pPlacesLayerGroup.Visible = true;

            IGroupLayer pEMSLayerGroup = new GroupLayerClass();
            pEMSLayerGroup.Name = "EMS";
            pEMSLayerGroup.Expanded = true;
            pEMSLayerGroup.Visible = true;

            IGroupLayer pReferenceLayerGroup = new GroupLayerClass();
            pReferenceLayerGroup.Name = "Reference";
            pReferenceLayerGroup.Expanded = true;
            pReferenceLayerGroup.Visible = true;

            IGroupLayer pLayerGroup = new GroupLayerClass();
            pLayerGroup.Name = "Featured Layers";
            pLayerGroup.Expanded = true;
            pLayerGroup.Visible = true;

            //Load Layers into layer groups using input SDE connection using LoadCSCLFeatureClass method
            if (isCSCL)
            {
                LoadCSCLFeatureClass(isCSCL, "CityLimit", pBoundaryLayerGroup, m_pCSCLSDE); 
                LoadCSCLFeatureClass(isCSCL, "ZipCode", pBoundaryLayerGroup, m_pCSCLSDE); 
                LoadCSCLFeatureClass(isCSCL, "AssemblyDistrict", pBoundaryLayerGroup, m_pCSCLSDE); 

                LoadCSCLFeatureClass(isCSCL, "CensusBlock2010", pReferenceLayerGroup, m_pCSCLSDE);
                LoadCSCLFeatureClass(isCSCL, "CensusTract2010", pReferenceLayerGroup, m_pCSCLSDE); 
                LoadCSCLFeatureClass(isCSCL, "Elevation", pReferenceLayerGroup, m_pCSCLSDE); 

                LoadCSCLFeatureClass(isCSCL, "FireBattalion", pEMSLayerGroup, m_pCSCLSDE); 
                LoadCSCLFeatureClass(isCSCL, "FireCompany", pEMSLayerGroup, m_pCSCLSDE); 
                LoadCSCLFeatureClass(isCSCL, "FireDivision", pEMSLayerGroup, m_pCSCLSDE); 

                LoadCSCLFeatureClass(isCSCL, "Rail", pTransportationLayerGroup, m_pCSCLSDE); 
                LoadCSCLFeatureClass(isCSCL, "Subway", pTransportationLayerGroup, m_pCSCLSDE); 
                LoadCSCLFeatureClass(isCSCL, "SubwayStation", pTransportationLayerGroup, m_pCSCLSDE); 
                LoadCSCLFeatureClass(isCSCL, "TransitEntrance", pTransportationLayerGroup, m_pCSCLSDE); 

                LoadCSCLFeatureClass(isCSCL, "PointOfInterest", pPlacesLayerGroup, m_pCSCLSDE); 
                
            }

            //Load Featured Layers if CSCL map or feature selected for custom map
            if (isCSCL || isTaxLot)
            {
                LoadLayers.LoadCSCLFeatureClass(isCSCL, "Cadastral", pLayerGroup, m_pDOFTAXMAPSDE);
            }
            if (isCSCL || isCenterline)
            {
                LoadLayers.LoadCSCLFeatureClass(isCSCL, "StreetCenterline", pLayerGroup, m_pCSCLSDE);
            }
            if (isCSCL || isBuilding)
            {
                LoadLayers.LoadCSCLFeatureClass(isCSCL, "Building", pLayerGroup, m_pBUILDINGSDE); 
            }
            if (isCSCL || isAddressPoint)
            {
                LoadLayers.LoadCSCLFeatureClass(isCSCL, "AddressPoint", pLayerGroup, m_pCSCLSDE); //0
            }

            //If cscl, load group layers 
            if (isCSCL)
            {
                pMap.AddLayer(pBoundaryLayerGroup);
                pMap.AddLayer(pReferenceLayerGroup);
                pMap.AddLayer(pTransportationLayerGroup);
                pMap.AddLayer(pPlacesLayerGroup);
                pMap.AddLayer(pEMSLayerGroup);
                pMap.AddLayer(pLayerGroup);
            }
            
            //Update map contents and refresh
            IActiveView pActiveView;
            pActiveView = (IActiveView)pMap;
            pActiveView.Extent = pEnvelope;
            pActiveView.Refresh();
            pMxDoc.UpdateContents();

            //Prompt save directory if isSave checkbox selected
            if(isSave)
            {
                UID uid = new UIDClass();
                uid.Value = "esriArcMapUI.MxFileMenuItem";
                uid.SubType = 3;
                ICommandItem item = ArcMap.Application.Document.CommandBars.Find(uid, false);
                item.Execute();
            }
            

        }


        // *********************
        //      Author: Erika Kamptner
        //      Created Date: 3/10/2017
        //      Description: This method takes several parameters from the BuildMap method to connect to geodatabase, load layer, 
        //      place in appropriate group layer, style, and label. Only Address points, Cadastral, Building, and Centerline layers
        //      have specified labels and symbology. 
        //
        // **************************

        public static void LoadCSCLFeatureClass(bool isCSCL, string strFCName, IGroupLayer pGroupLayer, string strGeodatabase)
        {
            //Point to map document
            IMxDocument pMxDoc;
            pMxDoc = (IMxDocument)ArcMap.Application.Document;

            IMap pMap;
            pMap = pMxDoc.FocusMap;

            //Set up shapefile Workspace connection
            IWorkspaceFactory pWorkspaceFactory;
            pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();

            //If connecting to SDE, set up SDE workspace connection
            //IWorkspaceFactory pWorkspaceFactory;
            //pWorkspaceFactory = new SdeWorkspaceFactoryClass();         

            IWorkspace pWorkspace;
            pWorkspace = pWorkspaceFactory.OpenFromFile(strGeodatabase, 0);

            IFeatureWorkspace pFeatureWorkspace;
            pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;

            IFeatureClass pFeatureClass;
            pFeatureClass = pFeatureWorkspace.OpenFeatureClass(strFCName);

            IFeatureLayer pFeatureLayer;
            pFeatureLayer = new FeatureLayer();

            pFeatureLayer.FeatureClass = pFeatureClass;
            pFeatureLayer.Name = pFeatureClass.AliasName;

            //Add layer to group if CSCL map type. Otherwise, just add layer.
            if (isCSCL)
            {
                pGroupLayer.Add((ILayer)pFeatureLayer);
            }
            else
            {
                pMap.AddLayer((ILayer)pFeatureLayer);
            }

            //Symbolize Address points based if the address has subaddresses
            if (pFeatureLayer.Name == "AddressPoint")
            {
                ILayer pLayer;
                pLayer = (ILayer)pFeatureLayer;

                IGeoFeatureLayer pGeoFeatureLayer;
                pGeoFeatureLayer = (IGeoFeatureLayer)pLayer;

                pGeoFeatureLayer.DisplayAnnotation = true;

                //Create unique symbology based on subaddress flag using unique value renderer
                ISimpleMarkerSymbol pAddressSymbolDefault;
                pAddressSymbolDefault = new SimpleMarkerSymbolClass();

                pAddressSymbolDefault.Color = SetColor(230, 0, 0);
                pAddressSymbolDefault.Style = esriSimpleMarkerStyle.esriSMSCircle;
                pAddressSymbolDefault.Size = 4;

                IUniqueValueRenderer pUniqueValueRenderer;
                pUniqueValueRenderer = new UniqueValueRendererClass();

                pUniqueValueRenderer.FieldCount = 1;
                pUniqueValueRenderer.set_Field(0, "SUBADDRESS");
                pUniqueValueRenderer.UseDefaultSymbol = false;

                //Create unique symbology for different values
                ISimpleMarkerSymbol pAddressSymbolYES;
                pAddressSymbolYES = new SimpleMarkerSymbolClass();

                pAddressSymbolYES.Color = SetColor(230, 0, 0);
                pAddressSymbolYES.Style = esriSimpleMarkerStyle.esriSMSCircle;
                pAddressSymbolYES.Size = 8;
                pAddressSymbolYES.OutlineSize = 2;
                pAddressSymbolYES.OutlineColor = SetColor(255,255,0);
                pAddressSymbolYES.Outline = true;

                ISimpleMarkerSymbol pAddressSymbolNO;
                pAddressSymbolNO = new SimpleMarkerSymbolClass();

                pAddressSymbolNO.Color = SetColor(230, 0, 0);
                pAddressSymbolNO.Style = esriSimpleMarkerStyle.esriSMSCircle;
                pAddressSymbolNO.Size = 4;              

                pUniqueValueRenderer.AddValue("YES", "SUBADDRESS", pAddressSymbolYES as ISymbol);
                pUniqueValueRenderer.set_Label("YES", "YES");
                pUniqueValueRenderer.set_Symbol("YES", pAddressSymbolYES as ISymbol);

                pUniqueValueRenderer.AddValue("NO", "SUBADDRESS", pAddressSymbolNO as ISymbol);
                pUniqueValueRenderer.set_Label("NO", "NO");
                pUniqueValueRenderer.set_Symbol("NO", pAddressSymbolNO as ISymbol);

                //Create Label
                LabelFeatures(pGeoFeatureLayer, "\"ADDRESSID: \" + [ADDRESS_ID]");

                pGeoFeatureLayer.Renderer = (IFeatureRenderer)pUniqueValueRenderer;
            }

            else if (pFeatureLayer.Name == "Building")
            {
                ILayer pLayer;
                pLayer = (ILayer)pFeatureLayer;

                IGeoFeatureLayer pGeoFeatureLayer;
                pGeoFeatureLayer = (IGeoFeatureLayer)pLayer;

                pGeoFeatureLayer.DisplayField = "BIN";
                pGeoFeatureLayer.DisplayAnnotation = true;

                //Create ouline element
                ISimpleLineSymbol pOutline;
                pOutline = new SimpleLineSymbolClass();
                pOutline.Color = SetColor(0, 0, 255);
                pOutline.Width = 2;

                //Create polygon with outline element
                ISimpleFillSymbol pBuildingSymbol;
                pBuildingSymbol = new SimpleFillSymbolClass();
                pBuildingSymbol.Style = esriSimpleFillStyle.esriSFSHollow;
                pBuildingSymbol.Outline = pOutline;

                //Create Label
                LabelFeatures(pGeoFeatureLayer, "\"BIN : \" + [BIN]");

                //Render features
                ISimpleRenderer pSimpleRenderer;
                pSimpleRenderer = new SimpleRenderer();
                pSimpleRenderer.Symbol = (ISymbol)pBuildingSymbol;

                pGeoFeatureLayer.Renderer = (IFeatureRenderer)pSimpleRenderer;
            }

            else if (pFeatureLayer.Name == "StreetCenterline")
            {
                ILayer pLayer;
                pLayer = (ILayer)pFeatureLayer;

                IGeoFeatureLayer pGeoFeatureLayer;
                pGeoFeatureLayer = (IGeoFeatureLayer)pLayer;

                pGeoFeatureLayer.DisplayField = "ST_NAME";
                pGeoFeatureLayer.DisplayAnnotation = true;

                //Create arrow element to place at end of line segment
                IArrowMarkerSymbol arrowMarkerSymbol = new ArrowMarkerSymbolClass();
                arrowMarkerSymbol.Color = SetColor(0, 0, 0);
                arrowMarkerSymbol.Size = 6;
                arrowMarkerSymbol.Length = 8;
                arrowMarkerSymbol.Width = 6;
                arrowMarkerSymbol.XOffset = 0.8;

                //Create cartographic line symbol
                ICartographicLineSymbol pCartographicLineSymbol;
                pCartographicLineSymbol = new CartographicLineSymbolClass();
                pCartographicLineSymbol.Color = SetColor(0, 0, 0);
                pCartographicLineSymbol.Width = 2;
                
                //Place arrow at end of line
                ISimpleLineDecorationElement pCenterlineDecoration;
                pCenterlineDecoration = new SimpleLineDecorationElementClass();
                pCenterlineDecoration.AddPosition(1);
                pCenterlineDecoration.MarkerSymbol = arrowMarkerSymbol;
                pCenterlineDecoration.Rotate = true;

                //set line decoration
                ILineDecoration pLineDecoration;
                pLineDecoration = new LineDecorationClass();
                pLineDecoration.AddElement(pCenterlineDecoration);                

                //Set line properties  
                ILineProperties lineProperties = (ILineProperties)pCartographicLineSymbol;
                lineProperties.LineDecoration = pLineDecoration;
                lineProperties.Flip = false;

                //Create Label
                LabelFeatures(pGeoFeatureLayer, "[L_LOW_HN] + \" - \" + [L_HIGH_HN]+ \"   \" +[ST_NAME] + vbCrLf + [PHYSICALID]");

                //Render features
                ISimpleRenderer pSimpleRenderer;
                pSimpleRenderer = new SimpleRenderer();
                pSimpleRenderer.Symbol = (ISymbol)pCartographicLineSymbol; 

                pGeoFeatureLayer.Renderer = (IFeatureRenderer)pSimpleRenderer;
            }
            else if (pFeatureLayer.Name == "Cadastral")
            {
                ILayer pLayer;
                pLayer = (ILayer)pFeatureLayer;

                IGeoFeatureLayer pGeoFeatureLayer;
                pGeoFeatureLayer = (IGeoFeatureLayer)pLayer;

                pGeoFeatureLayer.DisplayField = "BBL";
                pGeoFeatureLayer.DisplayAnnotation = true;

                //Create ouline element
                ISimpleLineSymbol pOutline;
                pOutline = new SimpleLineSymbolClass();
                pOutline.Color = SetColor(76,230,0);
                pOutline.Width = 2;

                //Create polygon with outline element
                ISimpleFillSymbol pTaxLotSymbol;
                pTaxLotSymbol = new SimpleFillSymbolClass();
                pTaxLotSymbol.Style = esriSimpleFillStyle.esriSFSHollow;
                pTaxLotSymbol.Outline = pOutline;

                //Create label
                LabelFeatures(pGeoFeatureLayer, "\"BBL: \" + [BBL]");

                //Render features
                ISimpleRenderer pSimpleRenderer;
                pSimpleRenderer = new SimpleRenderer();
                pSimpleRenderer.Symbol = (ISymbol)pTaxLotSymbol;

                pGeoFeatureLayer.Renderer = (IFeatureRenderer)pSimpleRenderer;
            }
            else
            {
                pFeatureLayer.Visible = false;
            }


        }


        // *********************
        //      Author: Erika Kamptner
        //      Created Date: 3/13/2017
        //      Description: The SetColor function is used throughout the LoadCSCLFeatureClass method by using input red
        //      green, blue values and returning an IRgbColor type. 
        //
        // **************************
        public static IRgbColor SetColor(int red, int green, int blue)
        {
            IRgbColor pRgbColor;
            pRgbColor = new RgbColor();

            pRgbColor.Red = red;
            pRgbColor.Green = green;
            pRgbColor.Blue = blue;

            return pRgbColor;
        }
        // *********************
        //      Author: Erika Kamptner
        //      Created Date: 3/13/2017
        //      Description: The LabelFeatures function is used throughout the LoadCSCLFeatureClass method by applying a string 
        //      expression to the pGeoFeatureLayer. 
        //
        // **************************
        public static void LabelFeatures(IGeoFeatureLayer pGeoFeatureLayer, string strExpression)
        {
            IAnnotateLayerPropertiesCollection pAnnoLayer;
            pAnnoLayer = pGeoFeatureLayer.AnnotationProperties;

            IAnnotateLayerProperties pAnnoLayerProps;
            pAnnoLayerProps = new LabelEngineLayerProperties() as IAnnotateLayerProperties;

            IElementCollection placedElements = new ElementCollectionClass();
            IElementCollection unplacedElements = new ElementCollectionClass();

            pAnnoLayer.QueryItem(0, out pAnnoLayerProps, out placedElements, out unplacedElements);

            ILabelEngineLayerProperties pLabelProps;
            pLabelProps = (ILabelEngineLayerProperties)pAnnoLayerProps;

            pLabelProps.Symbol.Color = SetColor(0, 0, 0);
            pLabelProps.Expression = strExpression;
        }

    }
}

