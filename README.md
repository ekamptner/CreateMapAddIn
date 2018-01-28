# CSCL Map Creation Tool
Created by Erika Kamptner

A version of this project was adapted for use for the NYC DoITT GIS team to allow proper SDE connections. 

The CSCL Map Creation tool is intended to create a map with New York City's City Street Centerline dataset (CSCL) elements. This repository includes the following:

* CreateMap: directory including CSCL Map Creation button .sln file

* Data: Source data for creating map.

* CSCLMapVideo: A sample video showing how the CSCL tool works with SDE connection files. The tool uses SDE connection files to connect to various servers. The map shows “SDE.DEFAULT” connections loading the select feature classes. 

## How to use tool: ##
1.	Unzip “Data” folder into desired work space.
2.	Install .esriAddIn and click the tool button to initiate the tool. The CSCL Map Creation will display a form field that will require user input.	
3.	Enter the SDE file location for each of the three databases (quotation marks are not needed). If using this tool without connecting to database, enter folder directory location. The provided “Data” folder contains 3 folders which represent separate SDE connection locations.

      Ex. C:\temp\Data\CSCL

4.	Select type of map that you would like to create. A “CSCL” map will load all data layers and a “Custom” map will allow you to pick specific layers in the following section. 

5.	If creating a “Custom” map, select layers you would like to include

6.	Select if you would like to save the final map document. If checked, a pop-up will display allowing you to save the map document. 

7.	Click “Create Map Document”
The map document will take a few moments to load all layers and apply symbology to featured layers. 

