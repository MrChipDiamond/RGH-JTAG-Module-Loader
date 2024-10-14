# Module Loader for Xbox 360 JTAG/RGH Consoles

This project is a **module loader** designed for Xbox 360 JTAG/RGH consoles. It allows you to load, inject, spoof, and unload modules to run custom code on your console. It is a modified version of a tool originally developed by Guido. The UI has been enhanced for a smoother user experience, and the code has been optimized for better performance.

## Features

- **Connect to Xbox 360 Console**: Establishes a JTAG/RGH connection to the console.
- **Inject Modules**: Load and inject modules into the Xbox 360's memory.
- **List Active Modules**: Displays a list of active modules on the console with detailed information.
- **Spoof Title IDs**: Temporarily spoof the console's Title ID to enable modules that otherwise wouldn't run in-game.
- **Unload Modules**: Unload any injected module.
- **Reboot Console**: Perform a cold reboot of the console.
- **Error Handling**: Enhanced error handling for module injection and connectivity issues.

## Setup and Usage

### Prerequisites

- JRPC Client library for Xbox development.
- XDevkit for Xbox 360 debugging tools.
- A JTAG/RGH modded Xbox 360.
- Visual Studio (for building the project).

### Installation

1. Clone or download the project from this repository.
2. Open the solution file (`Module_Loader.sln`) in Visual Studio.
3. Restore any missing NuGet packages or references.
4. Build the solution (`Build > Build Solution`).
5. Deploy the executable on your Windows machine.

### Usage

1. **Connect to Console**:
   - Enter the console's IP address in the text box and click the "Connect" button.
   - If connected successfully, you will see a green status message.

2. **Injecting Modules**:
   - Click "Browse" to select the module you want to inject.
   - Click "Inject" to inject the module.
   - If successful, you'll see a confirmation in the status bar.

3. **Spoofing Title ID**:
   - Check the "Spoof Title ID" checkbox to enable Title ID spoofing.
   - The console will temporarily spoof the Title ID to the dashboard for module compatibility.

4. **Listing Modules**:
   - Click "Refresh" to display all active modules on the console.
   - Module names, base addresses, and sizes will be shown in the DataGridView.

5. **Unloading Modules**:
   - Enter the name of the module to unload in the text box.
   - Click "Unload" to remove the module.

6. **Reboot Console**:
   - Click the "Reboot" button to cold reboot the console.
   - After rebooting, make sure to reconnect before loading more modules.

### Error Handling

- If a connection to the console fails, an error message will appear in red.
- Module injection failures will show a detailed error message with the COM error and HResult code, if applicable.
- Rebooting errors are also handled, and suggestions will appear in the status bar if a reboot fails.

## Important Notes

- **Spoofing Warning**: Enabling the Title ID spoofing might cause some modules to crash the game. Always use with caution, and disable spoofing if unnecessary.
- **Module Compatibility**: Not all modules may be compatible with in-game loading. If injection fails, it could be due to unsupported modules for the specific game or scenario.

## License

This project is open-source and free to use under the [MIT License](LICENSE).

## Credits

Original source code by Guido. UI modifications and performance enhancements by **Nexus**.
