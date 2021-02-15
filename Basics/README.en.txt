Geshe MC Master - Basic Demos Brief
========================================

File Brief
----------------------------------------
Naming rules for all sample files：<Function Name>.<Language>.gpj.The default language is Chinese without a language identifier.

Devices		- Examples of devices and interfaces
	Company.Device	- Demonstration of extending an interface through dynamic link libraries. After successful compilation, copy the folders in Bin directory to Plugins directory.

Schemas		- Examples of schemas
	Schemas	- Demonstrate the use of basic functions of schema, script call, data binding and dynamic action, etc.
	Extensions/SchemaExtension - Demonstrate the use of UserControlL reference via DLL.
	Extensions/Company.Schema.Controls - UserControl demo.

Steps		- Examples of steps
	ExtensionInterfaces/IProtocolAlgorithm.cs - Protocol Checking Algorithm's Interface for Extending Protocol Checking Algorithms
	Extensions/StepExtension - Demonstrate how to extend a protocol checking algorithm and Windows dialog box by means of dynamic link libraries
	Steps	- Demonstrate the use of basic steps and provide some examples of comprehensive testing

Variants	- Examples of variants
	Variants	- Demonstrate the use of basic and extended variables
	Extensions/Company.Variant.Analytics - Demonstration of extending a variant analysis function through plugins. After successful compilation, copy the folders in Bin directory to Plugins directory. The function's entry is in the Basics menu.

Run Demos
----------------------------------------
All the examples have their own simulators, which can run without equipment through simulation.

Serial Port Version：It is necessary to use serial virtual software, such as VSPD, to simulate a pair of serial ports. If the virtual serial port is different from the predefined one, you can modify either the example serial port or the virtual serial port.
Network Version    ：Use of local IP address 127.0.0.1, if the port number is occupied by other local software, then modify the port number of the example network port.