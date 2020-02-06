# Using Azurite for local storage when debugging #
Azurite package is described [here](https://github.com/Azure/Azurite) You can easily control it using the Azurite VSCode extension.

Alternatively, you can do this from the command line (ideal for integration tests), or as a custom task in vscode. This guide will show you how to use it in VSCode, but the commands are the same to run in the commandline. 

1. Add Azurite as a dev dependency to your project.
`npm install azurite --save-dev`

2. Add the following task to your tasks.json:
```json
{
    "type": "shell",
    "label": "run azurite",
    "command": "mkdir ./.azurite -p && npx azurite -l ./.azurite",
    "isBackground": true,
    "dependsOn": "npm build",
    "runOptions": {"runOn": "folderOpen"},
    "problemMatcher": []
}
```
This will run the azurite task automatically when you open the folder. A .azurite folder will be created for it to store data. Remember to add that to your ignore files.

3. To start it manually, press `ctrl+shift+p` -> `Run Task` -> `Run Azurite`
