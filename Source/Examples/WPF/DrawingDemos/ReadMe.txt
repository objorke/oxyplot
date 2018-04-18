To create new demos:
1. Copy a folder under Demos/ or Workitems/
2. Rename and modify
3. Run the demo in Debug mode
4. Press F12 when running the demo to generate a new demo image in the Images/ folder
5. Add the image to the project in the Images/ folder as a resource

The demos are found by reflection, see the MainWindow.GetDemos method.
Note that the window class must be decorated by an `DemoAttribute`.