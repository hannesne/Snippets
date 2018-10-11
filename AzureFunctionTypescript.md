1.  Install Azure Functions extension in VSCode

2.  Install Typescript and node dependencies as dev dependencies  

```json
    "devDependencies": {
        "\@types/node": "\^10.9.4",
        "typescript": "\^3.0.3"
    },
    "dependencies": {}
```

3.  Add node version setting
```
    "WEBSITE_NODE_DEFAULT_VERSION": "10.6.0"
```

4.  Add tsconfig.json file:
```json
    {

    "compilerOptions": {

    "module": "commonjs",

    "target": "es2018",

    "noImplicitAny": false,

    "strictNullChecks": true,

    "sourceMap": true

    },

    "exclude": [

    "node_modules"

    ]

    }
```
5.  Start Azure Storage Emulator

6.  Run
